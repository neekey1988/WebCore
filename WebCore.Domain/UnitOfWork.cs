using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using WebCore.Common.Logging;

namespace WebCore.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        public ILogHelper log { get; private set; }

        /// <summary>
        /// DbContext集合
        /// </summary>
        private Dictionary<string, DbContext> dbs;
        public UnitOfWork()
        {
            dbs = new Dictionary<string, DbContext>();
            log = new Log4NetHelper();
        }


        public TFeature Get<TFeature>() where TFeature : DbContext
        {
            DbContext feature;
            return dbs.TryGetValue(typeof(TFeature).Name, out feature)
                ? (TFeature)feature
                : default(TFeature);
        }
        public void Set<TFeature>(DbContext instance) where TFeature : DbContext
        {
            dbs[nameof(TFeature)] = instance;
        }
        public DbContext this[Type key]
        {
            set
            {
                if (key.IsSubclassOf(typeof(DbContext)))
                {
                    dbs[key.Name] = value;
                }
            }
        }


        public bool SaveChanges(DbContext db)
        {
            try
            {
                return db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveChanges(DbContext db, Func<bool> action)
        {
            using (var ts = db.Database.BeginTransaction())
            {
                try
                {
                    if (!action.Invoke())
                    {
                        ts.Rollback();
                        return false;
                    }
                    ts.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return false;
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in dbs)
            {
                if (item.Value!=null)
                {
                    item.Value.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
