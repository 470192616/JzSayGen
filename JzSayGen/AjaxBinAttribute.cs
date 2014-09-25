using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JzSayGen
{

    /// <summary>
    /// 类属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AjaxBinAttribute : Attribute
    {
        /// <summary>
        /// 获取值的方式 GET / POST 其他(默认)表示不限制
        /// </summary>
        public string RequestMethod { get; set; }

        /// <summary>
        /// 输出的格式 Text / Json / FormTarget 其他(默认)需要重写EchoCustom方法
        /// </summary>
        public string ResponseFormat { get; set; }

        /// <summary>
        /// 是否进行身份认证 仅AjaxBinCredential.Yes的时候才认证
        /// <para>默认父级AjaxBinCredential.Parent</para>
        /// </summary>
        public AjaxBinCredential Credential { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AjaxBinAttribute()
        {
            this.RequestMethod = ""; //获取操作方法的参数名称            
            this.ResponseFormat = ""; //输出信息的格式
            this.Credential = AjaxBinCredential.Parent;
        }
    }

    /// <summary>
    /// 是否需要身份认证
    /// </summary>
    public enum AjaxBinCredential
    {
        /// <summary>
        /// 父级设置
        /// </summary>        
        Parent = 1,

        /// <summary>
        /// 需要认证
        /// </summary>
        Yes = 2,

        /// <summary>
        /// 不需要认证
        /// </summary>
        No = 4
    }
}
