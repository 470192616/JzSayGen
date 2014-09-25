using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;
using System.Web;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace JzSayGen
{

    /// <summary>
    /// HttpHandler封装
    /// </summary>
    public class AjaxBinHttpHandler : System.Web.IHttpHandler
    {
        /// <summary>
        /// 提示代码
        /// </summary>
        protected Int32 Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        protected string Message { get; set; }

        /// <summary>
        /// 当前Http请求对象
        /// </summary>
        protected HttpContext CurrentContext { get; set; }

        /// <summary>
        /// 当前调用的方法
        /// </summary>
        protected MethodInfo CurentMethodInfo { get; set; }

        /// <summary>
        /// 参数验证错误列表
        /// </summary>
        protected List<AjaxBinValidError> ValidErrors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            this.CurrentContext = context;
            this.ValidErrors = new List<AjaxBinValidError>();

            Type clssType = this.GetType();
            Type attrType = typeof(AjaxBinAttribute);

            AjaxBinAttribute options = new AjaxBinAttribute(); //默认配置
            this.ParseAttributeABC(clssType.GetCustomAttributes(attrType, false), ref options); //类配置重写

            string action = this.GetActionName("Index"); //默认处理方法 Index

            MethodInfo methodInfo = clssType.GetMethod(action);
            if (methodInfo == null)
            {
                throw new NotSupportedException(action + "方法未找到");
            }

            this.CurentMethodInfo = methodInfo; //外显当前调用的方法

            this.ParseAttributeABC(methodInfo.GetCustomAttributes(attrType, false), ref options); //方法配置重写

            if (string.IsNullOrEmpty(options.RequestMethod) == false && this.CurrentContext.Request.HttpMethod.Equals(options.RequestMethod, StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new NotSupportedException("调用方式" + this.CurrentContext.Request.HttpMethod + "不支持");
            }

            if (options.Credential == AjaxBinCredential.Yes)
            {
                if (this.CredentialValidator(this.CurrentContext) == false)
                {
                    //身份认证失败
                    this.RenderView(options.ResponseFormat, typeof(string), string.Empty);
                    return;
                }
            }

            var invokeParameters = this.ParseParameters(this.CurrentContext.Request, options.RequestMethod, methodInfo.GetParameters());

            object result = methodInfo.Invoke(this, invokeParameters); //执行方法

            if (methodInfo.ReturnType.FullName.Equals("System.Void")) return;

            this.RenderView(options.ResponseFormat, methodInfo.ReturnType, result);
        }

        /// <summary>
        /// 参数验证是否通过
        /// </summary>
        public bool IsValid
        {
            get { return this.ValidErrors.Count == 0; }
        }

        /// <summary>
        /// 身份认证 true通过认证 false未通过 失败需要设置 this.Code this.Message 的值
        /// </summary>
        /// <param name="currentContext"></param>        
        public virtual bool CredentialValidator(HttpContext currentContext)
        {
            throw new NotSupportedException("没有重写CredentialValidator身份认证方法");
        }

        /// <summary>
        /// 输出文本内容
        /// </summary>        
        /// <param name="strData">文本字符串</param>
        /// <param name="contentType">输出类型 text/plain  text/html application/json, text/javascript  application/xml, text/xml</param>
        public void EchoString(string strData, string contentType = "text/plain")
        {
            this.CurrentContext.Response.ContentType = contentType;
            this.CurrentContext.Response.Write(strData);
        }

        /// <summary>
        /// 输出json数据
        /// </summary>
        /// <param name="T"></param>
        /// <param name="data"></param>
        public virtual void EchoJson(Type T, object data)
        {
            string d = new JavaScriptSerializer().Serialize(data);
            d = "{" + string.Format("\"Code\":{0}, \"Message\":\"{1}\", \"Data\":{2}", this.Code, this.Message, d) + "}";

            string cbFun = this.ParseParametersValue(this.CurrentContext.Request, "", "callback");

            this.EchoString(string.IsNullOrEmpty(cbFun) ? d : cbFun + "(" + d + ");", "application/json, text/javascript");
        }

        /// <summary>
        /// 输出FormTarget数据
        /// </summary>
        /// <param name="T"></param>
        /// <param name="data"></param>
        public virtual void EchoFormTarget(Type T, object data)
        {
            string d = new JavaScriptSerializer().Serialize(data);
            d = "{" + string.Format("\"Code\":{0}, \"Message\":\"{1}\", \"Data\":{2}", this.Code, this.Message, d) + "}";

            string cbFun = this.ParseParametersValue(this.CurrentContext.Request, "", "callback");
            StringBuilder s = new StringBuilder();
            s.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>");
            s.Append("<script type=\"text/javascript\">");
            s.Append("parent." + (string.IsNullOrEmpty(cbFun) ? "PostCallBack" : cbFun) + "(" + d + ");");
            s.Append("</script>");
            s.Append("</body></html>");

            this.EchoString(s.ToString(), "text/html");
        }

        /// <summary>
        /// 自定义输出        
        /// if (T == typeof(ResultType)) { ResultType result = data as ResultType;  ... }
        /// </summary>
        /// <param name="responseFormat"></param>
        /// <param name="T"></param>
        /// <param name="data"></param>
        public virtual void EchoCustom(string responseFormat, Type T, object data)
        {
            throw new NotSupportedException("没有重新EchoCustom输出方法");
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="responseFormat"></param>
        /// <param name="T"></param>
        /// <param name="data"></param>
        public virtual void RenderView(string responseFormat, Type T, object data)
        {
            if (responseFormat.Equals("Text", StringComparison.OrdinalIgnoreCase))
            {
                this.EchoString(data.ToString());
                return;
            }

            if (responseFormat.Equals("Json", StringComparison.OrdinalIgnoreCase))
            {
                this.EchoJson(T, data);
                return;
            }

            if (responseFormat.Equals("FormTarget", StringComparison.OrdinalIgnoreCase))
            {
                this.EchoFormTarget(T, data);
                return;
            }

            this.EchoCustom(responseFormat, T, data);
        }

        /// <summary>
        /// 获取当前调用方法的定制属性
        /// </summary>
        /// <typeparam name="T">定制属性 CustomAttributes</typeparam>
        /// <returns></returns>
        protected T GetCurentMethodInfoAttribute<T>() where T : Attribute
        {
            if (this.CurentMethodInfo == null) return null;

            var handleAtts = this.CurentMethodInfo.GetCustomAttributes(typeof(T), false);
            if (handleAtts != null && handleAtts.Length == 1)
            {
                return handleAtts[0] as T;
            }
            return null;
        }

        /// <summary>
        /// 解析参数值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestMethod"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string ParseParametersValue(HttpRequest request, string requestMethod, string parameterName)
        {
            if (requestMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) return request.QueryString.Get(parameterName) ?? string.Empty;
            if (requestMethod.Equals("POST", StringComparison.OrdinalIgnoreCase)) return request.Form.Get(parameterName) ?? string.Empty;

            var v = request.Form.Get(parameterName) ?? string.Empty; //post优先
            return string.IsNullOrEmpty(v) == false ? v : (request.QueryString.Get(parameterName) ?? string.Empty);
        }

        /// <summary>
        /// 转换参数值
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        object ParseParametersConvert(Type parameterType, string parameterValue)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(parameterType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFrom(parameterValue);
                }
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(parameterType))
                {
                    return converter.ConvertTo(parameterValue, parameterType);
                }
            }
            catch
            { }

            return parameterType.IsValueType ? Activator.CreateInstance(parameterType) : null;
        }

        /// <summary>
        /// 解析方法的参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestMethod"></param>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        object[] ParseParameters(HttpRequest request, string requestMethod, ParameterInfo[] methodParameters)
        {
            var invokeParameters = new object[methodParameters.Length];

            string pName = "", pValue = "";
            foreach (var parameter in methodParameters)
            {
                if (parameter.ParameterType.FullName.Equals("System.Web.HttpPostedFile")) //文件类型单独处理，且不验证
                {
                    invokeParameters[parameter.Position] = request.Files.Get(parameter.Name);
                    continue;
                }

                pName = parameter.Name; //参数名称
                pValue = this.ParseParametersValue(request, requestMethod, pName); //参赛值
                if (string.IsNullOrEmpty(pValue))
                {
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null);
                    continue;
                }
                if (parameter.ParameterType.FullName.Equals("System.String"))
                {
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, string.IsNullOrEmpty(pValue) ? "" : pValue.Trim());
                    continue;
                }
                if (parameter.ParameterType.FullName.Equals("System.Boolean"))
                {
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, string.IsNullOrEmpty(pValue) ? false : (-1 != ",1,t,true,y,yes,".IndexOf("," + pValue.ToLower() + ",", StringComparison.OrdinalIgnoreCase)));
                    continue;
                }
                if (parameter.ParameterType.FullName.Equals("System.Int32"))
                {
                    Int32 v32;
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, Int32.TryParse(pValue, out v32) ? v32 : 0);
                    continue;
                }
                if (parameter.ParameterType.FullName.Equals("System.Int64"))
                {
                    Int64 v64;
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, Int64.TryParse(pValue, out v64) ? v64 : 0);
                    continue;
                }
                if (parameter.ParameterType.FullName.Equals("System.Decimal"))
                {
                    Decimal vDecimal;
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, Decimal.TryParse(pValue, out vDecimal) ? vDecimal : 0M);
                    continue;
                }
                if (parameter.ParameterType.IsInstanceOfType(pValue))
                {
                    invokeParameters[parameter.Position] = this.ParameterValid(parameter, pValue);
                    continue;
                }
                invokeParameters[parameter.Position] = this.ParameterValid(parameter, this.ParseParametersConvert(parameter.ParameterType, pValue));
            }

            return invokeParameters;
        }

        /// <summary>
        /// 验证参数值 原样返回value
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object ParameterValid(ParameterInfo parameter, object value)
        {
            var vs = parameter.GetCustomAttributes(typeof(ValidationAttribute), false) as ValidationAttribute[];
            if (vs == null || vs.Length == 0) return value;
            foreach (var v in vs)
            {
                if (v.IsValid(value)) continue;
                this.ValidErrors.Add(new AjaxBinValidError(parameter.Name, v.FormatErrorMessage(parameter.Name)));
            }
            return value;
        }

        /// <summary>
        /// 重写属性
        /// </summary>
        /// <param name="curAttributes"></param>
        /// <param name="options"></param>
        void ParseAttributeABC(object[] curAttributes, ref AjaxBinAttribute options)
        {
            if (curAttributes == null || curAttributes.Length != 1) return;

            var opt = (curAttributes[0] as AjaxBinAttribute);
            if (opt.Credential != AjaxBinCredential.Parent) options.Credential = opt.Credential;
            if (string.IsNullOrEmpty(opt.RequestMethod) == false) options.RequestMethod = opt.RequestMethod;
            if (string.IsNullOrEmpty(opt.ResponseFormat) == false) options.ResponseFormat = opt.ResponseFormat;
        }


        /// <summary>
        /// 获取处理方法名称
        /// </summary>
        /// <returns></returns>
        string GetActionName(string defaultActionName)
        {
            string url = this.CurrentContext.Request.Url.AbsolutePath;
            string dat = url.Remove(url.LastIndexOf('.')).Substring(url.LastIndexOf('/') + 1);
            return dat.Contains('-') ? dat.Split('-')[1] : defaultActionName;
        }

        /// <summary>
        /// <![CDATA[
        /// public class classDemo
        /// {        
        ///     [XmlElement(IsNullable=true)]
        ///     public string Name {get;set;}
        ///     
        ///     [XmlIgnore]
        ///     public DateTime DateTimeTypeObj { get; set; }
        /// 
        ///     [XmlElement("DateTimeTypeObj")]
        ///     public string DateTimeTypeObjShow
        ///     {
        ///         //修复时间显示 为正常的格式
        ///         get { return DateTimeTypeObj.ToString("yyyy-MM-dd HH:mm:ss"); }
        ///         set { DateTimeTypeObj = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", null); }
        ///     }
        /// }
        /// ]]>
        /// </summary> 
        /// <param name="T"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ConvertToXml(Type T, object data)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            /*
            settings.OmitXmlDeclaration = false;
            settings.Indent = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.IndentChars = "\t";            
            */
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);

                    XmlSerializer xs = new XmlSerializer(T);
                    xs.Serialize(writer, data, ns);
                }
                string s = Encoding.UTF8.GetString(stream.ToArray());
                /*
                s = RegXmlDate.Replace(s, match =>
                {
                    //修复时间显示方式
                    return ">" + match.Groups[1].Value + " " + match.Groups[2].Value + "</";
                });
                */
                return s;
            }
        }
    }
}
