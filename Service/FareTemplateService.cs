using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Services.FareTemplates
{
    public class FareTemplateService : IFareTemplateService
    {
        #region Files

        private readonly IRepository<FareTemplate> _faretemplaterepository;
        private readonly IRepository<CarryMode> _carrymoderepository;
        private readonly IRepository<InclPostageProviso> _inclpostageproviso;
        private readonly IRepository<Product> _productrepository;
        private readonly IRepository<ProductVariant> _productVariantrepostitory;
        #endregion
        #region Ctor
        public FareTemplateService(IRepository<FareTemplate> faretemplaterepository, IRepository<CarryMode> carrymoderepository,
            IRepository<InclPostageProviso> inclpostageproviso, IRepository<Product> productrepository, IRepository<ProductVariant> productVariantrepostitory)
        {
            _faretemplaterepository = faretemplaterepository;
            _carrymoderepository = carrymoderepository;
            _inclpostageproviso = inclpostageproviso;
            _productrepository = productrepository;
            _productVariantrepostitory = productVariantrepostitory;
        }

        #endregion
        #region FareTemplate

        public IList<FareTemplate> GetFareTemplateList()
        {
            var query = _faretemplaterepository.Table;
            query = query.OrderBy(q => q.Id);
            return query.ToList();
        }

        public IPagedList<FareTemplate> GetFareTemplateList(int pageIndex, int pageSize)
        {
            var query = _faretemplaterepository.Table;
            query = query.OrderBy(q => q.Id);
            return new PagedList<FareTemplate>(query, pageIndex, pageSize);
        }

        public FareTemplate GetFareTemplate(int id)
        {
            return _faretemplaterepository.GetById(id);
        }

        public void InsertFareTemplate(FareTemplate faretemplate)
        {
            if (faretemplate == null)
            {
                throw new ArgumentNullException("faretemplate");
            }
            _faretemplaterepository.Insert(faretemplate);
        }

        public void DeleteFareTemplate(FareTemplate faretemplate)
        {
            if (faretemplate == null)
            {
                throw new ArgumentNullException("faretemplate");
            }
            _faretemplaterepository.Delete(faretemplate);
        }

        public bool FareTemplateIsUsed(int id)
        {
            bool used = false;
            var query = _productrepository.Table.Where(x => x.FareId == id);
            if (query.Any())
            {
                used = true;
            }
            return used;
        }

        public decimal GetAllCarriage(List<CarryMode> carrymodes, decimal? weights, decimal? bulks, int? pices)
        {
            decimal carriage = 0;
            var firstOrDefault = carrymodes.FirstOrDefault();
            if (firstOrDefault != null)
            {
                decimal firstCarriage = decimal.MinValue;
                decimal secandCarriage = decimal.MaxValue; ;
                foreach (var carrymode in carrymodes)
                {
                    if (carrymode.FirstPiece > 0)//计件
                    {
                        firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstPiece.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstPiece.ToString())));
                        secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondPiece.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondPiece.ToString())));
                        var secandpice = (pices - carrymode.FirstPiece) > 0 ? (pices - carrymode.FirstPiece) : 0;
                        carriage = firstCarriage * carrymode.FirstPiece.Value + secandCarriage * secandpice.Value;
                        continue;
                    }
                    if (carrymode.FirstWeight > 0)//计重
                    {
                        firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstWeight.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstWeight.ToString())));
                        secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondWeight.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondWeight.ToString())));
                        var secandweight = (weights - carrymode.FirstWeight) > 0 ? (weights - carrymode.FirstWeight) : 0;
                        carriage = firstCarriage * carrymode.FirstWeight.Value + secandCarriage * secandweight.Value;
                        continue;
                    }
                    if (carrymode.FirstBulk > 0)//按体积
                    {
                        firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstBulk.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstBulk.ToString())));
                        secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondBulk.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondBulk.ToString())));
                        var secandbulk = (bulks - carrymode.FirstBulk) > 0 ? (bulks - carrymode.FirstBulk) : 0;
                        carriage = firstCarriage * carrymode.FirstBulk.Value + secandCarriage * secandbulk.Value;
                    }
                }
            }
            return carriage;
        }

        public IList<CarryMode> GetOneCarriage(out decimal carriage, int productVariantId, int carryway, int pices, string area)
        {
            var carryMode = new List<CarryMode>();
            var productVariant = _productVariantrepostitory.GetById(productVariantId);
            var fareid = productVariant.Product.FareId;
            var fareTemplate = GetFareTemplate(fareid);
            if (fareTemplate == null)
            {
                carriage = -1;
                return null;
            }
            carriage = 0;//运费
            decimal amount = productVariant.Price * pices;//总金额
            decimal weights = productVariant.Weight * pices;//总重量
            decimal bulks = productVariant.Length * productVariant.Height * productVariant.Width * pices;//总体积
            int mode = fareTemplate.ValuationModel;
            var carrymodes = _carrymoderepository.Table.Where(x => x.FareId == fareid).Where(x => x.CarryWay == carryway);
            if (!string.IsNullOrEmpty(area))
            {
                string[] areas = area.Split('-');
                string province = areas[0];
                string city = areas[0] + "-" + areas[1];
                carrymodes = carrymodes.Where(x =>
                                              (x.Region == "0" ||
                                               ("|" + x.Region + "|").Contains("|" + province + "|") ||
                                               ("|" + x.Region + "|").Contains("|" + city + "|") ||
                                               ("|" + x.Region + "|").Contains("|" + area + "|")));
            }
            if (!carrymodes.Any())
            {
                carriage = -1;
                return null;
            }
            var firstOrDefault = carrymodes.FirstOrDefault();
            if (firstOrDefault != null)
            {
                decimal firstCarriage = decimal.MinValue;
                decimal secandCarriage = decimal.MaxValue;
                foreach (var carrymode in carrymodes)
                {
                    switch (mode)
                    {
                        case 1://计件
                            firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstPiece.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstPiece.ToString())));
                            secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondPiece.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondPiece.ToString())));
                            var secandpiece = (pices - carrymode.FirstPiece) > 0 ? (pices - carrymode.FirstPiece) : 0;
                            carriage = firstCarriage * carrymode.FirstPiece.Value + secandCarriage * secandpiece.Value;
                            break;
                        case 2://计重
                            firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstWeight.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstWeight.ToString())));
                            secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondWeight.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondWeight.ToString())));
                            var secandweight = (weights - carrymode.FirstWeight) > 0 ? (weights - carrymode.FirstWeight) : 0;
                            carriage = firstCarriage * carrymode.FirstWeight.Value + secandCarriage * secandweight.Value;
                            break;
                        case 3://按体积
                            firstCarriage = Math.Max(decimal.Divide(carrymode.FirstAmount, decimal.Parse(carrymode.FirstBulk.ToString())), decimal.Divide(firstCarriage, decimal.Parse(carrymode.FirstBulk.ToString())));
                            secandCarriage = Math.Min(decimal.Divide(carrymode.SecondAmount, decimal.Parse(carrymode.SecondBulk.ToString())), decimal.Divide(secandCarriage, decimal.Parse(carrymode.SecondBulk.ToString())));
                            var secandbulk = (bulks - carrymode.FirstBulk) > 0 ? (bulks - carrymode.FirstBulk) : 0;
                            carriage = firstCarriage * carrymode.FirstBulk.Value + secandCarriage * secandbulk.Value;
                            break;
                    }
                    carryMode.Add(carrymode);
                }
            }
            //判断是否包邮
            if (IsInclPostByOne(fareid, amount, pices, weights, bulks, carryway, area))
            {
                carriage = 0;
            }
            return carryMode;
        }

        public bool IsInclPostByOne(int fareId, decimal? amount, int? pices, decimal? weights, decimal? bulks, int? carryway, string area)
        {
            var fareTemplate = GetFareTemplate(fareId);
            if (fareTemplate == null)
                return false;
            int mode = fareTemplate.ValuationModel;
            //包邮
            if (fareTemplate.IsInclPostage == true)
            {
                return true;
            }
            //指定条件包邮
            if (fareTemplate.IsInclPostageByif == true)
            {
                string[] areas = area.Split('-');
                string province = areas[0];
                string city = areas[0] + "-" + areas[1];
                var query = _inclpostageproviso.Table.Where(x => x.FareId == fareId);
                if (carryway.HasValue)
                    query = query.Where(x => x.CarryWay == carryway);
                if (!string.IsNullOrEmpty(area))
                    query = query.Where(x =>
                        (x.Region == "0" || ("|" + x.Region + "|").Contains("|" + province + "|") || ("|" + x.Region + "|").Contains("|" + city + "|") || ("|" + x.Region + "|").Contains("|" + area + "|")));
                else query = query.Where(x => x.Region == "0");//如果地址为空，匹配地址为全国的包邮记录
                int sum = query.Where(x => !x.IsAll).Count(x => x.Amount <= amount);
                switch (mode)
                {
                    case 1://计件
                        sum = sum + query.Where(x => x.IsAll).Where(x => x.PieceNo <= pices).Count(x => x.Amount <= amount);
                        sum = sum + query.Where(x => !x.IsAll).Count(x => x.PieceNo <= pices);
                        break;
                    case 2://按重量
                        sum = sum + query.Where(x => x.IsAll).Where(x => x.WeightNo <= weights).Count(x => x.Amount <= amount);
                        sum = sum + query.Where(x => !x.IsAll).Count(x => x.WeightNo <= weights);
                        break;
                    case 3://按体积
                        sum = sum + query.Where(x => x.IsAll).Where(x => x.BulkNo <= bulks).Count(x => x.Amount <= amount);
                        sum = sum + query.Where(x => !x.IsAll).Count(x => x.BulkNo <= bulks);
                        break;
                }
                if (sum > 0)
                    return true;
            }
            return false;
        }

        #endregion

        #region CarryMode
        public IList<CarryMode> GetCarryModeByFareId(int fareid)
        {
            if (fareid == 0)
            {
                return null;
            }
            var query = _carrymoderepository.Table;
            query = query.Where(x => x.FareId == fareid);
            return query.ToList();
        }

        public void InsertCarryMode(CarryMode carrymode)
        {
            if (carrymode == null)
            {
                throw new ArgumentNullException("carrymode");
            }
            _carrymoderepository.Insert(carrymode);
        }

        public void DeleteCarryMode(CarryMode carrymode)
        {
            if (carrymode == null)
            {
                throw new ArgumentNullException("carrymode");
            }
            _carrymoderepository.Delete(carrymode);
        }

        #endregion

        #region InclPostageProviso

        public IList<InclPostageProviso> GetInclPostageProvisoByFareId(int fareid)
        {
            if (fareid == 0)
            {
                return null;
            }
            var query = _inclpostageproviso.Table;
            query = query.Where(x => x.FareId == fareid);
            return query.ToList();
        }

        public void InsertInclPostageProviso(InclPostageProviso inclpostageproviso)
        {
            if (inclpostageproviso == null)
            {
                throw new ArgumentNullException("inclpostageproviso");
            }
            _inclpostageproviso.Insert(inclpostageproviso);
        }

        public void DeletePostageProviso(InclPostageProviso inclpostageproviso)
        {
            if (inclpostageproviso == null)
            {
                throw new ArgumentNullException("inclpostageproviso");
            }
            _inclpostageproviso.Delete(inclpostageproviso);
        }
        #endregion
    }
}
