using System.Collections.Generic;

namespace Nop.Admin.Models.FareTemplates
{
    public class AreaModel
    {
        public AreaModel()
        {
            Provinces = new List<ProvinceModel>();
        }

        public int Id = 21;
        public string Name = "中国";
        public IList<ProvinceModel> Provinces { get; set; }
    }

    public class ProvinceModel
    {
        public ProvinceModel()
        {
            Citys = new List<CityModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CityModel> Citys { get; set; }
    }

    public class CityModel
    {
        public CityModel()
        {
            Streets = new List<StreetModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<StreetModel> Streets { get; set; }
    }

    public class StreetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}