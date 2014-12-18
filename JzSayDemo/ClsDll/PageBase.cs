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
    public class PageBase : UIPageBase
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected MemberPassPort Member { get; set; }

        protected string GoUrl { get; set; }

        public PageBase()
        {
            this.Init += new EventHandler(PageBase_Init);
        }

        void PageBase_Init(object sender, EventArgs e)
        {
            this.Member = MemberPassPort.GetSession();
            if (this.Member == null) Response.Redirect("/");

            this.GoUrl = HttpUtility.UrlEncode(this.Request.Url.PathAndQuery);
        }
    }
}