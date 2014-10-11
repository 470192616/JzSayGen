using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace JzSayGen
{
    /// <summary>
    /// ascx解析
    /// </summary>
    public class UIAscx
    {
        /// <summary>
        /// 解析ascx控件
        /// </summary>
        /// <param name="ascxPath">~/ArtList.ascx</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string RenderView(string ascxPath, HttpContext context = null)
        {
            return RenderView<System.Web.UI.Control>(ascxPath, null);            
        }

        /// <summary>
        /// 解析ascx控件
        /// </summary>
        /// <param name="ascxPath">~/ArtList.ascx</param>
        /// <param name="controlBindFn"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string RenderView<T>(string ascxPath, Action<T> controlBindFn, HttpContext context = null) where T : System.Web.UI.Control
        {
            if (context == null) context = HttpContext.Current;
            using (System.Web.UI.Page p = new System.Web.UI.Page())
            {
                using (StringWriter output = new StringWriter())
                {
                    T ctrl = p.LoadControl(ascxPath) as T;
                    if (controlBindFn != null) controlBindFn(ctrl);
                    p.Controls.Add(ctrl);
                    context.Server.Execute(p, output, true);
                    return output.ToString();
                }
            }
        }
    }
}
