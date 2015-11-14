using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using iTextSharp.text;

namespace Nop.Admin.Models.FareTemplates
{
    public class FareTemplateModel : BaseNopEntityModel
    {
        public FareTemplateModel()
        {
            ValuationList = new List<SelectListItem>();
            InclPostageProvisos = new List<InclPostageProvisoModel>();
            CarryModes = new List<CarryModeModel>();
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        [NopResourceDisplayName("模板名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        [NopResourceDisplayName("发货地址")]
        public virtual string ShopAddr { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        [NopResourceDisplayName("发货时间")]
        public virtual string DispatchTime { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>
        [NopResourceDisplayName("是否包邮")]
        public virtual bool? IsInclPostage { get; set; }
        /// <summary>
        /// 计价方式 1:按件 2:按重量 3:按体积
        /// </summary>
        [NopResourceDisplayName("计价方式")]
        public virtual int ValuationModel { get; set; }
        /// <summary>
        /// 指定条件包邮
        /// </summary>
        [NopResourceDisplayName("指定条件包邮")]
        public virtual bool? IsInclPostageByif { get; set; }

        public ICollection<InclPostageProvisoModel> InclPostageProvisos { get; set; }

        public ICollection<CarryModeModel> CarryModes { get; set; }

        public IList<SelectListItem> ValuationList { get; set; }

    }
}