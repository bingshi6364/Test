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
    public class SOApprovalRepository : ISOApprovalRepository
    {
        public async Task<string> CheckPosCode(string year,string season ,string dummycode, string poscode)
        {
            

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                DynamicParameters parm = new DynamicParameters();
                parm.Add("Year", year);
                parm.Add("@Season", season);
                parm.Add("DummyCode", dummycode);
                parm.Add("PosCode", poscode);
                var list = await Task.Run(() => conn.Query<string>("SIDE_PLM_CheckPoscodeAndDummyCode", parm, commandType: CommandType.StoredProcedure).ToList());
                string result = list.FirstOrDefault().ToString().Trim();
                return result;
            }
            

        }

        public async Task<List<string>> GetChannels()
        {
            string selectsql = "select CodeName from SIDE_MD_POS_Channel order by id";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = await conn.QueryAsync<string>(selectsql);
                return result.ToList();
            }
        }

        public async Task<List<CustomerGroup>> GetCustomerBranch()
        {
            string selectsql = "select * from SIDE_PLM_BLR_Customer_Group order by id";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result =await conn.QueryAsync<CustomerGroup>(selectsql);
                return result.ToList();
            }
        }

        public async Task<List<POS_Master>> GetExportList(SOApproval Model,string RoleName,string userid,string username,string roleid)
        {
            string sqlText = "select * from SIDE_PLM_V_POS_Master where 1=1";
            DynamicParameters parm = new DynamicParameters();
            if (RoleName == "AS")
            {
                sqlText += $" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where Account_SalesID={userid})";
            }
            else if (RoleName == "ASM")
            {
                sqlText += $" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where Account_SalesID IN (SELECT AS_ID FROM SIDE_PLM_BLR_Approve_Process WHERE ASM_ID={userid}))";
            }
            else if (RoleName == "SO")
            {
                sqlText += @" AND BRANCH_EN in ( select distinct [Branch] from SIDE_PLM_BLR_Customer_Group where ID in(
                           select substring(Branch+',',number,charindex(',',Branch+',',number)-number)
                           from SIDE_PLM_BLR_SalesOperation ,master..spt_values s
                           where type='p' and number>0 and substring(','+Branch,number,1)=','" +
                           $"and SalesName='{username}'))";
            }
            else if (roleid.Contains('4'))
            {
                sqlText += $" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where kids_account_salesID={userid})";
            }
            //else if (HttpContext.User.IsInRole("admin") || HttpContext.User.IsInRole("BLR") || HttpContext.User.IsInRole("RE") || HttpContext.User.IsInRole("CTC"))
            //{

            //}

            if (!string.IsNullOrEmpty(Model.year))
            {
                sqlText += " and TM_YEAR = @Year";
                parm.Add("Year", Model.year);
            }
            if (!string.IsNullOrEmpty(Model.season))
            {
                sqlText += " and TM_SEASON = @Season";
                parm.Add("Season", Model.season);
            }
            if (!string.IsNullOrEmpty(Model.customer))
            {
                sqlText += " and CUSTOMER_GROUP = @customer";
                parm.Add("customer", Model.customer);
            }
            if (!string.IsNullOrEmpty(Model.branch))
            {
                sqlText += " and BRANCH_EN = @Branch";
                parm.Add("Branch", Model.branch);
            }
            if (!string.IsNullOrEmpty(Model.channel))
            {
                sqlText += " and POS_CHANNEL = @Channel";
                parm.Add("Channel", Model.channel);
            }
            if (!string.IsNullOrEmpty(Model.city))
            {
                sqlText += " and City_CN LIKE @City";
                parm.Add("City", "%" + Model.city + "%");
            }
            if (!string.IsNullOrEmpty(Model.province))
            {
                sqlText += " and PROVINCE_CN LIKE @Province";
                parm.Add("Province", "%" + Model.province + "%");
            }
            if (!string.IsNullOrEmpty(Model.poscode))
            {
                sqlText += " and POS_CODE LIKE @POS_CODE";
                parm.Add("POS_CODE", "%" + Model.poscode + "%");
            }
            if (!string.IsNullOrEmpty(Model.format))
            {
                sqlText += " and BIG_FORMAT LIKE @Format";
                parm.Add("Format", "%" + Model.format + "%");
            }
            if (!string.IsNullOrEmpty(Model.posname))
            {
                sqlText += " and POS_NAME LIKE @Posname";
                parm.Add("Posname", "%" + Model.posname + "%");
            }


            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {

                var list = await conn.QueryAsync<POS_Master>(sqlText, parm);
                return list.ToList();
            }
        }

        public async Task<Tuple<List<POS_Master>, int>> GetPageByProcList(string viewName, string fieldName = "*", string wherestring = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10)
        {
            int recordTotal = 0;

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //conn.Open();
                DynamicParameters parm = new DynamicParameters();
                parm.Add("viewName", viewName);
                parm.Add("fieldName", fieldName);
                parm.Add("whereString", wherestring);
                parm.Add("pageSize", pageSize);
                parm.Add("pageNo", page);
                parm.Add("orderString", orderString);
                parm.Add("recordTotal", 0, DbType.Int32, ParameterDirection.Output);

                var list = await Task.Run(() => conn.Query<POS_Master>("SIDE_PLM_ProcViewPager", parm, commandType: CommandType.StoredProcedure).ToList());
                recordTotal = parm.Get<int>("@recordTotal");//返回总页数

                //  conn.Close();
                return new Tuple<List<POS_Master>, int>(list, recordTotal);

            }
        }


        public async Task<List<Province>> GetProvinces()
        {
            string selectsql = "select * from side_PLM_Province order by id";

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                var result = await conn.QueryAsync<Province>(selectsql);
                return result.ToList();
            }
        }
    }
}
