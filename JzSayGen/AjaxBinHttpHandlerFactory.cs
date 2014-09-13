using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace JzSayGen
{
    /// <summary>
    /// 动态ashx AjaxBin处理
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///         <add verb="*" path="/AjaxBin/*.ashx" type="JzSayGen.AjaxBinHttpHandlerFactory"/>
    /// ]]>
    /// </example>
    public class AjaxBinHttpHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>
        /// 宿主命名空间 推荐在Application_Start中设置 AjaxBinHttpHandlerFactory.MatrixNamespace = "JzSayGen.ClsDll";
        /// </summary>
        public static string MatrixNamespace { get; set; }

        /// <summary>
        /// 宿主的程序集设置 推荐在Application_Start中设置 AjaxBinHttpHandlerFactory.MatrixAssembly = Assembly.GetExecutingAssembly();
        /// </summary>
        public static Assembly MatrixAssembly { get; set; }

        /// <summary>
        /// /AjaxBin/Class1-arg1-arg2.ashx
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="pathTranslated"></param>
        /// <returns></returns>
        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (AjaxBinHttpHandlerFactory.MatrixNamespace.IsNullOrEmpty())
                throw new NotSupportedException("未设置MatrixNamespace的值");

            //获取到 /AjaxBin/*.ashx 路径的 * 的内容  Class1-arg1-arg2
            string method = url.Remove(url.LastIndexOf('.')).Substring(url.LastIndexOf('/') + 1);
            string typeName = method.Split('-')[0];
            string className = AjaxBinHttpHandlerFactory.MatrixNamespace + "." + typeName;

            IHttpHandler instance = AjaxBinHttpHandlerFactory.MatrixAssembly.CreateInstance(className) as IHttpHandler;
            if (instance == null)
            {
                throw new NotSupportedException("处理类" + className + "未找到");
            }
            return instance;
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
