using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.FareTemplates
{
    public class CarryModeModel : BaseNopEntityModel
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [NopResourceDisplayName("模板ID")]
        public int FareId { get; set; }
        /// <summary>
        /// 运送地区(存id,格式为'省-市-区',以'|'分隔)
        /// </summary>
        [NopResourceDisplayName("运送地区")]
        public string Region { get; set; }
        /// <summary>
        /// 首件
        /// </summary>
        [NopResourceDisplayName("首件")]
        public int? FirstPiece { get; set; }
        /// <summary>
        /// 首重
        /// </summary>
        [NopResourceDisplayName("首重")]
        public decimal? FirstWeight { get; set; }
        /// <summary>
        /// 首体积
        /// </summary>
        [NopResourceDisplayName("首体积")]
        public decimal? FirstBulk { get; set; }
        /// <summary>
        /// 首费
        /// </summary>
        [NopResourceDisplayName("首费")]
        public decimal FirstAmount { get; set; }
        /// <summary>
        /// 续件
        /// </summary>
        [NopResourceDisplayName("续件")]
        public int? SecondPiece { get; set; }
        /// <summary>
        /// 续重
        /// </summary>
        [NopResourceDisplayName("续重")]
        public decimal? SecondWeight { get; set; }
        /// <summary>
        /// 续体积
        /// </summary>
        [NopResourceDisplayName("续体积")]
        public decimal? SecondBulk { get; set; }
        /// <summary>
        /// 续费
        /// </summary>
        [NopResourceDisplayName("续费")]
        public decimal SecondAmount { get; set; }
        /// <summary>
        /// 运送物流（0:赣农速运,1:ems,2:快递）
        /// </summary>
        [NopResourceDisplayName("运送物流")]
        public int CarryWay { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [NopResourceDisplayName("是否默认")]
        public bool? IsDefault { get; set; }
        /// <summary>
        /// 运送地区(存名称,格式为'省-市-区',以'|'分隔)
        /// </summary>
        [NopResourceDisplayName("运送地区")]
        public string RegionList { get; set; }
    }
}