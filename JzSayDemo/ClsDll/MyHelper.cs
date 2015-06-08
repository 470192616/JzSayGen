using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using JzSayGen;
using System.Data;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    public static class MyHelper
    {
        /// <summary>
        ///  默认数据连接字符串 推荐在 Application_Start 中设置 server=localhost;User Id=root;password=;database=pigcms;Persist Security Info=False;
        /// </summary>
        public static string DB_CONN_STRING { get; set; }

        /// <summary>
        /// 转换字符串列表为sql参数集合 返回类似 @StrArg1,@StrArg2,@StrArg3
        /// </summary>
        /// <param name="strList">字符串列表参数</param>
        /// <param name="sp">参数对象</param>
        /// <param name="sqlPreifx">参数前缀</param>
        /// <returns></returns>
        public static string ToMySqlParameter(this List<string> strList, ref List<MySqlParameter> sp, string sqlPreifx = "StrArg")
        {
            if (strList == null || strList.Count == 0) return "";

            List<string> arg = new List<string>();
            foreach (var s in strList)
            {
                if (s.IsNullOrEmpty()) continue;
                if (arg.Contains(s)) continue;
                string a = "@" + sqlPreifx + arg.Count.ToString();
                arg.Add(a);
                sp.Add(new MySqlParameter(a, s));
            }
            return string.Join(",", arg);
        }

        /// <summary>
        /// 执行事务操作 空字符串表示处理成功，否则返回错误信息
        /// </summary>
        /// <param name="tranBlock">事务执行的语句块，空字符串表示处理成功，否则返回错误信息/param>
        /// <returns></returns>
        public static string ExecuteTransaction(params Func<MySqlCommand, string>[] tranBlock)
        {
            return ExecuteTransaction(MyHelper.DB_CONN_STRING, tranBlock);
        }

        /// <summary>
        /// 执行事务操作 空字符串表示处理成功，否则返回错误信息
        /// </summary>
        /// <param name="connectionstr"></param>
        /// <param name="tranBlock">事务执行的语句块，空字符串表示处理成功，否则返回错误信息</param>
        /// <returns></returns>
        public static string ExecuteTransaction(string connectionstr, params Func<MySqlCommand, string>[] tranBlock)
        {
            string errmsg = "";
            if (tranBlock == null || tranBlock.Count() == 0) return "没有要执行的语句块";
            using (MySqlConnection conn = new MySqlConnection(connectionstr))
            {
                conn.Open();
                //using (MySqlTransaction tx = conn.BeginTransaction(IsolationLevel.Serializable)) //所有事务顺序执行 保证数据一致、完整
                using (MySqlTransaction tx = conn.BeginTransaction()) //IsolationLevel.RepeatableRead  默认，可能出现幻读
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = tx;
                        try
                        {
                            foreach (var tran in tranBlock)
                            {
                                cmd.Parameters.Clear();
                                errmsg = tran(cmd);
                                if (string.IsNullOrEmpty(errmsg) == false)
                                {
                                    tx.Rollback();
                                    break;
                                }
                            }
                        }
                        catch (Exception exerr)
                        {
                            errmsg = exerr.ToString();
                            tx.Rollback();
                        }
                        if (string.IsNullOrEmpty(errmsg)) tx.Commit();
                        cmd.Dispose();
                    }
                }
                conn.Close();
            }
            return errmsg;
        }

        /// <summary>
        /// 列表数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>List T</returns>
        public static List<T> AdapterQuery<T>(string connectionstr, string sql, params MySqlParameter[] parameters) where T : class,new()
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sql, connectionstr))
            {
                if (parameters != null) foreach (var p in parameters) sda.SelectCommand.Parameters.Add(((ICloneable)p).Clone());
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    if (dt.Rows.Count == 0) return new List<T>();
                    sda.SelectCommand.Parameters.Clear();
                    return DataTableToModel<T>.ParseList(dt);
                }
            }
        }

        /// <summary>
        /// 列表数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static List<T> AdapterQuery<T>(string sql, params MySqlParameter[] parameters) where T : class,new()
        {
            return AdapterQuery<T>(MyHelper.DB_CONN_STRING, sql, parameters);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="parameters"></param>
        /// <param name="orderOver"></param>
        /// <param name="showCount"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static DataTable AdapterTable(string selectSql, List<MySqlParameter> parameters, string orderOver, int showCount, int pageNumber)
        {
            if (pageNumber < 1 || showCount < 1) throw new ArgumentOutOfRangeException("pageNumber or showCount error");

            string sql = string.Format("" + selectSql + " ORDER BY " + orderOver + " LIMIT {0},{1}", (pageNumber - 1) * showCount, showCount);
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sql.ToString(), MyHelper.DB_CONN_STRING))
            {
                foreach (var p in parameters) sda.SelectCommand.Parameters.Add(((ICloneable)p).Clone());
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    sda.SelectCommand.Parameters.Clear();
                    return dt;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable AdapterTable(string selectSql, params MySqlParameter[] parameters)
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter(selectSql, MyHelper.DB_CONN_STRING))
            {
                if (parameters != null) foreach (var p in parameters) sda.SelectCommand.Parameters.Add(((ICloneable)p).Clone());
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    sda.SelectCommand.Parameters.Clear();
                    return dt;
                }
            }
        }

        /// <summary>
        /// 第一行数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>T</returns>
        public static T ADOExecuteSingle<T>(string connectionstr, string sql, params MySqlParameter[] parameters) where T : class,new()
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sql, connectionstr))
            {
                if (parameters != null) foreach (var p in parameters) sda.SelectCommand.Parameters.Add(((ICloneable)p).Clone());
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    if (dt.Rows.Count == 0) return null;
                    sda.SelectCommand.Parameters.Clear();
                    return DataTableToModel<T>.ParseModel(dt.Columns, dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// 第一行数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>T</returns>
        public static T ADOExecuteSingle<T>(string sql, params MySqlParameter[] parameters) where T : class,new()
        {
            return ADOExecuteSingle<T>(MyHelper.DB_CONN_STRING, sql, parameters);
        }

        /// <summary>
        /// 单行单列数据记录
        /// </summary>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>查询结果</returns>
        public static string ADOExecuteScalar(string connectionstr, string sql, params MySqlParameter[] parameters)
        {
            string rc = "";
            using (MySqlConnection conn = new MySqlConnection(connectionstr))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(((ICloneable)p).Clone());
                    var arv = cmd.ExecuteScalar();
                    rc = arv == null ? string.Empty : arv.ToString();
                    cmd.Parameters.Clear();
                }
                conn.Close();
            }
            return rc;
        }

        /// <summary>
        /// 单行单列数据记录
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>查询结果</returns>
        public static string ADOExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            return ADOExecuteScalar(MyHelper.DB_CONN_STRING, sql, parameters);
        }

        /// <summary>
        /// 执行无返回的sql语句
        /// </summary>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">执行的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的数据行数</returns>
        public static int ADOExecuteNone(string connectionstr, string sql, params MySqlParameter[] parameters)
        {
            int rc = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionstr))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(((ICloneable)p).Clone());
                    rc = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                conn.Close();
            }
            return rc;
        }

        /// <summary>
        /// 执行无返回的sql语句
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的数据行数</returns>
        public static int ADOExecuteNone(string sql, params MySqlParameter[] parameters)
        {
            return ADOExecuteNone(MyHelper.DB_CONN_STRING, sql, parameters);
        }

        /// <summary>
        /// 数据分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="selectSql">查询语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <param name="orderOver">排序条件</param>
        /// <param name="showCount">显示的条数</param>
        /// <param name="pageNumber">显示的页数</param>
        /// <returns>实体列表</returns>
        public static List<T> DBShowList<T>(string connectionstr, string selectSql, List<MySqlParameter> sqlParameters, string orderOver, int showCount, int pageNumber) where T : class,new()
        {
            if (pageNumber < 1 || showCount < 1) throw new ArgumentOutOfRangeException("pageNumber or showCount error");

            if (orderOver.IsNullOrEmpty() == false) orderOver = " ORDER BY " + orderOver + " ";
            string sql = string.Format("" + selectSql + "" + orderOver + " LIMIT {0},{1}", (pageNumber - 1) * showCount, showCount);

            return MyHelper.AdapterQuery<T>(connectionstr, sql, sqlParameters.ToArray());
        }

        /// <summary>
        /// 数据分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="selectSql">查询语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <param name="orderOver">排序条件</param>
        /// <param name="showCount">显示的条数</param>
        /// <param name="pageNumber">显示的页数</param>
        /// <returns>实体列表</returns>
        public static List<T> DBShowList<T>(string selectSql, List<MySqlParameter> sqlParameters, string orderOver, int showCount, int pageNumber) where T : class,new()
        {
            return DBShowList<T>(MyHelper.DB_CONN_STRING, selectSql, sqlParameters, orderOver, showCount, pageNumber);
        }
    }
}