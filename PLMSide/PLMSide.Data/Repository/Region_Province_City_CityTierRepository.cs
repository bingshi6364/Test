using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace PLMSide.Data.Repository
{
    public class Region_Province_City_CityTierRepository : IRegion_Province_City_CityTierRepository
    {


        public async Task<List<City>> GetCityListByProvinceID(string ProvinceID)
        {
            string sql = "SELECT T.* FROM SIDE_PLM_City T";
            sql += " where Province=@Province ";
            sql += " order by nameEN";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Province", ProvinceID, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<City>(sql, parameters).ToList());
            }
        }

        public async Task<List<City>> GetCityListByProvinceName(string ProvinceName)
        {
            string sql = "SELECT a.nameCN FROM SIDE_PLM_City a left join SIDE_PLM_Province b on a.province=b.id";
            sql += " where b.nameCN=@Province ";
            sql += " order by a.nameEN";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Province", ProvinceName, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<City>(sql, parameters).ToList());
            }
        }

        public async Task<List<City>> GetCityListByProvinceIDList(string ProvinceIDList)
        {
            string sql = "SELECT T.* FROM SIDE_PLM_City T";
            sql += " where Province in (select value from dbo.Side_Split(@Province,',')) ";
            sql += " order by nameEN";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Province", ProvinceIDList, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<City>(sql, parameters).ToList());
            }
        }

        public async Task<List<object>> Getvw_CityListByProvinceID(string ProvinceID)
        {
            string sql = "SELECT T.* FROM [SIDE_vw_City] T";
            sql += " where Province=@Province ";
            sql += " order by nameEN";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Province", ProvinceID, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<object>(sql, parameters).ToList());
            }
        }

        public async Task<List<Province>> GetProvinceListByRegion(string Region)
        {

            string sql = "SELECT T.* FROM SIDE_PLM_Province T";

            sql += " where id in (select value from dbo.Side_Split((select province from[SIDE_PLM_RegionToProvince] where region = @Region),',')) ";

            sql += " order by nameEN";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Region", Region, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Province>(sql, parameters).ToList());
            }

        }

        public async Task<List<Province>> GetProvinceAllList()
        {
            string sql = "SELECT T.* FROM SIDE_PLM_Province T";
            sql += " order by nameEN";
            DynamicParameters parameters = new DynamicParameters();          
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Province>(sql).ToList());
            }

        }

        public async Task<List<Region>> GetRegionList()
        {
            string sql = "SELECT T.* FROM [SIDE_PLM_Region] T";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Region>(sql).ToList());
            }
        }

    }
}
