using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Fallback;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebCore.Common.Cache;
using WebCore.Common.Enums;
using WebCore.Common.Logging;
using WebCore.Common.Share;
using WebCore.Component.Options;

namespace WebCore.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HystrixAttribute : AbstractInterceptorAttribute
    {
        public ILogHelper log = new Log4NetHelper();


        /// <summary>
        /// 是否启用熔断，局部
        /// </summary>
        public E_Hystrix IsEnableCircuitBreaker { get; set; } = E_Hystrix.None;

        /// <summary>
        /// 是否启用降级，局部
        /// </summary>
        public E_Hystrix IsEnableFallBack { get; set; } = E_Hystrix.None;
        /// <summary>
        /// 是否启用缓存，局部
        /// </summary>
        public E_Hystrix IsEnableCache { get; set; } = E_Hystrix.None;

        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;

        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;

        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;

        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;

        /// <summary>
        /// 缓存多少毫秒，针对局部的（0表示采用全局配置文件的配置），
        /// </summary>
        public double CacheTTLSeconds { get; set; } = 0;



        /// <summary>
        /// 降级函数,必须在当前函数同一类中，该函数应该和当前函数参数一样，比如当前函数逻辑代码中的接口访问不了，降级函数的逻辑及参数和当前函数一样，但接口地址变为其他可用的。
        /// </summary>
        public string FallBacHandle { get; set; } = null;

        private readonly static object lock_Hystrix = new object();
        [FromContainer]
        public IOptionsSnapshot<OptionsAppSetting> appsetting { get; set; }
        [FromContainer]
        public ICache cache { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            int methodtoken = context.ServiceMethod.MetadataToken; //System.Reflection.MethodBase.GetCurrentMethod().MetadataToken;
            this.SetGlobalConfig();
            //查询字典中是否保存过当前函数的policy
            Share.GlobalStatic.dict_policies.TryGetValue(methodtoken, out AsyncPolicy policy);

            if (policy == null)
            {
                policy=this.SetFallBackPolicy(policy, context);

                AsyncPolicy wrappolicy = null;
                wrappolicy = this.SetCircuitBreaker(wrappolicy);
                wrappolicy = this.SetRetry(wrappolicy);
                wrappolicy = this.SetTimeout(wrappolicy);

                //如果降级启用，就把降级策略和以上策略进行组合，注意组合顺序，顺序不对可能部分策略失效（不确定）
                if (IsEnableFallBack == E_Hystrix.Enable)
                    policy = policy.WrapAsync(wrappolicy);
                else
                    policy = wrappolicy;

                //添加的时候，锁住再查询一次看字典中是否存在key，如果存在，说明并发的时候加入过这个，就不需要再加入了
                lock (lock_Hystrix)
                {
                    if (!Share.GlobalStatic.dict_policies.ContainsKey(methodtoken))
                        Share.GlobalStatic.dict_policies.TryAdd(methodtoken, policy);
                }
            }
            //policy.ExecuteAsync执行的时候，把pollyCtx当参数传递过去，避免闭包问题
            Context pollyCtx = new Context();
            pollyCtx["aspectContext"] = context;

            //todo:增加缓存算法，不然内存很快爆满，OneNote里有
            //全局启用，局部启用，缓存才启用
            if (cache != null && cache.Enable && IsEnableCache == E_Hystrix.Enable)
            {
                string cacheKey = MD5Helper.MD5Encrypt32( "CM_Hystrix_" + context.ServiceMethod.DeclaringType + "." + context.ServiceMethod + string.Join("_", context.Parameters));
                //尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
                if (cache.TryGetValue(cacheKey, out var cacheValue))
                {
                    context.ReturnValue = cacheValue;
                }
                else
                {
                    //如果缓存中没有，则执行实际被拦截的方法
                    await policy.ExecuteAsync(ctx => next(context), pollyCtx);
                    //把返回值存入缓存中
                    cache.Add(cacheKey, context.ReturnValue, CacheTTLSeconds == 0 ? cache.Expiration : CacheTTLSeconds);
                }
            }
            else//如果没有启用缓存，就直接执行业务方法
            {
                if (policy == null)
                    await next(context);//直接执行
                else
                    await policy.ExecuteAsync(ctx => next(context), pollyCtx);//polly执行
            }

        }

        /// <summary>
        /// 降级
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private AsyncPolicy SetFallBackPolicy(AsyncPolicy policy, AspectContext context)
        {
            if (IsEnableFallBack == E_Hystrix.Enable)
            {
                policy = Policy.Handle<Exception>().FallbackAsync((ctx, t) =>
                {
                    //避免闭包问题，如果直接用当前函数的参数context，那么如果多次调用SetFallBackPolicy，传递的AspectContext context不一样
                    //到时候执行Policy.Handle<Exception>().FallbackAsync 里面context肯定是最后一次为准。就会出现闭包问题。
                    AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
                    object result = null;
                    if (!string.IsNullOrEmpty(this.FallBacHandle))
                    {
                        //执行降级函数
                        var minfo = context.ServiceMethod.DeclaringType.GetMethod(this.FallBacHandle);
                        result = minfo == null ? minfo : minfo.Invoke(context.Implementation, context.Parameters);
                    }
                    //不能如下这样，因为这是闭包相关，如果这样写第二次调用Invoke的时候context指向的
                    //还是第一次的对象，所以要通过Polly的上下文来传递AspectContext
                    //context.ReturnValue = fallBackResult;
                    aspectContext.ReturnValue = result;
                    return Task.CompletedTask;

                }, async (ex, t) => { });
            }
            return policy;
        }


        /// <summary>
        /// 超时
        /// </summary>
        /// <param name="wrappolicy"></param>
        /// <returns></returns>
        private AsyncPolicy SetTimeout(AsyncPolicy wrappolicy)
        {
                //超时异常，采用Pessimistic悲观策略
            if (TimeOutMilliseconds > 0 && wrappolicy != null)
                wrappolicy = wrappolicy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
            return wrappolicy;
        }

        /// <summary>
        /// 重试
        /// </summary>
        /// <param name="wrappolicy"></param>
        /// <returns></returns>
        private AsyncPolicy SetRetry(AsyncPolicy wrappolicy)
        {
            if (MaxRetryTimes > 0)
            {
                //重试策略，RetryIntervalMilliseconds标识间隔多久重试一次
                var _wrappolicy = Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds));
                if (wrappolicy == null)
                    wrappolicy = _wrappolicy;
                else
                    wrappolicy = wrappolicy.WrapAsync(_wrappolicy);
            }
            return wrappolicy;
        }

        /// <summary>
        /// 熔断
        /// </summary>
        /// <param name="wrappolicy"></param>
        /// <returns></returns>
        private AsyncPolicy SetCircuitBreaker(AsyncPolicy wrappolicy) {
            if (IsEnableCircuitBreaker == E_Hystrix.Enable)
            {
                //异常ExceptionsAllowedBeforeBreaking次后进行熔断，熔断MillisecondsOfBreak后恢复
                var _wrappolicy = Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak));
                if (wrappolicy == null)
                    wrappolicy = _wrappolicy;
                else
                    wrappolicy = wrappolicy.WrapAsync(_wrappolicy);
            }
            return wrappolicy;
        }

        /// <summary>
        /// 获取全局启用，并赋值给当前局部变量
        /// </summary>
        private void SetGlobalConfig()
        {
            if (appsetting != null && appsetting.Value != null)
            {
                if (appsetting.Value.Items != null && appsetting.Value.Items.ContainsKey("EnableCircuitBreaker"))
                {
                    //全局熔断为true，局部也是true才启用熔断
                    if (appsetting.Value.Items["EnableCircuitBreaker"].ToLower() == "true")
                        IsEnableCircuitBreaker = IsEnableCircuitBreaker == E_Hystrix.None ? E_Hystrix.Disable : IsEnableCircuitBreaker;
                    if (appsetting.Value.Items["EnableCircuitBreaker"].ToLower() == "false")
                        IsEnableCircuitBreaker =  E_Hystrix.Disable;
                }
                if (appsetting.Value.Items != null && appsetting.Value.Items.ContainsKey("EnableFallBack"))
                {
                    //全局降级为true，局部也是true才启用降级
                    if (appsetting.Value.Items["EnableFallBack"].ToLower() == "true")
                        IsEnableFallBack = IsEnableFallBack == E_Hystrix.None ? E_Hystrix.Disable : IsEnableFallBack;
                    //反之
                    if (appsetting.Value.Items["EnableFallBack"].ToLower() == "false")
                        IsEnableFallBack = E_Hystrix.Disable;
                }
            }
            if (IsEnableCircuitBreaker == E_Hystrix.None)
                IsEnableCircuitBreaker = E_Hystrix.Disable;
            if (IsEnableFallBack == E_Hystrix.None)
                IsEnableFallBack = E_Hystrix.Disable;
        }
    }
}
