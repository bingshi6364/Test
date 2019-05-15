using Dapper;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.Repository
{
    public class CustomersInfoRepository : RepositoryBase<CustomersInfo>, ICustomersInfoRepository
    {
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<List<CustomersInfo>, int>> GetEntities(string whereStr,int currentPageIndex)
        {
            Tuple<List<CustomersInfo>, int> tp = await GetPageByProcList("SIDE_PLM_BLR_CustomersInfo", "*", whereStr, "ID", currentPageIndex, 10);
            return tp;
        }
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomersInfo>> GetEntities(string whereStr)
        {
            string sql = $"select *  from SIDE_PLM_BLR_CustomersInfo where {whereStr}";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<CustomersInfo>(sql).ToList());
            }
        }
        /// <summary>
        /// 通过角色名获取所有Account Sales
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetAccountSalesByRole()
        {
            string sql = @"SELECT DISTINCT 
                         dbo.SIDE_PLM_Employee.id, dbo.SIDE_PLM_Employee.region, dbo.SIDE_PLM_Employee.name, dbo.SIDE_PLM_Employee.ntAccount, dbo.SIDE_PLM_Employee.isValid, 
                         dbo.SIDE_PLM_Employee.isHeadquarter, dbo.SIDE_PLM_Employee.email, dbo.SIDE_PLM_Employee.is_internal, dbo.SIDE_PLM_Employee.password_encryption
                                FROM            (SELECT        id
                                                          FROM            dbo.SIDE_PLM_Role
                                                          WHERE        (nameEN = 'Account Sales(RA)') OR
                                                                                    (nameEN = 'Account Sales(Kids)') OR
                                                                                    (nameEN = 'Account Sales(Belle)') OR
                                                                                    (nameEN = 'Account Sales(YY)') OR
                                                                                    (nameEN = 'Account Sales(MB)')) AS tmep_role INNER JOIN
                                                         dbo.SIDE_PLM_EmployeeRole ON tmep_role.id = dbo.SIDE_PLM_EmployeeRole.role INNER JOIN
                                                         dbo.SIDE_PLM_Employee ON dbo.SIDE_PLM_EmployeeRole.employee = dbo.SIDE_PLM_Employee.id
                                ";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Users>(sql, null).ToList());
            }
        }

        public async Task<CustomersInfo> GetSingleEntityByCustomer_Code(string Customer_Code)
        {
            string sql = $"select top 1  *  from SIDE_PLM_BLR_CustomersInfo where  [Status]='Y' and Customer_Code=@Customer_Code ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Customer_Code", Customer_Code, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<CustomersInfo>(sql, parameters).FirstOrDefault());

            }

        }

        public async Task<CustomersInfo> GetSingleEntityByShipTo_Code(string ShipTo_Code)
        {
           
            string sql = " select top 1  *  from SIDE_PLM_BLR_CustomersInfo where [Status]='Y' and Customer_Code = ( select customer  from SIDE_PLM_STA where id = @ShipTo_Code )";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ShipTo_Code", ShipTo_Code, DbType.String, ParameterDirection.Input);
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<CustomersInfo>(sql, parameters).FirstOrDefault());

            }

        }
   
            /// <summary>
            /// Get Singel Entity By Id
            /// </summary>
            /// <param name="Id"></param>
            /// <returns></returns>
        public async Task<CustomersInfo> GetSingelEntity(int Id)
        {
            string selectSql = @"select ID,
                                Customer_Code, 
                                Customer_Name, 
                                Customer_Group, 
                                branch_EN, 
                                branch_CN,
                                WHS_Channel,
                                Account_Sales,
                                Account_SalesID,
                                Category_Sales,
                                Category_SalesID,
                                Status,
                                kids_account_sales,
                                kids_account_salesID,
                                Product_Channel
                                from [dbo].[SIDE_PLM_BLR_CustomersInfo] where ID=@Id";
            return await Detail(Id, selectSql);
        }
        /// <summary>
        /// Post ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task PostEntity(CustomersInfo entity)
        {
            string insertSql = @"INSERT INTO [dbo].[SIDE_PLM_BLR_CustomersInfo](
                                Customer_Code, 
                                Customer_Name, 
                                Customer_Group, 
                                branch_EN, 
                                branch_CN,
                                WHS_Channel,
                                Account_Sales,
                                Account_SalesID,
                                Category_Sales,
                                Category_SalesID,
                                Status,
                                kids_account_sales,
                                kids_account_salesID,
                                Product_Channel) VALUES
                                (   @Customer_Code,
                                    @Customer_Name, 
                                    @Customer_Group, 
                                    @branch_EN, 
                                    @branch_CN,
                                    @WHS_Channel,
                                    @Account_Sales,
                                    @Account_SalesID,
                                    @Category_Sales,
                                    @Category_SalesID,
                                    @Status,
                                    @kids_account_sales,
                                    @kids_account_salesID,
                                    @Product_Channel)";
            await Insert(entity, insertSql);
        }
        /// <summary>
        /// Put ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task PutEntity(CustomersInfo entity)
        {
            string updateSql = @"UPDATE [dbo].[SIDE_PLM_BLR_CustomersInfo] SET 
                                Customer_Code=@Customer_Code, 
                                Customer_Name=@Customer_Name, 
                                Customer_Group=@Customer_Group, 
                                branch_EN=@branch_EN, 
                                branch_CN=@branch_CN,
                                WHS_Channel=@WHS_Channel,
                                Account_Sales=@Account_Sales,
                                Account_SalesID=@Account_SalesID,
                                Category_Sales=@Category_Sales,
                                Category_SalesID=@Category_SalesID,
                                Status=@Status,
                                kids_account_sales=@kids_account_sales,
                                kids_account_salesID=@kids_account_salesID,
                                Product_Channel=@Product_Channel
                                WHERE Id=@Id";
            await Update(entity, updateSql);
        }
        /// <summary>
        /// Put ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task PutEntityByCode(CustomersInfo entity)
        {
            string updateSql = @"UPDATE [dbo].[SIDE_PLM_BLR_CustomersInfo] SET 
                                Customer_Name=@Customer_Name, 
                                Customer_Group=@Customer_Group, 
                                branch_EN=@branch_EN, 
                                branch_CN=@branch_CN,
                                WHS_Channel=@WHS_Channel,
                                Account_Sales=@Account_Sales,
                                Account_SalesID=@Account_SalesID,
                                Category_Sales=@Category_Sales,
                                Category_SalesID=@Category_SalesID,
                                Status=@Status,
                                kids_account_sales=@kids_account_sales,
                                kids_account_salesID=@kids_account_salesID,
                                Product_Channel=@Product_Channel
                                WHERE Customer_Code=@Customer_Code";
            await Update(entity, updateSql);
        }
        /// <summary>
        /// Delete ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task DeleteEntity(int Id)
        {
            string deleteSql = "DELETE FROM [dbo].[SIDE_PLM_BLR_CustomersInfo] WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }
    }
}
