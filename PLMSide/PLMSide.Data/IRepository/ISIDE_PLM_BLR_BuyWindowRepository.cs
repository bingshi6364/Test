using PLMSide.Data.Entites;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{





    public interface ISIDE_PLM_BLR_BuyWindowRepository : IRepositoryBase<SIDE_PLM_BLR_BuyWindow>
    {
        Task<List<SIDE_PLM_BLR_BuyWindow>> GetSIDE_PLM_BLR_BuyWindowList();

        ResultModel AddBuyWindowSP(SIDE_PLM_BLR_BuyWindow model);

        ResultModel NewBuyWindowSP(SIDE_PLM_BLR_BuyWindow model);

        /// <summary>
        /// 修改买货窗口 部分信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
          Task UpdateEntity(SIDE_PLM_BLR_BuyWindow model);
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        Task<Tuple<List<SIDE_PLM_BLR_BuyWindow>, int>> GetPageModelList(string whereStr);
        /// <summary>
        /// 查询没有新建买货窗口的季度列表
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        Task<List<string>> GetSeasonListByYear(string Year);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         Task<List<SIDE_PLM_BLR_BuyWindow>> GetSIDE_PLM_BLR_BuyWindowListByState(int State);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<SIDE_PLM_BLR_BuyWindow> GetModel(int ID);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<SIDE_PLM_BLR_BuyWindow> GetModel(string Year, string Season);
        /// <summary>
        /// 获取最新的买货窗口
        /// </summary>
        /// <returns></returns>
        Task<SIDE_PLM_BLR_BuyWindow> GetNewestBuyWindow();

        
    }

}
