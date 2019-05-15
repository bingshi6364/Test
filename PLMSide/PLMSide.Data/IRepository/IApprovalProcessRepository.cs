using PLMSide.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface IApprovalProcessRepository : IRepositoryBase<ApprovalProcess>
    {
        Task<Tuple<List<ApprovalProcess>, int>> GetEntities(string whereStr,int currentPageIndex);
        Task<ApprovalProcess> GetSingelEntity(int Id);
        Task PostEntity(ApprovalProcess entity);
        Task PutEntity(ApprovalProcess entity);
        Task DeleteEntity(int Id);
        Task<List<Users>> GetAccountSalesByRole();
        Task<List<Users>> GetASMByRole();
        Task<List<Users>> GetCategorySalesByRole();
        Task<List<Users>> GetAS_KidsByRole();
    }
}
