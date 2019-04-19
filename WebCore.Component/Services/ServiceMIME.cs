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
using Microsoft.AspNetCore.StaticFiles;

namespace WebCore.Component.Services
{
    public class ServiceMIME
    {
        private OptionsMIME options;
        private IServiceCollection services;
        public ServiceMIME(IServiceCollection _services, OptionsMIME _options) {
            options = _options;
            services = _services;
        }

        /// <summary>
        /// 添加MIME
        /// </summary>
        public void AddMIME() {
            var provider = new FileExtensionContentTypeProvider();
            if (this.options.DictMIME!=null)
                foreach (var item in this.options.DictMIME)
                    provider.Mappings.Add(item);

            services.AddSingleton<FileExtensionContentTypeProvider>(service=> {
                return provider;
            });
        }
    }


}
