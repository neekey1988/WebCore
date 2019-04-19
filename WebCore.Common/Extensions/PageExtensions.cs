using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WebCore.Common.Extensions;

namespace WebCore.Common.Extensions
{
    public static class PageExtensions
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_data"></param>
        /// <param name="page">当前页数</param>
        /// <param name="rows">每页几行</param>
        /// <param name="_total">当前数据总行数</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetPager<TEntity>(this IOrderedQueryable<TEntity> _data,int page, int rows,out int _total) {
            _total = _data.Count();
            return _data.Skip((page - 1) * rows).Take(rows);
        }
        /// <summary>
        /// 分页并返回json数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_data"></param>
        /// <param name="page">当前页数</param>
        /// <param name="rows">每页几行</param>
        /// <param name="_total">当前数据总行数</param>
        /// <returns></returns>
        public static string GetPagerToJson<TEntity>(this IOrderedQueryable<TEntity> _data,int page, int rows,out int _total, string dateformat = "yyyy-MM-dd") {
            _total = _data.Count();
            var data= _data.Skip((page - 1) * rows).Take(rows);
            var jsondata = new { total = _total, rows = data };
            return jsondata.ToJson(dateformat);
        }

    }
}
