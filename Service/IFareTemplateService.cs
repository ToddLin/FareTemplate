using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Services.FareTemplates
{
    public partial interface IFareTemplateService
    {
        #region FareTemplate

        IList<FareTemplate> GetFareTemplateList();
        /// <summary>
        /// 分页获取运费模板
        /// </summary>
        IPagedList<FareTemplate> GetFareTemplateList(int pageIndex, int pageSize);
        /// <summary>
        /// 获取单个运费模板
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns>FareTemplate</returns>
        FareTemplate GetFareTemplate(int id);
        /// <summary>
        /// 添加新的运费模板
        /// </summary>
        /// <param name="faretemplate">FareTemplate</param>
        void InsertFareTemplate(FareTemplate faretemplate);
        /// <summary>
        /// 删除指定的运费模板
        /// </summary>
        /// <param name="faretemplate">FareTemplate</param>
        void DeleteFareTemplate(FareTemplate faretemplate);
        /// <summary>
        /// 判断指定的运费模板是否被使用
        /// </summary>
        /// <param name="id">模板id</param>
        /// <returns></returns>
        bool FareTemplateIsUsed(int id);
        /// <summary>
        /// 获取多个商品的运费
        /// </summary>
        /// <param name="carrymodes">单商品最优运费方式的集合</param>
        /// <param name="weights">多个商品重量</param>
        /// <param name="bulks">多个商品体积</param>
        /// <param name="pices">多个商品件数</param>
        /// <returns>运费</returns>
        decimal GetAllCarriage(List<CarryMode> carrymodes, decimal? weights, decimal? bulks, int? pices);
        /// <summary>
        /// 计算单商品的运费
        /// </summary>
        /// <param name="carriage">计算出的运费 </param>
        /// <param name="productVariantId">订单单品id</param>
        /// <param name="area">运送地区(具体到区县)</param>
        /// <param name="carryway">物流</param>
        /// <param name="pices">单商品总件数</param>
        /// <returns>最优的运费方式</returns>
        IList<CarryMode> GetOneCarriage(out decimal carriage, int productVariantId, int carryway, int pices, string area);

        /// <summary>
        /// 判断单品是否包邮
        /// </summary>
        /// <param name="fareId">单品绑定的运费模板ID</param>
        /// <param name="amount">单品总额</param>
        /// <param name="pices">单品总件数</param>
        /// <param name="weights">单品总重量</param>
        /// <param name="bulks">单品总体积</param>
        /// <param name="carryway">运送方式</param>
        /// <param name="area">地区</param>
        /// <returns></returns>
        bool IsInclPostByOne(int fareId, decimal? amount, int? pices, decimal? weights, decimal? bulks, int? carryway, string area);

        #endregion

        #region CarryMode

        /// <summary>
        /// 根据运费模板id获取相关联的运费方式
        /// </summary>
        /// <param name="fareid">模板id</param>
        /// <returns></returns>
        IList<CarryMode> GetCarryModeByFareId(int fareid);
        /// <summary>
        /// 插入一条新的运费方式
        /// </summary>
        /// <param name="carrymode"></param>
        void InsertCarryMode(CarryMode carrymode);
        /// <summary>
        /// 删除一条指定的运费方式
        /// </summary>
        /// <param name="carrymode"></param>
        void DeleteCarryMode(CarryMode carrymode);

        #endregion

        #region InclPostageProviso
        /// <summary>
        /// 根据运费模板ID获取相关的包邮条件
        /// </summary>
        /// <param name="fareid">模板ID</param>
        /// <returns></returns>
        IList<InclPostageProviso> GetInclPostageProvisoByFareId(int fareid);
        /// <summary>
        /// 插入一条新的包邮条件
        /// </summary>
        /// <param name="inclpostageproviso"></param>
        void InsertInclPostageProviso(InclPostageProviso inclpostageproviso);
        /// <summary>
        /// 删除一条包邮条件
        /// </summary>
        /// <param name="inclpostageproviso"></param>
        void DeletePostageProviso(InclPostageProviso inclpostageproviso);
        #endregion

    }
}
