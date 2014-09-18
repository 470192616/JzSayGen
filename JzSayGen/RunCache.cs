using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace JzSayGen
{
    /// <summary>
    /// 运行时缓存
    /// </summary>
    public class RunCache
    {
        /// <summary>
        /// 创建缓存项
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="val">缓存对象</param>
        public static void Set(string key, object val)
        {            
            HttpRuntime.Cache.Insert(key, val);
        }

        /// <summary>
        /// 创建文件依赖缓存项
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="val">缓存对象</param>
        /// <param name="fileName">依赖文件的绝对路径</param>
        public static void Set(string key, object val, string fileName)
        {
            HttpRuntime.Cache.Insert(key, val, new CacheDependency(fileName));
        }

        /// <summary>
        /// 创建指定过期分钟数缓存项
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="val">缓存对象</param>
        /// <param name="minutes">过期分钟数</param>
        public static void Set(string key, object val, int minutes)
        {
            HttpRuntime.Cache.Insert(key, val, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static object Get(string key)
        {            
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存 没有找到返回默认值
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存Key</param>
        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

    }
}
