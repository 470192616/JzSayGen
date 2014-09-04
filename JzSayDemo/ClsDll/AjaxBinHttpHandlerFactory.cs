using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    public class AjaxBinHttpHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>
        /// /AjaxBin/Class1-arg1-arg2.ashx
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="pathTranslated"></param>
        /// <returns></returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //获取到 /AjaxBin/*.ashx 路径的 * 的内容  Class1-arg1-arg2
            string method = url.Remove(url.LastIndexOf('.')).Substring(url.LastIndexOf('/') + 1);
            string typeName = method.Split('-')[0];
            string className = this.GetType().Namespace + "." + typeName;

            IHttpHandler instance = Assembly.GetExecutingAssembly().CreateInstance(className) as IHttpHandler;
            if (instance == null)
            {
                throw new NotSupportedException("处理类" + typeName + "未找到");
            }
            return instance;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
