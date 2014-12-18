using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JzSayGen;

namespace JzSayDemo.ClsDll
{
    public class General
    {
    }

    /// <summary>
    /// 公共状态
    /// </summary>
    public enum EGenStat
    {
        /// <summary>
        /// 未知1
        /// </summary>
        [EnumTip("未知")]
        Unknow = 1,

        /// <summary>
        /// 显示2
        /// </summary>
        [EnumTip("显示")]
        Normal = 2,

        /// <summary>
        /// 隐藏4
        /// </summary>
        [EnumTip("隐藏")]
        Hiden = 4,

        /// <summary>
        /// 已删除
        /// </summary>
        [EnumTip("已删除")]
        Delete = 8

    }
}