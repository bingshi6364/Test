using Dapper;
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
    public class CustomerGroupRepository : RepositoryBase<CustomerGroup>, ICustomerGroupRepository
    {
        public async Task<List<CustomerGroup>> GetCustomerGroups()
        {
            string selectsql = "select * from SIDE_PLM_BLR_Customer_Group order by id";
            return await Select(selectsql);
        }

        public async Task<List<int>> GetOwnChannels(string username)
        {
            string selectsql = @"select distinct [ID] from SIDE_PLM_BLR_Customer_Group where ID in(
select substring(Branch+',',number,charindex(',',Branch+',',number)-number)
from SIDE_PLM_BLR_SalesOperation ,master..spt_values s
where type='p' and number>0 and substring(','+Branch,number,1)=','
and SalesName=@username)
";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.Query<int>(selectsql, new { username = username }).ToList());
            }
        }

        public async Task<List<string>> GetOwnCustomers(string username)
        {
            string selectsql = @"select distinct [Customer] from SIDE_PLM_BLR_Customer_Group where ID in(
select substring(Branch+',',number,charindex(',',Branch+',',number)-number)
from SIDE_PLM_BLR_SalesOperation ,master..spt_values s
where type='p' and number>0 and substring(','+Branch,number,1)=','
and SalesName=@username)
";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.Query<string>(selectsql, new { username = username }).ToList());
            }
        }

        public async Task<List<string>> GetOwnGroups(string username)
        {
            string selectsql = @"select distinct [Group] from SIDE_PLM_BLR_Customer_Group where ID in(
select substring(Branch+',',number,charindex(',',Branch+',',number)-number)
from SIDE_PLM_BLR_SalesOperation ,master..spt_values s
where type='p' and number>0 and substring(','+Branch,number,1)=','
and SalesName=@username)
";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                //string selectSql = @"SELECT Id, UserName, Password, Gender, Birthday, CreateDate, IsDelete FROM dbo.Users";
                return await Task.Run(() => conn.Query<string>(selectsql, new { username = username }).ToList());
            }
        }

        public async Task UpdateSalesOperation(List<CustomerGroup> channels, string username)
        {
            string ownchannels = "";
            foreach (var channel in channels)
            {
                ownchannels += channel.ID + ",";
            }
            string updatesql = @"if exists (select * from SIDE_PLM_BLR_SalesOperation where SalesName=@username)
                            update SIDE_PLM_BLR_SalesOperation set Branch=@ownchannels where SalesName=@username
                            else 
                            insert into SIDE_PLM_BLR_SalesOperation(SalesName,Branch)values(@username,@ownchannels)
             ";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                await Task.Run(() => conn.ExecuteAsync(updatesql, new { username, ownchannels = ownchannels.Substring(0, ownchannels.Length - 1) }));
            }



        }

        public async Task<List<object>> GetCustomerGroupByGroup(string group)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<object>("select distinct [Customer]   from SIDE_PLM_BLR_Customer_Group where [group]=@group", new { group }).ToList());
            }
        }

        public async Task<List<CustomerGroup>> GetBranchByCustomer_group(string customer_group)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<CustomerGroup>
                (" select [Id],[Group],[Customer],[Branch],[BranchCN] from [dbo].[SIDE_PLM_BLR_Customer_Group] where Customer=@customer_group", new { customer_group }).ToList());
            }
        }

        public async Task<List<CustomerGroup>> GetBranchAll()
        {

            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<CustomerGroup>
                (" select distinct Branch,BranchCN,[Group] from[dbo].[SIDE_PLM_BLR_Customer_Group] where isnull(Branch,'') <>'' ").ToList());
            }


        }

    }





        
}
