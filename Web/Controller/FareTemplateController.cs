using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Nop.Admin.Models.Directory;
using Nop.Admin.Models.FareTemplates;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.FareTemplates;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public partial class FareTemplateController : BaseNopController
    {
        //
        // GET: /FareTemplate/

        #region Files
        private readonly IPermissionService _permissionService;
        private readonly IFareTemplateService _faretemplateservice;
        private readonly IStateProvinceService _stateprovinceservice;
        private readonly ICityService _cityservice;
        private readonly IStreetService _streetservice;
        #endregion

        #region Constructors
        public FareTemplateController(IPermissionService permissionService, IFareTemplateService faretemplateservice, IStateProvinceService stateprovinceservice, ICityService cityservice, IStreetService streetservice)
        {
            _permissionService = permissionService;
            _faretemplateservice = faretemplateservice;
            _stateprovinceservice = stateprovinceservice;
            _cityservice = cityservice;
            _streetservice = streetservice;
        }

        #endregion
        #region Views
        public ActionResult Index(PageModel command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFareTemplate))
                return AccessDeniedView();
            var model = new FareTemplateListModel();
            if (command.PageSize <= 0) command.PageSize = 5;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            var fareTemplates = _faretemplateservice.GetFareTemplateList();
            var fs = new List<FareTemplateModel>();
            if (fareTemplates.Count > 0)
            {
                foreach (var f in fareTemplates)
                {
                    var m = new FareTemplateModel();
                    m = f.ToModel();
                    foreach (var c in m.CarryModes)
                    {
                        c.RegionList = GetArea(c.Region);
                    }
                    fs.Add(m);
                }
                model.FareTemplates = new PagedList<FareTemplateModel>(fs, command.PageNumber - 1, command.PageSize);
                model.PageFilterModel.LoadPagedList(model.FareTemplates);
            }
            return View(model);
        }
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFareTemplate))
                return AccessDeniedView();
            var area = new AreaModel();
            var provinces = _stateprovinceservice.GetStateProvincesByCountryId(area.Id);
            if (provinces.Count > 0)
            {
                foreach (var pro in provinces)
                {
                    var p = new ProvinceModel();
                    p.Id = pro.Id;
                    p.Name = pro.Name;
                    foreach (var city in pro.Cities)
                    {
                        var c = new Models.FareTemplates.CityModel();
                        c.Id = city.Id;
                        c.Name = city.Name;
                        foreach (var street in city.Streets)
                        {
                            var s = new Models.FareTemplates.StreetModel();
                            s.Id = street.Id;
                            s.Name = street.Name;
                            c.Streets.Add(s);
                        }
                        p.Citys.Add(c);
                    }
                    area.Provinces.Add(p);
                }
            }
            return View(area);
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            bool isok = true;
            string[] allkeys = form.AllKeys;
            int inclpostSum = 0;
            int carrySum = 0;
            foreach (var key in allkeys)
            {
                if (key.StartsWith("InclPostageProvisos"))
                {
                    inclpostSum++;
                }
                if (key.StartsWith("CarryModes"))
                {
                    carrySum++;
                }
            }
            inclpostSum = inclpostSum / 5;//条件包邮记录数量
            carrySum = carrySum / 7;//运送方式数量
            //添加运费模板
            FareTemplateModel faretemplate = new FareTemplateModel();
            faretemplate.DispatchTime = form["DispatchTime"];
            bool ispost = form["IsInclPostage"] == "1";//是否包邮
            faretemplate.IsInclPostage = ispost;
            bool ispostif = form["IsInclPostageByif"] == "1";//是否指定条件包邮
            faretemplate.IsInclPostageByif = ispostif;
            faretemplate.Name = form["Name"];
            faretemplate.ShopAddr = form["ShopAddr"];
            int mode = DataParse.ToInt(form["ValuationModel"], 1);//计价方式
            faretemplate.ValuationModel = mode;
            //var fareId = faretemplate.Id;
            #region 添加包邮条件
            if (ispostif)
            {
                for (int i = 0; i < inclpostSum; i++)
                {
                    InclPostageProvisoModel inclpostage = new InclPostageProvisoModel();
                    inclpostage.CarryWay = DataParse.ToInt(form["InclPostageProvisos[" + i + "][CarryWay]"], 0);
                    //inclpostage.FareId = fareId;
                    inclpostage.Region = form["InclPostageProvisos[" + i + "][Region]"];
                    int seleif = DataParse.ToInt(form["InclPostageProvisos[" + i + "][SelectIf]"], 0);
                    if (seleif == 0 || seleif == 2)
                    {
                        switch (mode)
                        {
                            case 1:
                                inclpostage.PieceNo = DataParse.ToInt(form["InclPostageProvisos[" + i + "][First]"], 0);
                                break;
                            case 2:
                                inclpostage.WeightNo = DataParse.ToDecimal(form["InclPostageProvisos[" + i + "][First]"], 0);
                                break;
                            case 3:
                                inclpostage.BulkNo = DataParse.ToDecimal(form["InclPostageProvisos[" + i + "][First]"], 0);
                                break;
                        }
                    }
                    if (seleif == 1)
                    {
                        inclpostage.Amount = DataParse.ToDecimal(form["InclPostageProvisos[" + i + "][First]"], 0);
                    }
                    if (seleif == 2)
                    {
                        inclpostage.Amount = DataParse.ToDecimal(form["InclPostageProvisos[" + i + "][Secand]"], 0);
                        inclpostage.IsAll = true;
                    }
                    faretemplate.InclPostageProvisos.Add(inclpostage);
                }
            }
            #endregion
            #region 添加运送方式
            if (!ispost)
            {
                for (int i = 0; i < carrySum; i++)
                {
                    CarryModeModel carry = new CarryModeModel();
                    carry.CarryWay = DataParse.ToInt(form["CarryModes[" + i + "][CarryWay]"], 0);
                    //carry.FareId = fareId;
                    carry.FirstAmount = DataParse.ToDecimal(form["CarryModes[" + i + "][FirstAmount]"], 0);
                    carry.IsDefault = form["CarryModes[" + i + "][IsDefault]"] == "1";
                    carry.Region = form["CarryModes[" + i + "][Region]"];
                    carry.SecondAmount = DataParse.ToDecimal(form["CarryModes[" + i + "][SecondAmount]"], 0);
                    switch (mode)
                    {
                        case 1:
                            carry.FirstPiece = DataParse.ToInt(form["CarryModes[" + i + "][FirstSum]"], 0);
                            carry.SecondPiece = DataParse.ToInt(form["CarryModes[" + i + "][SecondSum]"], 0);
                            break;
                        case 2:
                            carry.FirstWeight = DataParse.ToDecimal(form["CarryModes[" + i + "][FirstSum]"], 0);
                            carry.SecondWeight = DataParse.ToDecimal(form["CarryModes[" + i + "][SecondSum]"], 0);
                            break;
                        case 3:
                            carry.FirstBulk = DataParse.ToDecimal(form["CarryModes[" + i + "][FirstSum]"], 0);
                            carry.SecondBulk = DataParse.ToDecimal(form["CarryModes[" + i + "][SecondSum]"], 0);
                            break;
                    }
                    faretemplate.CarryModes.Add(carry);
                }
            }
            try
            {
                _faretemplateservice.InsertFareTemplate(faretemplate.ToEntity());
            }
            catch (Exception)
            {
                isok = false;
            }
            #endregion
            string msg = "添加失败！";
            if (isok)
            {
                msg = "添加成功！";
            }
            return Json(new { msg = msg, isok = isok });
        }

        #endregion

        #region Method
        /// <summary>
        /// 删除运费模板
        /// </summary>
        /// <param name="id"></param>
        public ActionResult Delete(int id)
        {
            bool del = false;
            //if 有商品使用当前模板
            //then 不能delete
            if (!_faretemplateservice.FareTemplateIsUsed(id))
            {
                var carrymodes = _faretemplateservice.GetCarryModeByFareId(id);
                if (carrymodes.Count > 0)
                {
                    foreach (var carryMode in carrymodes)
                    {
                        _faretemplateservice.DeleteCarryMode(carryMode);
                    }
                }
                var inclpostageprovisos = _faretemplateservice.GetInclPostageProvisoByFareId(id);
                if (inclpostageprovisos.Count > 0)
                {
                    foreach (var postageProviso in inclpostageprovisos)
                    {
                        _faretemplateservice.DeletePostageProviso(postageProviso);
                    }
                }
                var faretemplate = _faretemplateservice.GetFareTemplate(id);
                if (faretemplate != null)
                {
                    _faretemplateservice.DeleteFareTemplate(faretemplate);
                }
                del = true;
            }
            return Json(new { isdel = del }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取省市区名称
        /// </summary>
        /// <param name="str">省市区IDs</param>
        /// <returns>省市区名称</returns>
        public string GetArea(string str)
        {
            string strArea = null;
            if (string.IsNullOrEmpty(str) || str == "0")
            {
                strArea = "全国";
            }
            else
            {
                string areas = null;
                string[] regions = str.Split('|');//多个地区由'|'隔开
                for (int i = 0; i < regions.Length; i++)
                {
                    string area = null;
                    string[] region = regions[i].Split('-');//省市区用'-'隔开
                    switch (region.Length)
                    {
                        case 1:
                            area = _stateprovinceservice.GetStateProvinceById(int.Parse(region[0])).Name;//只有省取省
                            break;
                        case 2:
                            area = _cityservice.GetCityById(int.Parse(region[1])).Name;//省市取市
                            break;
                        case 3:
                            area = _streetservice.GetStreetById(int.Parse(region[2])).Name;//省市区取区
                            break;
                    }
                    areas = areas + "," + area;
                }
                strArea = areas.Substring(1);
            }
            return strArea;
        }

        #endregion

    }
}
