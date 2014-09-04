using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace JzSayGen
{
    /// <summary>
    /// LinqHelper
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// using (ISDBDataContext db = new ISDBDataContext(this.DBConnStr))
    /// {
    ///     var sql = LinqWhereHelper.True<ShopBill>();
    ///     if (this.SearchID > 0)
    ///     {
    ///         sql = sql.And(x => x.Id == this.SearchID);
    ///     }
    ///     var tmp = db.ShopBill.Where(sql);
    ///     Int32 rsCount = tmp.Select(x => x.Id).Count();
    ///     var listData = tmp.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
    /// }
    /// ]]>
    /// </example>
    public static class LinqWhereHelper
    {
        /// <summary>
        /// 可以不需要参数
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>对象</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 必须要参数
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>对象</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// 或者操作
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="expr1">a</param>
        /// <param name="expr2">b</param>
        /// <returns>o</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// 并且操作
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="expr1">a</param>
        /// <param name="expr2">b</param>
        /// <returns>o</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
