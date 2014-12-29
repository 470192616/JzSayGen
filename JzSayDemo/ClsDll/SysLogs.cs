using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using JzSayGen;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 系统事件
    /// </summary>
    public enum ELogs
    {
        /// <summary>
        /// 尝试登录系统
        /// </summary>
        [EnumTip("尝试登录系统")]
        TryLogin = 1,

        /// <summary>
        /// 登录系统成功
        /// </summary>
        [EnumTip("登录系统成功")]
        Login = 2,

        /// <summary>
        /// 退出系统
        /// </summary>
        [EnumTip("退出系统")]
        LogOut = 4
    }

    /// <summary>
    /// 系统日志
    /// </summary>
    public class SysLogs
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logData"></param>
        public static void Write(ELogs logType, string logData = "")
        {
            SqlHelper.ADOExecuteNone(string.Format("INSERT INTO [logsLib] ([WaterId] ,[EventKey] ,[EventData] ,[EventDate]) VALUES ({0},{1},@EDA,getdate()) ", MACPrimaryKey.NewKey3, (Int64)logType), new SqlParameter("@EDA", logData.GetLeft(340)));
        }
    }
}