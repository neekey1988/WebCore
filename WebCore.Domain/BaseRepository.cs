using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WebCore.Entity;
using WebCore.Common.Logging;
using System.Threading.Tasks;
using System.Data.Common;

namespace WebCore.Domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">主表</typeparam>
    /// <typeparam name="TDbContext">表所属的dbcontext</typeparam>
    public class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TDbContext : DbContext
    {
        #region 属性
        /// <summary>
        /// 日志帮助类
        /// </summary>
        public ILogHelper log
        {
            get => _unitOfWork.log;
        }

        /// <summary>
        /// 当前表对应的dbcontext
        /// </summary>
        public TDbContext db { get; private set; }
        /// <summary>
        /// 工作单元，主要用于事务性提交，并可以通过Get方法获得整个项目的DbContext
        /// </summary>
        public IUnitOfWork _unitOfWork { get; private set; }

        public BaseRepository(IUnitOfWork _uow)
        {
            _unitOfWork = _uow;
            db = _unitOfWork.Get<TDbContext>();
        }
        #endregion

        /// <summary>
        /// 获取主库中符合查询条件的第一条结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
            => db.Set<TEntity>().FirstOrDefault(where);
        /// <summary>
        /// 获取指定库中符合查询条件的第一条结果
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual TCEntity Get<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity:class,IEntity,new()
            => _unitOfWork.Get<TContext>().Set<TCEntity>().FirstOrDefault(where);

        /// <summary>
        /// 获取主库中符合查询条件的结果集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> where)
            => db.Set<TEntity>().Where(where);
        /// <summary>
        /// 获取指定库中符合查询条件的结果集
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TCEntity> GetList<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity:class,IEntity,new()
            => _unitOfWork.Get<TContext>().Set<TCEntity>().Where(where);

        /// <summary>
        /// 获取主库中是否存在符合查询条件的数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> where)
            => db.Set<TEntity>().Any(where);
        /// <summary>
        /// 获取指定库中是否存在符合查询条件的数据
        /// </summary>
        /// <typeparam name="TContext">DbContext名</typeparam>
        /// <typeparam name="TCEntity">实体名</typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual bool Exists<TContext, TCEntity>(Expression<Func<TCEntity, bool>> where)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
            => _unitOfWork.Get<TContext>().Set<TCEntity>().Any(where);

        /// <summary>
        /// 执行sql查询
        /// </summary>
        /// <typeparam name="TContext">sql语句表对应的DbContext，如果涉及到跨库，sql里关联查询用到的其他库中的表，可以尝试用“库名.所有者.表名”的方式查询</typeparam>
        /// <typeparam name="TCEntity">返回的实体类型，如果自定义的类型，可以尝试用dynamic</typeparam>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="para">sql语句中相关参数</param>
        /// <returns></returns>
        public virtual IQueryable<TCEntity> Sql<TContext, TCEntity>(string sql, params DbParameter[] para)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
            => _unitOfWork.Get<TContext>().Set<TCEntity>().FromSql(sql, para);



        #region 增删改
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity) => db.Add(entity);
        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys"></param>
        public virtual void AddRange(List<TEntity> entitys) => db.AddRange(entitys);

        /// <summary>
        /// 添加指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="_TEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void Add<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().Add(entity);
        /// <summary>
        /// 批量添加指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void AddRange<TContext, TCEntity>(List<TCEntity> entitys)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().AddRange(entitys);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity) => db.Update(entity);
        /// <summary>
        /// 批量修改实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void UpdateRange(List<TEntity> entitys) => db.UpdateRange(entitys);
        /// <summary>
        /// 修改指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void Update<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().Update(entity);
        /// <summary>
        /// 批量修改指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void UpdateRange<TContext, TCEntity>(List<TCEntity> entitys)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().UpdateRange(entitys);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(TEntity entity) => db.Remove(entity);
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void RemoveRange(List<TEntity> entitys) => db.RemoveRange(entitys);
        /// <summary>
        /// 删除指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void Remove<TContext, TCEntity>(TCEntity entity)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().Remove(entity);
        /// <summary>
        /// 批量删除指定DbContext的实体
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCEntity"></typeparam>
        /// <param name="entity"></param>
        public virtual void RemoveRange<TContext, TCEntity>(List<TCEntity> entitys)
            where TContext : DbContext
            where TCEntity : class, IEntity, new()
        => _unitOfWork.Get<TContext>().Set<TCEntity>().RemoveRange(entitys);
        #endregion

        #region SaveChanges实现
        /// <summary>
        /// 提交主库的数据
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveChanges()
        {
            return _unitOfWork.SaveChanges(db);
        }
        /// <summary>
        /// 提交只顶你数据库的数据
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public virtual bool SaveChanges<TContext>() where TContext : DbContext
        {
            return _unitOfWork.SaveChanges(_unitOfWork.Get<TContext>());
        }

        /// <summary>
        /// 根据委托进行主库的上下文级别的事务性提交，不支持跨库事务。
        /// </summary>
        /// <param name="action">委托里面返回true则正常进行提交，返回false则数据回滚。</param>
        /// <returns></returns>
        public virtual bool SaveChanges(Func<bool> action)
        {
            return _unitOfWork.SaveChanges(db, action);
        }

        /// <summary>
        /// 根据委托进行泛型TContext的上下文级别的事务性提交，不支持跨库事务。
        /// </summary>
        /// <param name="action">委托里面返货true则正常进行提交，返回false则数据回滚。</param>
        /// <returns></returns>
        public virtual bool SaveChanges<TContext>(Func<bool> action) where TContext : DbContext
        {
            return _unitOfWork.SaveChanges(_unitOfWork.Get<TContext>(), action);
        }
        #endregion
    }
}
