using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using log4net;

namespace JzSayDemo.ClsDll
{
    public class General
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        public static JavaScriptSerializer JsSerialize = new JavaScriptSerializer();

        static ILog sysLog = null;

        static General()
        {
            sysLog = LogManager.GetLogger("SysLogger");
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 系统日志
        /// </summary>
        public static ILog SysLog
        {
            get { return sysLog; }
        }
    }
}