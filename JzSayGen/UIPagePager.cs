using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JzSayGen
{
    /// <summary>
    /// 基础分页
    /// </summary>
    public class UIPagePager
    {
        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public Int32 DisplayLimit { get; private set; }

        /// <summary>
        /// 两边显示的条数
        /// </summary>
        public Int32 FlankLimit { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public Int64 RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public Int32 PageCount { get; private set; }

        /// <summary>
        /// 基础连接字符串
        /// </summary>
        public string BaseLink { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public string AttachUrlParameter { get; set; }

        /// <summary>
        /// 没有上一页模版
        /// </summary>
        public string NoPrevTemplate { get; set; }

        /// <summary>
        /// 上一页模版
        /// </summary>
        public string PrevTemplate { get; set; }

        /// <summary>
        /// 没有下一页模版
        /// </summary>
        public string NoNextTemplate { get; set; }

        /// <summary>
        /// 下一页模版
        /// </summary>
        public string NextTemplate { get; set; }

        /// <summary>
        /// 普通链接模版
        /// </summary>
        public string LinkTemplate { get; set; }

        /// <summary>
        /// 当前显示模版
        /// </summary>
        public string CurentTemplate { get; set; }

        /// <summary>
        /// 信息统计模版
        /// </summary>
        public string CounterTemplate { get; set; }

        /// <summary>
        /// 模版链接变量
        /// </summary>
        public string TLVar { get; set; }

        /// <summary>
        /// 模版页数变量
        /// </summary>
        public string TPVar { get; set; }

        /// <summary>
        /// 模版统计记录总数变量
        /// </summary>
        public string TRSCVar { get; set; }

        /// <summary>
        /// 模版统计翻页总数变量
        /// </summary>
        public string TPCVar { get; set; }

        /// <summary>
        /// 每个链接项换行符
        /// </summary>
        public string LinkItemBreak { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordCount"></param>
        public UIPagePager(int recordCount)
            : this(recordCount, 10)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="displayLimit"></param>
        public UIPagePager(Int64 recordCount, int displayLimit)
        {
            this.DisplayLimit = displayLimit;
            this.FlankLimit = 8;
            this.RecordCount = recordCount;
            this.PageCount = (int)Math.Ceiling(this.RecordCount / (double)this.DisplayLimit);
            this.LinkItemBreak = " ";
            this.BaseLink = "?Page=";
            this.AttachUrlParameter = "";
            this.NoPrevTemplate = "";
            this.NoNextTemplate = "";
            this.PrevTemplate = "<a href=\"{H}\" title=\"第{P}页\">&#8249;</a>";
            this.NextTemplate = "<a href=\"{H}\" title=\"第{P}页\">&#8250;</a>";
            this.LinkTemplate = "<a href=\"{H}\" title=\"第{P}页\">{P}</a>";
            this.CurentTemplate = "<span title=\"当前第{P}页\">{P}</span>";
            this.CounterTemplate = "";// "共{RC}条数据{PC}页 ";
            this.TLVar = "{H}";
            this.TPVar = "{P}";
            this.TPCVar = "{PC}";
            this.TRSCVar = "{RC}";            
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <param name="curentPage"></param>
        /// <returns></returns>
        public string Show(Int32 curentPage)
        {

            Func<int, string> isCurentPageFn = (dp) =>
            {
                return (
                    dp == curentPage ?
                    this.CurentTemplate.Replace(this.TLVar, dp.ToString()).Replace(this.TPVar, dp.ToString()) :
                    this.LinkTemplate.Replace(this.TLVar, this.BaseLink + dp.ToString() + this.AttachUrlParameter).Replace(this.TPVar, dp.ToString())
                    ) + this.LinkItemBreak;
            };

            var pager = new StringBuilder();

            pager.Append(this.CounterTemplate.Replace(this.TRSCVar, this.RecordCount.ToString()).Replace(this.TPCVar, this.PageCount.ToString()) + this.LinkItemBreak);

            pager.Append((
                curentPage > 1 ?
                this.PrevTemplate.Replace(this.TLVar, this.BaseLink + (curentPage - 1).ToString() + this.AttachUrlParameter).Replace(this.TPVar, (curentPage - 1).ToString()) :
                this.NoPrevTemplate
                ) + this.LinkItemBreak);

            if (this.PageCount < (this.FlankLimit * 2 + 2))
            {
                for (var p = 1; p <= this.PageCount; p++) pager.Append(isCurentPageFn(p));
            }
            else
            {
                if (curentPage < (this.FlankLimit + 2)) //开始
                {
                    for (var p = 1; p < (this.FlankLimit * 2 + 2); p++) pager.Append(isCurentPageFn(p));
                }
                else if (curentPage > this.PageCount - (this.FlankLimit + 1)) //结尾
                {
                    for (var p = this.PageCount - (this.FlankLimit * 2); p <= this.PageCount; p++) pager.Append(isCurentPageFn(p));
                }
                else //中间
                {
                    for (var p = curentPage - this.FlankLimit; p <= curentPage + this.FlankLimit; p++) pager.Append(isCurentPageFn(p));
                }
            }
            pager.Append((
                curentPage < this.PageCount ?
                this.NextTemplate.Replace(this.TLVar, this.BaseLink + (curentPage + 1).ToString() + this.AttachUrlParameter).Replace(this.TPVar, (curentPage + 1).ToString()) :
                this.NoNextTemplate
                ) + this.LinkItemBreak);

            return pager.ToString();
        }
    }
}
