using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace JzSayGen
{
    /// <summary>
    /// 枚举成员说明 [EnumTip("China")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTipAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Description"></param>
        public EnumTipAttribute(string Description)
        {
            this.Tip = Description;
        }
    }

    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumExten
    {
        /// <summary>
        /// 枚举转dictionary类型
        /// </summary>
        private static Dictionary<Type, Dictionary<Int32, string>> EnumDictionaryCache = new Dictionary<Type, Dictionary<Int32, string>>();

        /// <summary>
        /// 判断是否枚举成员值
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefinedInt32(Type enumType, Int32 value)
        {
            return System.Enum.IsDefined(enumType, value);
        }

        /// <summary>
        /// int32 转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseInt32<T>(Int32 value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        
        /// <summary>
        /// 带EnumTip属性的枚举转dictionary类型
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="attachKeyNum">每个key值增加的数字 主要用于枚举中有值为0的时候搜索中默认int32的值也是0做处理</param>
        /// <param name="removeKeys">要移除的key值</param>
        /// <returns></returns>
        public static Dictionary<Int32, string> GetDictionary(Type enumType, Int32 attachKeyNum, params Int32[] removeKeys)
        {                
            Dictionary<Int32, string> r = new Dictionary<int, string>();
            var dict = GetDictionary(enumType);
            foreach (var d in dict)
            {
                if (removeKeys.Any(x => x == d.Key)) continue;
                r.Add(d.Key + attachKeyNum, d.Value);
            }
            return r;
        }

        /// <summary>
        /// 带EnumTip属性的枚举转dictionary类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<Int32, string> GetDictionary(Type enumType)
        {
            if (EnumDictionaryCache.ContainsKey(enumType)) return EnumDictionaryCache[enumType];

            Dictionary<Int32, string> dit = new Dictionary<int, string>();
            foreach (int i in Enum.GetValues(enumType))
            {
                string enumName = Enum.GetName(enumType, i);
                object[] attrs = enumType.GetField(enumName).GetCustomAttributes(typeof(EnumTipAttribute), true);
                if (attrs != null && attrs.Length == 1)
                {
                    EnumTipAttribute desc = attrs[0] as EnumTipAttribute;
                    if (desc != null) enumName = desc.Tip;
                }
                dit.Add(i, enumName);
            }
            EnumDictionaryCache.Add(enumType, dit);
            return dit;
        }

        /// <summary>
        /// 获取位域的值列表
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int[] GetFlagsInt32List(this Enum e)
        {
            var et = e.GetType();
            var arr = e.ToString().Split(',');
            var tmp = new Int32[arr.Length];
            for (int i = 0, j = arr.Length; i < j; i++)
            {
                tmp[i] = (Int32)Enum.Parse(et, arr[i]);
            }
            return tmp;
        }

        /// <summary>
        /// 数字值缓存
        /// </summary>
        private static Dictionary<Enum, Int32> EnumInt32Cache = new Dictionary<Enum, Int32>();

        /// <summary>
        /// 获取数字值
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Int32 GetInt32(this Enum e)
        {
            if (EnumInt32Cache.ContainsKey(e)) return EnumInt32Cache[e];

            var et = e.GetType();
            var ev = (Int32)Enum.Parse(et, e.ToString());
            EnumInt32Cache.Add(e, ev);
            return ev;
        }

        /// <summary>
        /// 获取数字的字符串值
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetInt32Str(this Enum e)
        {
            return e.GetInt32().ToString();
        }

        /// <summary>
        /// enumTip属性值缓存
        /// </summary>
        private static Dictionary<Enum, string> EnumTipCache = new Dictionary<Enum, string>();

        /// <summary>
        /// 获取枚举成员说明
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetTip(this Enum e)
        {
            if (EnumTipCache.ContainsKey(e)) return EnumTipCache[e];

            string strDisc = _GetTip(e);
            EnumTipCache.Add(e, strDisc);
            return strDisc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string _GetTip(Enum e)
        {
            FieldInfo fieldInfo = e.GetType().GetField(e.ToString());
            object[] attrs = fieldInfo.GetCustomAttributes(typeof(EnumTipAttribute), true);
            if (attrs == null || attrs.Length != 1) return e.ToString();

            EnumTipAttribute desc = attrs[0] as EnumTipAttribute;
            return desc == null ? e.ToString() : desc.Tip;
        }
    }
}
