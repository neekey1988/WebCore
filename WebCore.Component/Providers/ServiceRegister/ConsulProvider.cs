using Consul;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Common.Logging;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.ServiceRegister
{
    public class ConsulProvider : IServiceRegisterProvider
    {
        public ILogHelper log = new Log4NetHelper();

        public void Register(IApplicationLifetime lifetime, OptionsServiceInfo serviceInfo,OptionsHealth health)
        {
            if (!serviceInfo.Enable)
                return;
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceInfo.RegisterIP}:{serviceInfo.RegisterPort}"));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(serviceInfo.Interval),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceInfo.ServiceIP}:{serviceInfo.ServicePort}{health.Router}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceInfo.ServiceName,
                Address = serviceInfo.ServiceIP,
                Port = serviceInfo.ServicePort,
                Tags = new[] { $"urlprefix-/{serviceInfo.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            try
            {
                consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
                });
            }
            catch (Exception ex)
            {
                log.InfoAsync("注册到Consul异常：" + ex.Message);
            }

        }
    }
}
