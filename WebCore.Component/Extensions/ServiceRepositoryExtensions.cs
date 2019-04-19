using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Builders;
using WebCore.Component.Options;
using WebCore.Component.Services;

namespace WebCore.Component.Extensions
{
    public static class ServiceRepositoryExtensions
    {
        /// <summary>
        /// 添加自定义的Repository服务，完成dbcontext的注入，Repository的注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddRepositorys(this IServiceCollection services, Action<OptionsRepository> action, IConfigurationRoot config)
        {
            OptionsRepository options = new OptionsRepository();
            action.Invoke(options);

            var dbOptions = new DbOptionsBuilder();
            var dictOptions = new Dictionary<string, Action<DbContextOptionsBuilder>>();
            //加载dbcontext所有的连接串和初始化所需的DbContextOptionsBuilder
            foreach (var item in options.DbContexts)
            {
                dictOptions.Add(item.Key, dbOptions.Build(config.GetConnectionString(item.Key), providerOptions => providerOptions.CommandTimeout(60), Enum.Parse<DBType>(item.Value)));
            }


            ServiceRepository srv = new ServiceRepository(services, options);
            srv.LoadDbContext(dictOptions);
            srv.LoadRepository();
        }

    }

}
