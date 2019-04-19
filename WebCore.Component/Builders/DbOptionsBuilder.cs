using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.Component.Builders
{
    public enum DBType
    {
        sqlserver,
        oracle,
        mysql
    }
    public interface IDbOptionsBuilder
    {
        Action<DbContextOptionsBuilder> Build(string connstring, Action<SqlServerDbContextOptionsBuilder> options, DBType type);
    }

    public class DbOptionsBuilder : IDbOptionsBuilder
    {
        public Action<DbContextOptionsBuilder> Build(string connstring, Action<SqlServerDbContextOptionsBuilder> options, DBType type)
        {
            switch (type)
            {
                case DBType.sqlserver:
                    return sqloptions => sqloptions.UseSqlServer(connstring, options);
                case DBType.oracle:
                    break;
                case DBType.mysql:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
