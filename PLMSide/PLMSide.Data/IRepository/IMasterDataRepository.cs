using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
   public interface IMasterDataRepository
    {
        Task<List<MD_Big_Format>> GetBig_Formats();
        Task<List<MD_Big_Format>> GetBig_FormatsPOSChannel(string Channel);
        
        Task<List<MD_Buying_Region>> GetMD_Buying_Region();
        Task<List<MD_Grading>> GetMD_Grading(string channel);
        Task<List<MD_POS_Channel>> GetMD_POS_Channel();
     
        Task<List<MD_Segmentation>> GetMD_Segmentation();
        Task<List<MD_SubChannel>> GetMD_SubChannel();
        Task<List<MD_SubChannel>> GetMD_SubChannelByChannel(string Channel);
        Task<List<MD_WHS_Channel>> GetMD_WHS_Channel();
        Task<List<BLR_OCSGrading>> GetBLR_OCSGrading();
        Task<List<object>> GetOSCGrading();
        Task<List<MD_Product_Channel>> GetMD_Product_Channel();

       

        #region POS Type
        Task<List<MD_POS_Type>> GetMD_POS_Type();
        int AddMD_POS_Type(MD_POS_Type model);
        int UpdateMD_POS_Type(MD_POS_Type model);
        Task<MD_POS_Type> GetMD_POS_TypeModel(int Id);
        Task<Tuple<List<MD_POS_Type>, int>> GetPageByProcList_MD_POS_Type(string viewName, string fieldName = "*", string whereString = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10);
        Task<List<object>> GetPOSType(string segmentation);
        int UpdateMD_POS_TypeStatus(int Status, int ID);

        #endregion

        #region MD_BulkShipTo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddMD_BulkShipTo(MD_BulkShipTo model);
        int UpdateMD_BulkShipTo(MD_BulkShipTo model);
        Task<MD_BulkShipTo> GetMD_BulkShipToByBranch(string Branch);
        Task<MD_BulkShipTo> GetMD_BulkShipToSingleModel(int Id);

        Task<Tuple<List<MD_BulkShipTo>, int>> GetPageByProcList_MD_BulkShipTo(string viewName, string fieldName = "*", string whereString = " 1=1", string orderString = "ID", int page = 1, int pageSize = 10);

        #endregion
       
       
      

    }
}
