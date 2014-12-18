using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using JzSayGen;
using System.Configuration;
using System.Reflection;
using System.Web.Routing;
using System.Web.Script.Serialization;
using log4net;

namespace JzSayDemo
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        public static ILog SysLog { get; set; }

        /// <summary>
        /// 序列化对象
        /// </summary>
        public static JavaScriptSerializer JsSerialize = new JavaScriptSerializer();


        protected static void RegisterRoutes(RouteCollection routes)
        {            
            routes.Ignore("{resource}.axd/{*pathInfo}");

            // UrlParameter.Optional 可选参数必须要引用 System.Web.Mvc 故这里多写一次匹配规则

            RouteTable.Routes.Add("urlRoute0", new Route("HH.aspx", new JzSayRouteHandlerFactory("url")));
            RouteTable.Routes.Add("JSRoute0", new Route("JS.aspx", new JzSayRouteHandlerFactory("JS")));

            RouteValueDictionary routeValueDefault1 = new RouteValueDictionary { { "action", "Index" }};
            RouteValueDictionary routeValueMatch1 = new RouteValueDictionary() { { "action", @"^[a-zA-Z]+$" } };
            RouteTable.Routes.Add("urlRoute1", new Route("HH/{action}.aspx", routeValueDefault1, routeValueMatch1, new JzSayRouteHandlerFactory("url")));
            RouteTable.Routes.Add("JSRoute1", new Route("JS-{action}.aspx", routeValueDefault1, routeValueMatch1, new JzSayRouteHandlerFactory("JS")));

            RouteValueDictionary routeValueDefault2 = new RouteValueDictionary { { "action", "Index" }, { "day", "1" } };
            RouteValueDictionary routeValueMatch2 = new RouteValueDictionary() { { "action", @"^[a-zA-Z]+$" }, { "day", @"^[0-9]+$" } };
            RouteTable.Routes.Add("urlRoute2", new Route("HH/{action}/{day}.aspx", routeValueDefault2, routeValueMatch2, new JzSayRouteHandlerFactory("url")));
            RouteTable.Routes.Add("JSRoute2", new Route("JS-{action}-{day}.aspx", routeValueDefault2, routeValueMatch2, new JzSayRouteHandlerFactory("JS")));
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Global.SysLog = LogManager.GetLogger("SysLogger");
            log4net.Config.XmlConfigurator.Configure();

            AjaxBinHttpHandlerFactory.MatrixNamespace = "JzSayDemo.ClsDll";
            AjaxBinHttpHandlerFactory.MatrixAssembly = Assembly.GetExecutingAssembly();
            SqlHelper.DB_CONN_STRING = ConfigurationManager.AppSettings.Get("DB_CONN_STRING") ?? "";

            RegisterRoutes(RouteTable.Routes);
        }       
    }

    /// <summary>
    /// 
    /// </summary>
    public class JzSayRouteHandlerFactory : IRouteHandler
    {
        protected string RouteName { get; set; }

        public JzSayRouteHandlerFactory(string routeName)
        {
            this.RouteName = routeName;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (this.RouteName == "url") return new JzSayHandlerPage(requestContext);
            return new JSHandlerPage(requestContext);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AjaxBin(RequestMethod = "GET", ResponseFormat = "Json", Credential = AjaxBinCredential.No)]
    public class JSHandlerPage : AjaxBinHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 路由对象
        /// </summary>
        protected RequestContext RouteContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rContext"></param>
        public JSHandlerPage(RequestContext rContext)
        {
            this.RouteContext = rContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        protected string GetRouteData(string routeKey)
        {
            return this.RouteContext.RouteData.Values.Any(cc => cc.Key == routeKey) ? this.RouteContext.RouteData.Values[routeKey].ToString() : "";
        }

        /// <summary>
        /// 
        /// </summary>
        public void Index()
        {
            this.EchoString("arg=> ");
        }

        /// <summary>
        /// 
        /// </summary>
        [AjaxBin(RequestMethod = "GET")]
        public void Test(string a)
        {
            string day = this.GetRouteData("day");
            this.EchoString("arg=> " + day + " a=" + a);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JzSayHandlerPage : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        public RequestContext RouteContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public JzSayHandlerPage(RequestContext context)
        {
            this.RouteContext = context;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        protected string GetRouteData(string routeKey)
        {
            return this.RouteContext.RouteData.Values.Any(cc => cc.Key == routeKey) ? this.RouteContext.RouteData.Values[routeKey].ToString() : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string action = this.GetRouteData("action");
            string day = this.GetRouteData("day");

            context.Response.ContentType = "text/html";
            context.Response.Write(action);
            context.Response.Write(" &nbsp; ");
            context.Response.Write(day);
            
        }
    }
}