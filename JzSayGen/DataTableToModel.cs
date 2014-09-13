using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace JzSayGen
{
    /// <summary>
    /// 设值代理
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="value">值</param>
    public delegate void DataTableToModelDelegateSetValue<T>(T value);


    /// <summary>
    /// 转换DataTable数据为实体对象T
    /// </summary>
    /// <typeparam name="T">实体数据类型</typeparam>
    public static class DataTableToModel<T> where T : class, new()
    {
        /// <summary>
        /// 创建类型代理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static Delegate CreateSetDelegate(T model, string propertyName, out PropertyInfo propertyInfo)
        {
            propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propertyInfo == null) return null;
            Type delType = typeof(DataTableToModelDelegateSetValue<>).MakeGenericType(propertyInfo.PropertyType);
            return Delegate.CreateDelegate(delType, model, propertyInfo.GetSetMethod());
        }


        /// <summary>
        /// 转换单行数据
        /// </summary>
        /// <param name="tableColumns">DataTableColumns</param>
        /// <param name="tableRow">DataRow</param>
        /// <returns>实体</returns>
        public static T ParseModel(DataColumnCollection tableColumns, DataRow tableRow)
        {
            Delegate setDelegate;
            T model = new T();
            foreach (DataColumn dc in tableColumns)
            {
                var tmc = tableRow[dc.ColumnName];

                PropertyInfo px;
                setDelegate = CreateSetDelegate(model, dc.ColumnName, out px);
                if (setDelegate == null) continue;
                setDelegate.DynamicInvoke(ObjectExten.Convert(tableRow[dc.ColumnName], px.PropertyType));
            }
            return model;
        }

        /// <summary>
        /// 转换列表
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <returns>实体列表</returns>
        public static List<T> ParseList(DataTable table)
        {
            List<T> list = new List<T>();
            if (table == null || table.Rows.Count < 1) return list;
            foreach (DataRow dr in table.Rows)
            {
                var mo = ParseModel(table.Columns, dr);
                list.Add(mo);
            }
            return list;
        }

    }
}
