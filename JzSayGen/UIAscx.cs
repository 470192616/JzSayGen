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
        /// <returns></returns>
        public static string RenderView(string ascxPath)
        {
            using (System.Web.UI.Page p = new System.Web.UI.Page())
            {
                using (StringWriter output = new StringWriter())
                {
                    p.Controls.Add(p.LoadControl(ascxPath));
                    HttpContext.Current.Server.Execute(p, output, true);
                    return output.ToString();
                }
            }
        }

        /// <summary>
        /// 解析ascx控件
        /// </summary>
        /// <param name="ascxPath">~/ArtList.ascx</param>
        /// <param name="controlBindFn"></param>
        /// <returns></returns>
        public static string RenderView(string ascxPath, Action<System.Web.UI.Control> controlBindFn)
        {
            using (System.Web.UI.Page p = new System.Web.UI.Page())
            {
                using (StringWriter output = new StringWriter())
                {
                    System.Web.UI.Control ctrl = p.LoadControl(ascxPath);
                    controlBindFn(ctrl);
                    p.Controls.Add(ctrl);
                    HttpContext.Current.Server.Execute(p, output, true);
                    return output.ToString();
                }
            }
        }
    }
}
