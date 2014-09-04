using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace JzSayGen
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExten
    {
        /// <summary>
        /// 获取位域的值列表
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int[] ToIntVal(this Enum v)
        {
            var et = v.GetType();
            var arr = v.ToString().Split(',');
            var tmp = new int[arr.Length];
            for (int i = 0, j = arr.Length; i < j; i++)
            {
                var ddc = Enum.Parse(et, arr[i]);
                tmp[i] = (int)Enum.Parse(et, arr[i]);
            }
            return tmp;
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
