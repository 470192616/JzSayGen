using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JzSayGen
{
    /// <summary>
    /// 验证错误
    /// </summary>
    [Serializable]
    public class AjaxBinValidError
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="errorMessage"></param>
        public AjaxBinValidError(string parameterName, string errorMessage)
        {
            this.ParameterName = parameterName;
            this.ErrorMessage = errorMessage;
        }
    }
}
