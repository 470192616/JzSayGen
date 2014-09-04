using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JzSayGen
{    
    /// <summary>
    /// 限定取值范围 比如性别、状态
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AjaxBinValueInAttribute : ValidationAttribute
    {
        string[] AllowValues { get; set; }
        public AjaxBinValueInAttribute(params string[] allowValue)
        {
            if (allowValue == null || allowValue.Length < 2) throw new ArgumentNullException("allow", "必须设置值列表");
            this.AllowValues = allowValue;
        }

        public override bool IsValid(object value)
        {
            if (null == value) return true;
            return this.AllowValues.Any(x => x.Equals(value.ToString()));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(base.ErrorMessageString, name, string.Join(",", this.AllowValues));
        }
    }

}
