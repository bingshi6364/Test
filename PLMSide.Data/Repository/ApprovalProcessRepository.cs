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
    public class ApprovalProcessRepository : RepositoryBase<ApprovalProcess>, IApprovalProcessRepository
    {
        /// <summary>
        /// 根据筛选条件获取数据(分页)
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="currentPageIndex"></param>
        /// <returns></returns>
        public async Task<Tuple<List<ApprovalProcess>, int>> GetEntities(string whereStr, int currentPageIndex)
        {
            Tuple<List<ApprovalProcess>, int> tp = await GetPageByProcList("SIDE_PLM_BLR_Approve_Process", "*", whereStr, "ID", currentPageIndex, 10);
            return tp;
        }
        /// <summary>
        /// Get Singe Entity
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ApprovalProcess> GetSingelEntity(int Id)
        {
            string selectSql = @"select ID,AS_ID, Account_Sales, ASM_ID, ASM from [dbo].[SIDE_PLM_BLR_Approve_Process] where ID=@Id";
            return await Detail(Id, selectSql);
        }
        /// <summary>
        /// Post ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task PostEntity(ApprovalProcess entity)
        {
            string insertSql = @"INSERT INTO [dbo].[SIDE_PLM_BLR_Approve_Process](AS_ID, Account_Sales, ASM_ID, ASM) VALUES( @AS_ID, @Account_Sales, @ASM_ID, @ASM)";
            await Insert(entity, insertSql);
        }
        /// <summary>
        /// Put ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task PutEntity(ApprovalProcess entity)
        {
            string updateSql = @"UPDATE [dbo].[SIDE_PLM_BLR_Approve_Process] SET 
                                AS_ID=@AS_ID, 
                                Account_Sales=@Account_Sales, 
                                ASM_ID=@ASM_ID, 
                                ASM=@ASM
                                WHERE Id=@Id";
            await Update(entity, updateSql);
        }
        /// <summary>
        /// Delete ApproalProcess
        /// </summary>
        /// <returns></returns>
        public async Task DeleteEntity(int Id)
        {
            string deleteSql = "DELETE FROM [dbo].[SIDE_PLM_BLR_Approve_Process] WHERE Id=@Id";
            await Delete(Id, deleteSql);
        }
        /// <summary>
        /// 通过角色名获取所有Account Sales(已过滤掉了添加过的Account Sales)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetAccountSalesByRole()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Users>("select * from SIDE_VW_GetAccountSalesByRole", null).ToList());
            }
        }
        /// <summary>
        /// 通过角色名获取所有ASM
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetASMByRole()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Users>("select * from SIDE_VW_GetASMByRole", null).ToList());
            }
        }
        /// <summary>
        /// 通过角色名获取所有CategorySales
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetCategorySalesByRole()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Users>("select * from SIDE_VW_GetCategotySalesByRole", null).ToList());
            }
        }
        /// <summary>
        /// 通过角色名获取所有AcountSales(Kids)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Users>> GetAS_KidsByRole()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<Users>("select * from SIDE_VM_GetAS_KidsByRole", null).ToList());
            }
        }

    }
}
