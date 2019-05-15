using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLMSide.Data.IRepository;

using PLMSide.Data.Entites;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Region_Province_City_CityTierController : ControllerBase
    {
        private readonly IRegion_Province_City_CityTierRepository Repository;

        public Region_Province_City_CityTierController(IRegion_Province_City_CityTierRepository _Repository)
        {
            Repository = _Repository;
        }
        /// <summary>
        /// 获取所有的区域
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetRegionList")]
        public async Task<object> GetRegionList()
        {
            List<Region> list = await Repository.GetRegionList();
            
            return Ok(list);

        }

        /// <summary>
        /// 获取 省份列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetProvinceAllList")]
        public async Task<object> GetProvinceAllList()
        {
            List<Province> list = await Repository.GetProvinceAllList();

            return Ok(list);

        }
        /// <summary>
        /// 获取 省份列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetProvinceListByRegion/{Region}")]
        public async Task<object> GetProvinceListByRegion(string Region)
        {
            List<Province> list = await Repository.GetProvinceListByRegion(Region);

            return Ok(list);

        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetCityListByProvinceID/{ProvinceID}")]
        public async Task<object> GetCityListByProvinceID(string ProvinceID)
        {
            List<City> list = await Repository.GetCityListByProvinceID(ProvinceID);
            return Ok(list);
        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetCityListByProvinceName/{ProvinceName}")]
        public async Task<object> GetCityListByProvinceName(string ProvinceName)
        {
            List<City> list = await Repository.GetCityListByProvinceName(ProvinceName);
            return Ok(list);
        }


        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetCityListByProvinceIDList/{ProvinceIDList}")]
        public async Task<object> GetCityListByProvinceIDList(string ProvinceIDList)
        {
            List<City> list = await Repository.GetCityListByProvinceIDList(ProvinceIDList);
            return Ok(list);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("Getvw_CityListByProvinceID/{ProvinceID}")]
        public async Task<object> Getvw_CityListByProvinceID(string ProvinceID)
        {
            List<object> list = await Repository.Getvw_CityListByProvinceID(ProvinceID);
            return Ok(list);
        }
    }

}