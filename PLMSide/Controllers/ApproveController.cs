using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveController : Controller
    {
        private readonly IBLR_POSReviewRepository bLR_POSReviewRepository;
        private readonly IPOS_MasterRepository pos_MasterRepository;
        private readonly IUserRepository userRepository;
        public ApproveController(IBLR_POSReviewRepository _blR_POSReviewRepository, IPOS_MasterRepository _pos_MasterRepository, IUserRepository _userRepository)
        {
            bLR_POSReviewRepository = _blR_POSReviewRepository;
            pos_MasterRepository = _pos_MasterRepository;
            userRepository = _userRepository;
        }
        /// <summary>
        /// Account Sales 提交
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <param name="AccountSalesID"></param>
        /// <returns></returns>
        [HttpGet("Confirm/{Year}/{Season}/{AccountSalesID}")]
        public async Task<OkObjectResult> Confirm(string Year, string Season, string AccountSalesID)
        {
            try
            {
                //先获取需要审批的店铺 year season account_sales
                List<POS_Master> pos_list = await pos_MasterRepository.GetApprovalPOS(Year, Season, AccountSalesID);
                //新增POS 审批记录
                foreach (var item in pos_list)
                {
                    BLR_POSReview result = await bLR_POSReviewRepository.GetSinglePOSReview(Year, Season, item.POS_CODE);
                    if (result == null)
                    {
                        BLR_POSReview entity = new BLR_POSReview();
                        entity.POSCode = item.POS_CODE;
                        entity.AccountSales = AccountSalesID;
                        entity.AccountSalesName = AccountSalesID;
                        entity.AccountSalesCreateTime = DateTime.Now;
                        entity.Season = Season;
                        entity.Year = Year;
                        entity.PosState = "Submitted By AS";
                        await bLR_POSReviewRepository.PostEntity(entity);
                    }
                    else {
                        result.AccountSalesCreateTime = DateTime.Now;
                        result.PosState = "Submitted By AS";
                        await bLR_POSReviewRepository.PutEntity(result);
                    }
                }
                return Ok(new { Status = "Success" });
            }
            catch (Exception EX)
            {

                return Ok(EX);
            }

        }
        /// <summary>
        /// 获取ASM审批列表(SO 根据SalesOperation获取所有)
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <returns></returns>
        [HttpGet("GetASMPOSApprove/{Year}/{Season}")]
        public async Task<OkObjectResult> GetASMPOSApprove(string Year, string Season)
        {
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles role = _users.Roles.Where(t => t.RoleName == "SO").FirstOrDefault();
            bool isSO = false;
            if (role != null)
            {
                isSO = true;
            }
            List<ASApprovalModel> list = await pos_MasterRepository.GetASMApproval(Year, Season, _users.ID, isSO, _users.name);
            return Ok(list);
        }
        /// <summary>
        /// ASM审批
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <param name="paramsCondition"></param>
        /// <returns></returns>
        [HttpGet("ASMApprove/{Year}/{Season}")]
        public async Task<OkObjectResult> ASMApprove(string Year, string Season, string paramsCondition)
        {
            try
            {
                string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
                //Roles role = _users.Roles.Where(t => t.RoleName == "SO").FirstOrDefault();
                //if (role == null)
                //{

                //}
                string[] arry = paramsCondition.Split("&&");
                foreach (var param in arry)
                {
                    string[] param_list = param.Split("_");
                    List<BLR_POSReview> list = await bLR_POSReviewRepository.GetASMApprove(Year, Season, param_list[0], param_list[1], param_list[2], param_list[3]);
                    foreach (var item in list)
                    {
                        item.ASM_ID = userid;
                        item.ASM_Name = _users.name;
                        item.ASM_ApproveTime = DateTime.Now;
                        item.PosState = "Approved By ASM";
                        await bLR_POSReviewRepository.PutEntity(item);
                    }
                }
                return Ok(new { Status = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        /// <summary>
        /// ASM拒绝
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <param name="paramsCondition"></param>
        /// <returns></returns>
        [HttpGet("ASMReject/{Year}/{Season}")]
        public async Task<OkObjectResult> ASMReject(string Year, string Season, string paramsCondition)
        {
            try
            {
                string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
                string[] arry = paramsCondition.Split("&&");
                foreach (var param in arry)
                {
                    string[] param_list = param.Split("_");
                    List<BLR_POSReview> list = await bLR_POSReviewRepository.GetASMApprove(Year, Season, param_list[0], param_list[1], param_list[2], param_list[3]);
                    foreach (var item in list)
                    {
                        item.ASM_ID = userid;
                        item.ASM_Name = _users.name;
                        item.ASM_ApproveTime = DateTime.Now;
                        item.PosState = "Rejected By ASM";
                        await bLR_POSReviewRepository.PutEntity(item);
                    }
                }
                return Ok(new { Status = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        /// <summary>
        /// SO ReOpen
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <param name="paramsCondition"></param>
        /// <returns></returns>
        [HttpGet("SOReOpen/{Year}/{Season}")]
        public async Task<OkObjectResult> SOReOpen(string Year, string Season, string paramsCondition)
        {
            try
            {
                string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
                string[] arry = paramsCondition.Split("&&");
                foreach (var param in arry)
                {
                    string[] param_list = param.Split("_");
                    List<BLR_POSReview> list = await bLR_POSReviewRepository.GetASMApprove(Year, Season, param_list[0], param_list[1], param_list[2], param_list[3]);
                    foreach (var item in list)
                    {
                        item.PosState = "In Processing";
                        await bLR_POSReviewRepository.PutEntity(item);
                    }
                }
                return Ok(new { Status = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        /// <summary>
        /// 统计Big Format
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Season"></param>
        /// <param name="ASM_ID"></param>
        /// <returns></returns>
        [HttpGet("StatisBigFormat/{Year}/{Season}/{ASM_ID}")]
        public async Task<OkObjectResult> StatisBigFormat(string Year, string Season, string ASM_ID)
        {
            try
            {
                object obj = await pos_MasterRepository.StatisBigFormat(Year, Season, ASM_ID);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        /// <summary>
        /// 是否是能更新BLR(AS 提交或者 ASM审批都不能更新)
        /// </summary>
        /// <param name="YEAR"></param>
        /// <param name="SEASON"></param>
        /// <param name="POSCode"></param>
        /// <returns></returns>
        [HttpGet("IsUpdateBLR/{YEAR}/{SEASON}/{POSCode}")]
        public async Task<OkObjectResult> IsUpdateBLR(string YEAR, string SEASON, string POSCode)
        {
            bool result = await bLR_POSReviewRepository.IsUpdateBLR(YEAR, SEASON, POSCode);
            return Ok(result);
        }
        [HttpGet("whetherSO")]
        public async Task<OkObjectResult> whetherSO() {
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles role = _users.Roles.Where(t => t.RoleName == "SO").FirstOrDefault();
            bool isSO = false;
            if (role != null)
            {
                isSO = true;
            }
            return Ok(new { whetherSO = isSO });
        }
        [HttpGet("whetherApprove")]
        public async Task<OkObjectResult> whetherApprove()
        {
            //只有BLR SO ASM可以看到Approve按钮
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles SO = _users.Roles.Where(t => t.RoleName == "SO").FirstOrDefault();
            Roles ASM = _users.Roles.Where(t => t.RoleName == "ASM").FirstOrDefault();
            Roles BLR = _users.Roles.Where(t => t.RoleName == "BLR").FirstOrDefault();
            bool result = false;
            if (SO != null || ASM!=null || BLR!=null)
            {
                result = true;
            }
            return Ok(new { whetherApprove = result });
        }
    }
}