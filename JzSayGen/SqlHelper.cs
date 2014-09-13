using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace JzSayGen
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        ///  默认数据连接字符串 推荐在 Application_Start 中设置
        /// </summary>
        public static string DB_CONN_STRING { get; set; }

        /// <summary>
        /// 列表数据查询
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>List T</returns>
        public static List<T> AdapterQuery<T>(string connectionstr, string sql, params SqlParameter[] parameters) where T : class,new()
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, connectionstr))
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
        public static List<T> AdapterQuery<T>(string sql, params SqlParameter[] parameters) where T : class,new()
        {
            return AdapterQuery<T>(SqlHelper.DB_CONN_STRING, sql, parameters);
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
        public static DataTable AdapterTable(string selectSql, List<SqlParameter> parameters, string orderOver, int showCount, int pageNumber)
        {
            if (pageNumber < 1 || showCount < 1) throw new ArgumentOutOfRangeException("pageNumber or showCount error");

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT * FROM ( ");
            sql.AppendLine(" SELECT ROW_NUMBER() OVER(ORDER BY " + orderOver + ") iRowNum, * FROM (" + selectSql + ") tempData ");
            sql.AppendLine(" ) OrderData ");
            sql.AppendLine(" WHERE iRowNum BETWEEN @iRowCount*(@iPageNo-1)+1 AND @iRowCount*@iPageNo ;");

            parameters.Add(new SqlParameter("@iRowCount", showCount));
            parameters.Add(new SqlParameter("@iPageNo", pageNumber));

            using (SqlDataAdapter sda = new SqlDataAdapter(sql.ToString(), SqlHelper.DB_CONN_STRING))
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
        public static DataTable AdapterTable(string selectSql, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(selectSql, SqlHelper.DB_CONN_STRING))
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
        public static T ADOExecuteSingle<T>(string connectionstr, string sql, params SqlParameter[] parameters) where T : class,new()
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, connectionstr))
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
        public static T ADOExecuteSingle<T>(string sql, params SqlParameter[] parameters) where T : class,new()
        {
            return ADOExecuteSingle<T>(SqlHelper.DB_CONN_STRING, sql, parameters);
        }

        /// <summary>
        /// 单行单列数据记录
        /// </summary>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>查询结果</returns>
        public static string ADOExecuteScalar(string connectionstr, string sql, params SqlParameter[] parameters)
        {
            string rc = "";
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
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
        public static string ADOExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            return ADOExecuteScalar(SqlHelper.DB_CONN_STRING, sql, parameters);
        }

        /// <summary>
        /// 执行无返回的sql语句
        /// </summary>
        /// <param name="connectionstr">连接数据库的字符串</param>
        /// <param name="sql">执行的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的数据行数</returns>
        public static int ADOExecuteNone(string connectionstr, string sql, params SqlParameter[] parameters)
        {
            int rc = 0;
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
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
        public static int ADOExecuteNone(string sql, params SqlParameter[] parameters)
        {
            return ADOExecuteNone(SqlHelper.DB_CONN_STRING, sql, parameters);
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
        public static List<T> DBShowList<T>(string connectionstr, string selectSql, List<SqlParameter> sqlParameters, string orderOver, int showCount, int pageNumber) where T : class,new()
        {
            if (pageNumber < 1 || showCount < 1) throw new ArgumentOutOfRangeException("pageNumber or showCount error");

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT * FROM ( ");
            sql.AppendLine(" SELECT ROW_NUMBER() OVER(ORDER BY " + orderOver + ") iRowNum, * FROM (" + selectSql + ") tempData ");
            sql.AppendLine(" ) OrderData ");
            sql.AppendLine(" WHERE iRowNum BETWEEN @iRowCount*(@iPageNo-1)+1 AND @iRowCount*@iPageNo ;");

            sqlParameters.Add(new SqlParameter("@iRowCount", showCount));
            sqlParameters.Add(new SqlParameter("@iPageNo", pageNumber));

            return SqlHelper.AdapterQuery<T>(connectionstr, sql.ToString(), sqlParameters.ToArray());
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
        public static List<T> DBShowList<T>(string selectSql, List<SqlParameter> sqlParameters, string orderOver, int showCount, int pageNumber) where T : class,new()
        {
            return DBShowList<T>(SqlHelper.DB_CONN_STRING, selectSql, sqlParameters, orderOver, showCount, pageNumber);
        }

    }
}
