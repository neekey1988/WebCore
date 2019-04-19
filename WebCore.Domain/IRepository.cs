using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebCore.Entity;
using System.Data.Common;
using WebCore.Common.Attributes;

namespace WebCore.Domain
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, new()
    {

        /// <summary>
        /// 获取主库中符合查询条件的第一条结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 获取指定库中符合查询条件的第一条结果
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        TCEntity Get<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();
        /// <summary>
        /// 获取主库中符合查询条件的结果集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 获取指定库中符合查询条件的结果集
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        IQueryable<TCEntity> GetList<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();

        /// <summary>
        /// 获取主库中是否存在符合查询条件的数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 获取指定库中是否存在符合查询条件的数据
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();


        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <typeparam name="TContext">sql语句表对应的DbContext，如果涉及到跨库，sql里关联查询用到的其他库中的表，可以尝试用“库名.所有者.表名”的方式查询</typeparam>
        /// <typeparam name="TCEntity">返回的实体类型，如果自定义的类型，可以尝试用dynamic</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="para">sql语句中相关参数</param>
        /// <returns></returns>
        IQueryable<TCEntity> Sql<TContext, TCEntity>(string sql, params DbParameter[] para)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();

        //bool GetPage(Expression<Func<TEntity, bool>> where);
        //bool GetPage<TContext, TCEntity>(Expression<Func<TEntity, bool>> where)
        //                where TContext : DbContext
        //    where TCEntity : class, IEntity, new();



            #region 增删改
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">要添加的实体数据</param>
        /// <returns></returns>
        void Add(TEntity entity);
        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys"></param>
        void AddRange(List<TEntity> entitys);

        /// <summary>
        /// 添加指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext">dbcotext类型</typeparam>
        /// <param name="entity">要添加的实体数据</param>
        /// <returns></returns>
        void Add<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();
        /// <summary>
        /// 批量添加指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        void AddRange<TContext, TCEntity>(List<TCEntity> entitys)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// 批量修改实体
        /// </summary>
        /// <param name="entity"></param>
        void UpdateRange(List<TEntity> entitys);
        /// <summary>
        /// 修改指定DbContext的实体
        /// </summary>
        /// <param name="entity"></param>
        void Update<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();
        /// <summary>
        /// 批量修改指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        void UpdateRange<TContext, TCEntity>(List<TCEntity> entitys)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entitys"></param>
        void RemoveRange(List<TEntity> entitys);
        /// <summary>
        /// 删除指定DbContext的实体
        /// </summary>
        /// <param name="entity"></param>
        void Remove<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new();
        /// <summary>
        /// 批量删除指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entitys"></param>
        void RemoveRange<TContext, TCEntity>(List<TCEntity> entitys)
           where TContext : DbContext
           where TCEntity : class, IEntity, new();
        #endregion

        #region SaveChanges
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <typeparam name="TContext">对应的DbContext</typeparam>
        /// <returns></returns>
        bool SaveChanges<TContext>() where TContext : DbContext;

        /// <summary>
        /// 提交数据，涉及到多步操作、跨库、需要返回值的时候用
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        bool SaveChanges(Func<bool> action);
        #endregion
    }
}
