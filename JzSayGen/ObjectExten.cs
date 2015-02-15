using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Data.SqlClient;

namespace JzSayGen
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExten
    {

        /// <summary>
        /// 日期转时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return System.Convert.ToInt64((dateTime - start).TotalSeconds);
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(timestamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts">DateTime yyyyMMddHHmmssfff</param>
        /// <returns></returns>
        public static DateTime ParseDateTimeTS(this Int64 ts)
        {
            if (ts < 20141204120412100 || ts > 20881204120412100) throw new ArgumentException("不是有效的ts数据");
            string cl = ts.ToString();
            DateTime dt;
            if (DateTime.TryParse(string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", cl.Substring(0, 4), cl.Substring(4, 2), cl.Substring(6, 2), cl.Substring(8, 2), cl.Substring(10, 2), cl.Substring(12, 2), cl.Substring(14, 3)), out dt)) return dt;
            throw new ArgumentException("不是有效的的ts数据格式");
        }

        /// <summary>
        /// sql参数调试
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static List<string> ToDebugStr(this List<SqlParameter> sp)
        {
            if (sp == null || sp.Count == 0) return new List<string>();
            List<string> s = new List<string>();
            foreach (var a in sp)
            {
                s.Add(string.Format("{0} {1} {2}", a.ParameterName, a.DbType, a.Value));
            }
            return s;
        }

        /// <summary>
        /// 转换字符串列表为sql参数集合 返回类似 @StrArg1,@StrArg2,@StrArg3
        /// </summary>
        /// <param name="strList">字符串列表参数</param>
        /// <param name="sp">参数对象</param>
        /// <param name="sqlPreifx">参数前缀</param>
        /// <returns></returns>
        public static string ToSqlParameter(this List<string> strList, ref List<SqlParameter> sp, string sqlPreifx = "StrArg")
        {
            if (strList == null || strList.Count == 0) return "";

            List<string> arg = new List<string>();
            foreach (var s in strList)
            {
                if (s.IsNullOrEmpty()) continue;
                if (arg.Contains(s)) continue;
                string a = "@" + sqlPreifx + arg.Count.ToString();
                arg.Add(a);
                sp.Add(new SqlParameter(a, s));
            }
            return string.Join(",", arg);
        }

        /// <summary>
        /// 转换对象类型
        /// </summary>
        /// <param name="value">对象值</param>
        /// <param name="destinationType">目标类型</param>
        /// <returns>目标类型的对象值</returns>
        public static object Convert(object value, Type destinationType)
        {
            if (value == null) return value;
            TypeConverter destinationConverter = TypeDescriptor.GetConverter(destinationType);
            TypeConverter sourceConverter = TypeDescriptor.GetConverter(value.GetType());
            if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(value);
            if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(value, destinationType);
            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);
            if (!destinationType.IsAssignableFrom(value.GetType()))
                try
                {
                    return System.Convert.ChangeType(value, destinationType);
                }
                catch (InvalidCastException)
                {
                    if (value is string)
                    {
                        var parseMethod = destinationType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .Where(m => m.Name == "Parse")
                            .Where(m => m.GetParameters().Length == 1)
                            .Where(m => m.GetParameters()[0].ParameterType == typeof(string))
                            .FirstOrDefault();
                        if (parseMethod != null && parseMethod.GetParameters().Length == 1 && parseMethod.GetParameters()[0].ParameterType == typeof(string))
                            return parseMethod.Invoke(null, new[] { value });
                        var constructor = destinationType.GetConstructors()
                            .Where(c => c.GetParameters().Length == 1)
                            .Where(c => c.GetParameters()[0].ParameterType == typeof(string))
                            .FirstOrDefault();
                        if (constructor != null)
                            return constructor.Invoke(new[] { value });
                    }
                    else if (value is System.DBNull)
                    {
                        return Activator.CreateInstance(destinationType);
                    }
                    throw;
                }
            return value;
        }
    }
}
