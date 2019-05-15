using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface ICustomersInfoRepository : IRepositoryBase<CustomersInfo>
    {
        Task<Tuple<List<CustomersInfo>, int>> GetEntities(string whereStr,int currentPageIndex);
        Task<List<CustomersInfo>> GetEntities(string whereStr);
        Task<CustomersInfo> GetSingelEntity(int Id);
        Task<CustomersInfo> GetSingleEntityByCustomer_Code(string Customer_Code);
        Task<CustomersInfo> GetSingleEntityByShipTo_Code(string ShipTo_Code);
        Task PostEntity(CustomersInfo entity);
        Task PutEntityByCode(CustomersInfo entity);
        Task PutEntity(CustomersInfo entity);
        Task DeleteEntity(int Id);
        Task<List<Users>> GetAccountSalesByRole();
    }
}
