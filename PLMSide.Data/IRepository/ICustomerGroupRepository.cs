using PLMSide.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PLMSide.Data.IRepository
{
    public interface ICustomerGroupRepository
    {
        Task<List<CustomerGroup>> GetCustomerGroups();

        Task UpdateSalesOperation(List<CustomerGroup> group, string username);

        Task<List<string>> GetOwnGroups(string username);

        Task<List<string>> GetOwnCustomers(string username);

        Task<List<int>> GetOwnChannels(string username);
        
        /// <summary>
        /// 根据Group获取所有的Customer_group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<List<object>> GetCustomerGroupByGroup(string group);
        /// <summary>
        /// 根据customer_group获取所有的Branch
        /// </summary>
        /// <param name="customer_group"></param>
        /// <returns></returns>
        Task<List<CustomerGroup>> GetBranchByCustomer_group(string customer_group);
        Task<List<CustomerGroup>> GetBranchAll();
        
    }
}
