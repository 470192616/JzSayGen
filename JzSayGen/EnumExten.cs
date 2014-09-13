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
        /// 获取位域的值列表
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int[] ToIntVal(this Enum e)
        {
            var et = e.GetType();
            var arr = e.ToString().Split(',');
            var tmp = new int[arr.Length];
            for (int i = 0, j = arr.Length; i < j; i++)
            {
                var ddc = Enum.Parse(et, arr[i]);
                tmp[i] = (int)Enum.Parse(et, arr[i]);
            }
            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<Enum, string> dictDiscs = new Dictionary<Enum, string>();

        /// <summary>
        /// 获取枚举成员说明
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetTip(this Enum e)
        {
            if (dictDiscs.ContainsKey(e)) return dictDiscs[e];

            string strDisc = _GetTip(e);
            dictDiscs.Add(e, strDisc);
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
