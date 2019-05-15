using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
   public interface IRegion_Province_City_CityTierRepository
    {
        Task<List<City>> GetCityListByProvinceID(string ProvinceID);
        Task<List<City>> GetCityListByProvinceName(string ProvinceName);
        Task<List<City>> GetCityListByProvinceIDList(string ProvinceIDList);
        
        Task<List<object>> Getvw_CityListByProvinceID(string ProvinceID);
        Task<List<Province>> GetProvinceListByRegion(string Region);
        Task<List<Province>> GetProvinceAllList();
        
        Task<List<Region>> GetRegionList();
    }
}
