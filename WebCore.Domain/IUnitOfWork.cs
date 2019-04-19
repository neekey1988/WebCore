using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Common.Logging;

namespace WebCore.Domain
{
    /// <summary>
    /// 工作单元，主要用于事务性提交，SaveChanges自带事务
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 事务帮助类
        /// </summary>
        ILogHelper log { get; }

        /// <summary>
        /// 获取指定的dbcontext
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <returns></returns>
        TFeature Get<TFeature>() where TFeature : DbContext;
        /// <summary>
        /// 设置DbContext
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="instance"></param>
        void Set<TFeature>(DbContext instance) where TFeature : DbContext;

        /// <summary>
        /// 索引器，获取指定的DbContext
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        DbContext this[Type key] { set; }

        /// <summary>
        /// 根据DbContext进行提交
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        bool SaveChanges(DbContext db);
        /// <summary>
        /// 根据委托进行上下文级别的事务性提交，不支持跨库事务。
        /// </summary>
        /// <param name="action">可以在委托中进行一些业务操作，也可以捕获具体异常</param>
        /// <returns></returns>
        bool SaveChanges(DbContext db,Func<bool> action);
    }
}
