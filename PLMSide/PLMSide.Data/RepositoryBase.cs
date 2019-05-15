using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Data
{
    public class RepositoryBase<T> : IRepositoryBase<T>
    {
        public async Task Delete(int Id, string deleteSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                await conn.ExecuteAsync(deleteSql, new { Id = Id });
            }
        }

        public async Task<T> Detail(int Id, string detailSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string querySql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users WHERE Id=@Id";
                return await conn.QueryFirstOrDefaultAsync<T>(detailSql, new { Id = Id });
            }
        }

        /// <summary>
        /// 无参存储过程
        /// </summary>
        /// <param name="SPName"></param>
        /// <returns></returns>
        public async Task<List<T>> ExecQuerySP(string SPName)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<T>(SPName, null, null, true, null, CommandType.StoredProcedure).ToList());
            }
        }

        public async Task<Tuple<List<T>, int>> GetPageByProcList(string viewName, string fieldName = "*", string whereString = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10)
        {
            int recordTotal = 0;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //conn.Open();
                DynamicParameters parm = new DynamicParameters();
                parm.Add("viewName", viewName);
                parm.Add("fieldName", fieldName);
                parm.Add("whereString", whereString);
                parm.Add("pageSize", pageSize);
                parm.Add("pageNo", page);
                parm.Add("orderString", orderString);
                parm.Add("recordTotal", 0, DbType.Int32, ParameterDirection.Output);

                var list = await Task.Run(() => conn.Query<T>("SIDE_PLM_ProcViewPager", parm, commandType: CommandType.StoredProcedure).ToList());
                recordTotal = parm.Get<int>("@recordTotal");//返回总页数

                //  conn.Close();
                return new Tuple<List<T>, int>(list, recordTotal);

            }


        }



        public async Task Insert(T entity, string insertSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                await conn.ExecuteAsync(insertSql, entity);
            }
        }

        public async Task<List<T>> SelectPara(string selectSql, object param = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                return await Task.Run(() => conn.Query<T>(selectSql, param).ToList());
            }
        }

        public async Task<List<T>> Select(string selectSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.Query<T>(selectSql).ToList());
            }
        }

        public async Task<T> Select(int Id, string selectSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.QuerySingle<T>(selectSql, new { Id = Id }));
            }
        }

        public async Task Update(T entity, string updateSql)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                await conn.ExecuteAsync(updateSql, entity);
            }
        }
    }
}
