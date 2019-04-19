using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebCore.Entity
{
    /// <summary>
    /// 由于本项目是类库，所以dbcontext没有配置来源，所以需要添加这个文件来对dbcontext文件进行配置。这样才能通过命令的方式生成Migrations文件
    /// </summary>
    public class TestFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Sql_Test");

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseSqlServer(connectionString,
            mysqlOptions =>{mysqlOptions.CommandTimeout(60);});

            return new TestDbContext(builder.Options);
        }
    }
    public class Test2Factory : IDesignTimeDbContextFactory<Test2DbContext>
    {
        public Test2DbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Sql_Test2");

            var builder = new DbContextOptionsBuilder<Test2DbContext>();
            builder.UseSqlServer(connectionString,
            mysqlOptions => { mysqlOptions.CommandTimeout(60); });

            return new Test2DbContext(builder.Options);
        }
    }
}
