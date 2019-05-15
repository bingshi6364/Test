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
    public class SIDE_PLM_STARepository: RepositoryBase<SIDE_PLM_STA>,ISIDE_PLM_STARepository
    {
        /// <summary>
        /// 根据筛选条件获取数据(分页)
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="currentPageIndex"></param>
        /// <returns></returns>
        public async Task<Tuple<List<SIDE_PLM_STA>, int>> GetEntitiesByPaging(string whereStr, int currentPageIndex)
        {
            Tuple<List<SIDE_PLM_STA>, int> tp = await GetPageByProcList("SIDE_VM_ShipTo", "*", whereStr, "Customer_Code", currentPageIndex, 10);
            return tp;
        }
        public async Task<List<SIDE_PLM_STA>> GetEntities(string whereStr)
        {
            string sql = $"select *  from SIDE_VM_ShipTo where {whereStr}";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection())
            {
                return await Task.Run(() => conn.Query<SIDE_PLM_STA>(sql).ToList());
            }
        }
    }
}
