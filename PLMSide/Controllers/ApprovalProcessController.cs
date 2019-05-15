using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalProcessController : Controller
    {
        private readonly IApprovalProcessRepository Repository;
        public ApprovalProcessController(IApprovalProcessRepository _repository)
        {
            Repository = _repository;
        }
        /// <summary>
        /// 获取所有实体（可通过Account Sales进行模糊查询）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetEntities(string account_sales, string ASM, int currentPageIndex=1)
        {
            string whereStr = string.Empty;
            if (!string.IsNullOrEmpty(account_sales))
            {
                whereStr = string.Format("Account_Sales like '%{0}%'", account_sales);
            }
            if (!string.IsNullOrEmpty(ASM))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" ASM like '%{0}%'", ASM);
            }
            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = "1=1";
            }
            Tuple<List<ApprovalProcess>, int> tp = await Repository.GetEntities(whereStr, currentPageIndex);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }
        /// <summary>
        /// 通过Id获取单个实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<object> GetSingleEntity(int Id)
        {
            ApprovalProcess entity = await Repository.GetSingelEntity(Id);
            return Ok(entity);
        }
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task PostEntity(ApprovalProcess entity)
        {
            await Repository.PostEntity(entity);
        }
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task PutEntity(ApprovalProcess entity)
        {
            await Repository.PutEntity(entity);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteEntity(int Id)
        {
            await Repository.DeleteEntity(Id);
        }
        /// <summary>
        /// 获取所有AccountSales角色的所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAccountSalesByRole")]
        public async Task<object> GetAccountSalesByRole()
        {
            List<Users> users_accountSales = await Repository.GetAccountSalesByRole();
            return Ok(users_accountSales);
        }
        /// <summary>
        /// 获取所有ASM角色的所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetASMByRole")]
        public async Task<object> GetASMByRole()
        {
            List<Users> users_accountSales = await Repository.GetASMByRole();
            return Ok(users_accountSales);
        }
        /// <summary>
        /// 获取所有Category Sales角色的所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategorySalesByRole")]
        public async Task<object> GetCategorySalesByRole()
        {
            List<Users> users_accountSales = await Repository.GetCategorySalesByRole();
            return Ok(users_accountSales);
        }
        /// <summary>
        /// 通过角色名获取所有AcountSales(Kids)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetASkidsByRole")]
        public async Task<object> GetAS_KidsByRole()
        {
            List<Users> users_accountSales = await Repository.GetAS_KidsByRole();
            return Ok(users_accountSales);
        }
    }
}