using PLMSide.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IBLR_POSReviewRepository : IRepositoryBase<BLR_POSReview>
    {
        Task PostEntity(BLR_POSReview entity);
        Task PutEntity(BLR_POSReview entity);
        Task<List<BLR_POSReview>> GetASMApprove(string YEAR, string SEASON, string ASM_ID);
        Task<List<BLR_POSReview>> GetASMApprove(string YEAR, string SEASON,  string Channel, string customer, string Branch, string AS_ID);
        Task<bool> IsUpdateBLR(string YEAR, string SEASON, string POSCode);
        Task<BLR_POSReview> GetSinglePOSReview(string YEAR, string SEASON, string POSCode);
    }
}
