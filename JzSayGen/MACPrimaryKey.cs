﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;

namespace JzSayGen
{
    /// <summary>
    /// 当前电脑硬指令
    /// </summary>
    public static class MACPrimaryKey
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Int64 ZERO64 = 0;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Int32 ZERO32 = 0;

        /// <summary>
        /// 
        /// </summary>
        public static readonly decimal ZERODecimal = 0.00M;

        /// <summary>
        /// 获取当前日期时间 return DateTime.Now
        /// </summary>
        public static DateTime GetNow { get { return DateTime.Now; } }


        private static Int64 seed1 = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));
        private static readonly Int64 spnk1 = Int64.Parse(string.Format("{0}0000000000000", ConfigurationManager.AppSettings.Get("SERVER_PREFIX_NUM1") ?? "1"));

        private static Int64 seed2 = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));
        private static readonly Int64 spnk2 = Int64.Parse(string.Format("{0}0000000000000", ConfigurationManager.AppSettings.Get("SERVER_PREFIX_NUM2") ?? "2"));

        private static Int64 seed3 = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));
        private static readonly Int64 spnk3 = Int64.Parse(string.Format("{0}0000000000000", ConfigurationManager.AppSettings.Get("SERVER_PREFIX_NUM3") ?? "3"));

        /// <summary>
        /// 获取新标识主键1 AppSettings["SERVER_PREFIX_NUM1"]
        /// </summary>
        public static Int64 NewKey1
        {
            get
            {
                return spnk1+ Interlocked.Increment(ref seed1);
            }
        }

        /// <summary>
        /// 获取新标识主键2 AppSettings["SERVER_PREFIX_NUM2"]
        /// </summary>
        public static Int64 NewKey2
        {
            get
            {
                return spnk2 + Interlocked.Increment(ref seed2);
            }
        }

        /// <summary>
        /// 获取新标识主键3 AppSettings["SERVER_PREFIX_NUM3"]
        /// </summary>
        public static Int64 NewKey3
        {
            get
            {
                return spnk3 + Interlocked.Increment(ref seed3);
            }
        }
    }
}
