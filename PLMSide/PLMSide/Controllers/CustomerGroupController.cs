using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerGroupController : ControllerBase
    {
        private readonly ICustomerGroupRepository customergroupRepository;
        public CustomerGroupController(ICustomerGroupRepository _customergroupRepository)
        {
            customergroupRepository = _customergroupRepository;
        }

        [HttpGet]
        public async Task<object> GetCustomerGroup()
        {
            List<CustomerGroup> list = await customergroupRepository.GetCustomerGroups();
            return Ok(list);
        }

        [HttpGet("GetBranchAll")]
        public async Task<object> GetBranchAll()
        {
            List<CustomerGroup> list = await customergroupRepository.GetBranchAll();
            return Ok(list);
        }

        
        [HttpGet("Group")]
        public async Task<object> GetOwnGroup()
        {
            List<string> list = await customergroupRepository.GetOwnGroups(HttpContext.User.Identity.Name);
            return Ok(list);
        }

        [HttpGet("Customer")]
        public async Task<object> GetOwnCustomemr()
        {
            List<string> list = await customergroupRepository.GetOwnCustomers(HttpContext.User.Identity.Name);
            return Ok(list);
        }

        [HttpGet("Channel")]
        public async Task<object> GetOwnChannel()
        {
            List<int> list = await customergroupRepository.GetOwnChannels(HttpContext.User.Identity.Name);
            return Ok(list);
        }

        [HttpPut]
        public async Task<object> UpdateSalesOperation(List<CustomerGroup> group)
        {
            await customergroupRepository.UpdateSalesOperation(group, HttpContext.User.Identity.Name);
            return Ok(new
            {
                result = "success"
            });
        }
        
        [HttpGet("GetCustomerGroupByGroup")]
        public async Task<object> GetCustomerGroupByGroup(string group)
        {
            List<object> Groups = await customergroupRepository.GetCustomerGroupByGroup(group);
            return Ok(Groups);
        }
        [HttpGet("GetBranchByCustomer_group")]
        public async Task<object> GetBranchByCustomer_group(string customer_group)
        {
            List<CustomerGroup> customerGroups = await customergroupRepository.GetBranchByCustomer_group(customer_group);
            return Ok(customerGroups);
        }
    }
}