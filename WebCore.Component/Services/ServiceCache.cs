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
using WebCore.Common.Cache;

namespace WebCore.Component.Services
{
    public class ServiceCache
    {
        private OptionsCache options;
        private IServiceCollection services;
        public ServiceCache(IServiceCollection _services, OptionsCache _options) {
            options = _options;
            services = _services;
        }

        /// <summary>
        /// 注入cache
        /// </summary>
        public void LoadCache()
        {
            services.AddSingleton(service =>
            {
                Type type = Type.GetType($"{options.AssemblyName}.{options.ClassName},{options.AssemblyName}");
                var cache= (ICache)Activator.CreateInstance(type);
                cache.Expiration = options.Expiration;
                cache.Enable = options.Enable;
                return cache;
            });
        }

    }


}
