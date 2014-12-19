using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JzSayGen;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 用户已登录基础页
    /// </summary>
    public class ACPageBase : UIPageBase
    {
        /// <summary>
        /// 缓存自身类型
        /// </summary>
        static Type CurType = typeof(ACPageBase);

        /// <summary>
        /// 当前类型对象
        /// </summary>
        private Type SubType { get; set; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected MemberPassPort Member { get; set; }

        /// <summary>
        /// Url.PathAndQuery
        /// </summary>
        protected string GoUrl { get; set; }


        /// <summary>
        /// 找到当前类型对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Type GetBehindType(Type t)
        {
            Type baseType = t.BaseType;
            if (baseType == CurType) return t;
            return GetBehindType(baseType);
        }

        /// <summary>
        /// 获取类的自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetClassAttribute<T>() where T : Attribute
        {
            object[] curAttributes = this.SubType.GetCustomAttributes(typeof(T), false);
            if (curAttributes == null || curAttributes.Length != 1) return null;
            return curAttributes[0] as T;
        }

        public ACPageBase()
        {
            this.SubType = this.GetBehindType(this.GetType());
            this.Init += new EventHandler(PageBase_Init);
        }

        void PageBase_Init(object sender, EventArgs e)
        {
            this.Member = MemberPassPort.GetSession();
            if (this.Member == null) Response.Redirect("/");

            string pageClassName = this.SubType.FullName.Substring(this.SubType.FullName.IndexOf('.') + 1);

            //var tt = this.GetClassAttribute<JSVAttribute>();

            this.GoUrl = HttpUtility.UrlEncode(this.Request.Url.PathAndQuery);
        }
    }

    /*
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class JSVAttribute : Attribute
    {
        public JSVAttribute(string attr)
        {
            this.Attr = attr;
        }
        public string Attr { get; set; }
    }
    */

}