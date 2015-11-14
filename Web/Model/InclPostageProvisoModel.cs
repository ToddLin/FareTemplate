using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.FareTemplates
{
    public class InclPostageProvisoModel : BaseNopEntityModel
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
        /// 免邮件数
        /// </summary>
        [NopResourceDisplayName("免邮件数")]
        public int? PieceNo { get; set; }
        /// <summary>
        /// 免邮重量
        /// </summary>
        [NopResourceDisplayName("免邮重量")]
        public decimal? WeightNo { get; set; }
        /// <summary>
        /// 免邮体积
        /// </summary>
        [NopResourceDisplayName("免邮体积")]
        public decimal? BulkNo { get; set; }
        /// <summary>
        /// 免邮金额
        /// </summary>
        [NopResourceDisplayName("免邮金额")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 运送地区(存名称,格式为'省-市-区',以'|'分隔)
        /// </summary>
        [NopResourceDisplayName("运送地区")]
        public string RegionList { get; set; }
        /// <summary>
        /// 运送物流（0:赣农速运,1:ems,2:顺丰，3:天天）
        /// </summary>
        [NopResourceDisplayName("运送物流")]
        public int CarryWay { get; set; }
        /// <summary>
        /// 是否全部条件满足
        /// </summary>
        public bool IsAll { get; set; }

    }
}