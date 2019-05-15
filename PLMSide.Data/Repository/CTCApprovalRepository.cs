using Dapper;
using PLMSide.Data.Dto;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class CTCApprovalRepository : ICTCApprovalRepository
    {
        public async Task<List<POS_Master>> GetExportList(CTCApproval Model)
        {

            string sqlText = "select * from SIDE_PLM_V_POS_Master where 1=1";
            DynamicParameters parm = new DynamicParameters();
            if (!string.IsNullOrEmpty(Model.Year))
            {
                sqlText += " and TM_YEAR = @Year";
                parm.Add("Year", Model.Year);
            }
            if (!string.IsNullOrEmpty(Model.Season))
            {
                sqlText += " and TM_SEASON = @Season";
                parm.Add("Season", Model.Season);
            }
            if (!string.IsNullOrEmpty(Model.Group))
            {
                sqlText += " and CUSTOMER_GROUP = @Group";
                parm.Add("Group", Model.Group);
            }
            if (!string.IsNullOrEmpty(Model.Branch))
            {
                sqlText += " and BRANCH_EN = @Branch";
                parm.Add("Branch", Model.Branch);
            }
            if (!string.IsNullOrEmpty(Model.Channel))
            {
                sqlText += " and POS_CHANNEL = @Channel";
                parm.Add("Channel", Model.Channel);
            }
            if (!string.IsNullOrEmpty(Model.City))
            {
                sqlText += " and City_CN LIKE @City";
                parm.Add("City", "%"+Model.City+"%");
            }
            if (!string.IsNullOrEmpty(Model.Province))
            {
                sqlText += " and PROVINCE_CN LIKE @Province";
                parm.Add("Province", "%" + Model.Province + "%");
            }
            if (!string.IsNullOrEmpty(Model.Sellspacebegin) && string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE >= @Sellspacebegin";
                parm.Add("Sellspacebegin", Model.Sellspacebegin);
            }
            if (!string.IsNullOrEmpty(Model.Sellspacebegin) && string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE <= @Sellspaceend";
                parm.Add("Sellspaceend", Model.Sellspaceend);
            }
            if (!string.IsNullOrEmpty(Model.Province) && !string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE between @Sellspacebegin and @Sellspaceend";
                parm.Add("Sellspacebegin", Model.Sellspacebegin);
                parm.Add("Sellspaceend", Model.Sellspaceend);
            }
            if (!string.IsNullOrEmpty(Model.Range))
            {
                sqlText += " and (POS_Type is NULL OR POS_Type in (select POS_TYPE from SIDE_MD_POS_Type WHERE Type_Status=0) )";
            }

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                var list =await conn.QueryAsync<POS_Master>(sqlText, parm);
                return list.ToList();
            }
        }

        //public async Task<Tuple<List<POS_Master>, int>> GetPageByProcList(string viewName, string fieldName = "*", string wherestring = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10)
        //{
        //    int recordTotal = 0;

        //    using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
        //    {
        //        //conn.Open();
        //        DynamicParameters parm = new DynamicParameters();
        //        parm.Add("viewName", viewName);
        //        parm.Add("fieldName", fieldName);
        //        parm.Add("whereString", wherestring);
        //        parm.Add("pageSize", pageSize);
        //        parm.Add("pageNo", page);
        //        parm.Add("orderString", orderString);
        //        parm.Add("recordTotal", 0, DbType.Int32, ParameterDirection.Output);

        //        var list = await Task.Run(() => conn.Query<POS_Master>("SIDE_PLM_ProcViewPager", parm, commandType: CommandType.StoredProcedure).ToList());
        //        recordTotal = parm.Get<int>("@recordTotal");//返回总页数

        //        //  conn.Close();
        //        return new Tuple<List<POS_Master>, int>(list, recordTotal);

        //    }
        //}

        public async Task<Tuple<List<POS_Master>, int>> GetPageByProcList(CTCApproval Model)
        {
            
            string sqlText = "select * from SIDE_PLM_V_POS_Master where 1=1";
            DynamicParameters parm = new DynamicParameters();
            if (!string.IsNullOrEmpty(Model.Year))
            {
                sqlText += " and TM_YEAR = @Year";
                parm.Add("Year", Model.Year);
            }
            if (!string.IsNullOrEmpty(Model.Season))
            {
                sqlText += " and TM_SEASON = @Season";
                parm.Add("Season", Model.Season);
            }
            if (!string.IsNullOrEmpty(Model.Group))
            {
                sqlText += " and CUSTOMER_GROUP = @Group";
                parm.Add("Group", Model.Group);
            }
            if (!string.IsNullOrEmpty(Model.Branch))
            {
                sqlText += " and BRANCH_EN = @Branch";
                parm.Add("Branch", Model.Branch);
            }
            if (!string.IsNullOrEmpty(Model.Channel))
            {
                sqlText += " and POS_CHANNEL = @Channel";
                parm.Add("Channel", Model.Channel);
            }
            if (!string.IsNullOrEmpty(Model.City))
            {
                sqlText += " and City_CN LIKE @City";
                parm.Add("City", "%" + Model.City + "%");
            }
            if (!string.IsNullOrEmpty(Model.Province))
            {
                sqlText += " and PROVINCE_CN LIKE @Province";
                parm.Add("Province", "%" + Model.Province + "%");
            }
            if (!string.IsNullOrEmpty(Model.Sellspacebegin) && string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE >= @Sellspacebegin";
                parm.Add("Sellspacebegin", Model.Sellspacebegin);
            }
            if (string.IsNullOrEmpty(Model.Sellspacebegin) && !string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE <= @Sellspaceend";
                parm.Add("Sellspaceend", Model.Sellspaceend);
            }
            if (!string.IsNullOrEmpty(Model.Province) && !string.IsNullOrEmpty(Model.Sellspaceend))
            {
                sqlText += " and SELLING_SPACE between @Sellspacebegin and @Sellspaceend";
                parm.Add("Sellspacebegin", Model.Sellspacebegin);
                parm.Add("Sellspaceend", Model.Sellspaceend);
            }
            if (!string.IsNullOrEmpty(Model.Range))
            {
                sqlText += " and (POS_Type is NULL OR POS_Type in (select POS_TYPE from SIDE_MD_POS_Type WHERE Type_Status=0) )";
            }

            parm.Add("PageIndex", Model.PageIndex);
            parm.Add("PageSize", Model.PageSize);

            string selectQuery = @" ;WITH _data AS ("+ sqlText+@"                    
                    ),
                      _count AS (
                        SELECT COUNT(1) AS TotalCount FROM _data
                    )
                    SELECT * FROM _data CROSS APPLY _count Order by ID OFFSET @PageIndex * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
                    ";
            string sqlText2=sqlText.Replace("*", "COUNT(*)");
            selectQuery += sqlText2;
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                var list = await conn.QueryMultipleAsync(selectQuery, parm);
                var i = list.Read<POS_Master>();
                var s = list.Read<int>().Single();
                Tuple<List<POS_Master>, int> tuple = new Tuple<List<POS_Master>, int>(i.ToList(), s);
                return tuple;
            }
        }

        public async Task<List<POS_MasterImport>> ImportData(List<POS_MasterImport> Model,List<string> colString,string ProcName)
        {
            StringBuilder sbCol = new StringBuilder();
            StringBuilder sbColProp= new StringBuilder();
            foreach (var col in colString)
            {
                sbCol.Append(col + ",");
                sbColProp.Append("@"+col + ",");
            }
            string Col = sbCol.ToString().Substring(0, sbCol.ToString().Length - 1);
            string ColProp= sbColProp.ToString().Substring(0, sbColProp.ToString().Length - 1);
            string insertSql = $"insert into [SIDE_PLM_POS_MasterImport]({Col}) Values({ColProp})";
            string delSql = $"delete SIDE_PLM_POS_MasterImport where UserID={Model[0].UserID}";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    await conn.ExecuteAsync(delSql,null,transaction);
                    foreach (var mol in Model)
                    {
                        await conn.ExecuteAsync(insertSql, mol,transaction);
                    }
                    DynamicParameters parm = new DynamicParameters();
                    parm.Add("USERID", Model[0].UserID);
                    //
                    var list = await Task.Run(() => conn.Query<POS_MasterImport>(ProcName, parm, transaction, commandType: CommandType.StoredProcedure).ToList());
                    transaction.Commit();
                    return list;
                }
                catch(Exception exception)
                {
                   
                    transaction.Rollback();
                    return null;
                    throw new Exception(exception.Message);

                }
            }
            
        }
    }
}
