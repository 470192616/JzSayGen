using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using JzSayGen;
using System.ComponentModel.DataAnnotations;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    [AjaxBin(RequestMethod = "GET", ResponseFormat = "Json", Credential = AjaxBinCredential.Yes)]
    public class ABTest : AjaxBinHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 输出方法重写
        /// </summary>
        /// <param name="responseFormat"></param>
        /// <param name="T"></param>
        /// <param name="data"></param>
        public override void EchoCustom(string responseFormat, Type T, object data)
        {

            if (T == typeof(string))  //if(responseFormat.Equals("vText"))
            {
                string result = data as string;
                base.EchoString("call me" + data.ToString());  //base.EchoString("vText" + data.ToString());
                return;
            }
            base.EchoJson(T, data);
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="currentContext"></param>
        /// <returns></returns>
        public override bool CredentialValidator(HttpContext currentContext)
        {
            this.Code = 0;
            this.Message = "通过认证";

            return true;
        }

        /// <summary>
        /// 默认的处理函数
        /// http://localhost:9385/AjaxBin/ABTest.ashx
        /// </summary>
        /// <returns></returns>
        [AjaxBin(ResponseFormat = "vText")]
        public string Index()
        {
            return DateTime.Now.ToString();
        }


        /// <summary>
        /// 参数测试
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="payMoney"></param>
        /// <param name="cardNumber"></param>
        /// <param name="stat"></param>
        /// <param name="cateType"></param>
        /// <param name="uploadFile"></param>
        /// <param name="dt"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        [AjaxBin(ResponseFormat = ""), CUS1(Des = "cus1"), CUS2(Attr = "attr")]
        public string Arg(
            [Required(ErrorMessage = "必填"), StringLength(20, MinimumLength = 6, ErrorMessage = "{0}长度{2}到{1}位之间")] string userName,
            [Range(10.00, 100.00)] decimal payMoney,
            [RegularExpression("[a-z]+", ErrorMessage = "匹配正则[a-z]+")] string cardNumber,
            [AjaxBinValueIn("1", "2", "3", ErrorMessage = "{0}参数值必须在{1}中")] Int32 stat,
            [AjaxBinValueIn("保密", "未知", "你猜猜猜", ErrorMessage = "{0}参数值必须在{1}中")] string cateType,
            HttpPostedFile uploadFile,
            DateTime dt,
            ArgTestEnum sex)
        {
            if (!this.IsValid)
            {
                foreach (var e in this.ValidErrors)
                {
                    this.Message = e.ParameterName + "|" + e.ErrorMessage;
                    return this.Message;
                }
            }

            var c1 = this.GetCurentMethodInfoAttribute<CUS1Attribute>(); //自定义属性
            var c2 = this.GetCurentMethodInfoAttribute<CUS2Attribute>();
            
            //c1.Des
            //c2.Attr

            return DateTime.Now.ToString();
        }


    }




    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CUS1Attribute : Attribute
    {
        public string Des { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CUS2Attribute : Attribute
    {
        public string Attr { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public enum ArgTestEnum
    {        
        Man = 1,

        Woman = 2
    }
}