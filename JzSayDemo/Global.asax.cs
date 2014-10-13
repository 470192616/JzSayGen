using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using JzSayGen;
using System.Configuration;
using System.Reflection;

namespace JzSayDemo
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AjaxBinHttpHandlerFactory.MatrixNamespace = "JzSayDemo.ClsDll";
            AjaxBinHttpHandlerFactory.MatrixAssembly = Assembly.GetExecutingAssembly();

            SqlHelper.DB_CONN_STRING = ConfigurationManager.AppSettings.Get("DB_CONN_STRING") ?? "";
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}