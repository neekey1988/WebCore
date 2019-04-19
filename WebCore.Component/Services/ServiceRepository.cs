using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebCore.Domain;
using WebCore.Common.Logging;
using WebCore.Component.Options;

namespace WebCore.Component.Services
{
    public class ServiceRepository
    {
        private OptionsRepository options;
        private IServiceCollection services;
        public ServiceRepository(IServiceCollection _services, OptionsRepository _options) {
            options = _options;
            services = _services;
        }

        /// <summary>
        /// 注入所有dbcontext
        /// </summary>
        /// <param name="dictOptions">多个dbcontext的配置项，传递到对应的dbcontext进行配置</param>
        public void LoadDbContext(Dictionary<string, Action<DbContextOptionsBuilder>> dictOptions) {
            services.AddScoped<IUnitOfWork>(service=> {
                UnitOfWork unitOfWork = new UnitOfWork();
                foreach (var item in this.options.DbContexts)
                {
                    Type type = Type.GetType($"WebCore.Entity.{item.Key},WebCore.Entity");
                    unitOfWork[type] = (DbContext)Activator.CreateInstance(type, dictOptions[item.Key]);
                }
                return unitOfWork;
            });
        }
        /// <summary>
        /// 注入所有repository
        /// </summary>
        public void LoadRepository() {
            Type t_baseInterface = typeof(IRepository<>);
            Assembly assembly1 = Assembly.Load("WebCore.Domain");
            //找到WebCore.Domain下所有继承了IRepository的接口，
            var t_IRepository =assembly1.GetTypes().Where(t =>t.IsInterface&&t!=t_baseInterface&& t.GetInterfaces().Any(x => t_baseInterface == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)));
            //找到WebCore.Domain下所有继承了IRepository的类，
            var t_CRepository = assembly1.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Any(x => t_baseInterface == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)));
            //比对itemCR是否继承于itemIR，是的话就注入
            foreach (var itemCR in t_CRepository)
            {
                foreach (var itemIR in t_IRepository)
                {
                    if (itemIR.IsAssignableFrom(itemCR))
                        services.Add(new ServiceDescriptor(itemIR,itemCR, ServiceLifetime.Scoped));
                }
            }
        }
    }


}
