using Dapper;
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
    public class BLR_POSReviewRepository : RepositoryBase<BLR_POSReview>, IBLR_POSReviewRepository
    {
        public async Task PutEntity(BLR_POSReview entity)
        {
            string sql = @"update SIDE_PLM_BLR_POSReview set AccountSalesCreateTime=@AccountSalesCreateTime,
                            ASM_ID=@ASM_ID,ASM_Name=@ASM_Name,ASM_ApproveTime=@ASM_ApproveTime,PosState=@PosState where ID=@ID";
            await Insert(entity, sql);
        }
        public async Task PostEntity(BLR_POSReview entity)
        {
            string sql = @"insert into SIDE_PLM_BLR_POSReview (POSCode,AccountSales,AccountSalesName,AccountSalesCreateTime,Season,Year,PosState)
                        values (@POSCode,@AccountSales,@AccountSalesName,@AccountSalesCreateTime,@Season,@Year,@PosState)";
            await Update(entity, sql);
        }
        public async Task<List<BLR_POSReview>> GetASMApprove(string YEAR, string SEASON, string ASM_ID)
        {
            string sql = @"SELECT *  FROM  SIDE_PLM_BLR_POSReview 
                            WHERE [Year]=@YEAR and [Season]=@SEASON and [PosState]='Submitted By AS' and AccountSales in 
                              (select AS_ID  from SIDE_PLM_BLR_Approve_Process WHERE ASM_ID=@ASM_ID)";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<BLR_POSReview>(sql, new { YEAR, SEASON, ASM_ID }).ToList());
            }
        }
        public async Task<List<BLR_POSReview>> GetASMApprove(string YEAR, string SEASON, string Channel, string customer, string Branch, string AS_ID)
        {
            string sql = @" SELECT DISTINCT SIDE_PLM_BLR_POSReview.*  FROM  SIDE_PLM_BLR_POSReview INNER JOIN 
                              SIDE_PLM_POS_Master ON SIDE_PLM_BLR_POSReview.POSCode=SIDE_PLM_POS_Master.POS_CODE INNER JOIN 
                              [dbo].[SIDE_PLM_BLR_CustomersInfo]ON [dbo].[SIDE_PLM_POS_Master].CUSTOMER_CODE=[dbo].[SIDE_PLM_BLR_CustomersInfo].Customer_Code
                              WHERE [dbo].[SIDE_PLM_POS_Master].TM_YEAR=@YEAR AND [dbo].[SIDE_PLM_POS_Master].TM_SEASON=@SEASON
                              AND Account_SalesID=@AS_ID and [dbo].[SIDE_PLM_POS_Master].POS_CHANNEL=@Channel and [dbo].[SIDE_PLM_POS_Master].CUSTOMER_GROUP=@customer
                              and [dbo].[SIDE_PLM_POS_Master].BRANCH_EN=@Branch";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<BLR_POSReview>(sql, new { YEAR, SEASON, AS_ID, Channel, customer, Branch }).ToList());
            }
        }
        //根据Year Season POSCode 查询是否存在审批记录
        public async Task<bool> IsUpdateBLR(string YEAR, string SEASON, string POSCode)
        {
            string sql = @"  select *  from SIDE_PLM_BLR_POSReview
                            where [Year] = @YEAR and [Season] = @SEASON and [POSCode] = @POSCode";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                BLR_POSReview result= await Task.Run(() => conn.QueryFirstOrDefault<BLR_POSReview>(sql, new { YEAR, SEASON, POSCode }));
                if (result==null)
                {
                    //未提交
                    return true;
                }
                else {
                    if (result.PosState == "Submitted By AS" || result.PosState == "Approved By ASM")
                    {
                        return false;
                    }
                    else {
                        return true;
                    }
                   
                }
            }
        }
        public async Task<BLR_POSReview> GetSinglePOSReview(string YEAR, string SEASON, string POSCode) {
            string sql = @"  select *  from SIDE_PLM_BLR_POSReview
                            where [Year] = @YEAR and [Season] = @SEASON and [POSCode] = @POSCode";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                BLR_POSReview result = await Task.Run(() => conn.QueryFirstOrDefault<BLR_POSReview>(sql, new { YEAR, SEASON, POSCode }));
                return result;
            }
        }
    }
}
