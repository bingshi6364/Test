using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Data;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POS_MasterController : Controller
    {
        private readonly IPOS_MasterRepository Repository;
        private readonly ICustomersInfoRepository CustomerRepository;
        private readonly IUserRepository userRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_repository"></param>
        public POS_MasterController(IPOS_MasterRepository _repository, ICustomersInfoRepository _customerRepository, IUserRepository _userRepository)
        {
            Repository = _repository;
            CustomerRepository = _customerRepository;
            userRepository = _userRepository;
        }


        /// <summary>
        /// 获取 Overview
        /// </summary>
        /// <returns></returns>
        ///        
        [HttpGet("GetListByUser/{UserId}/{Year}/{Season}")]
        public OkObjectResult GetListByUser(int UserId, string Year, string Season)
        {
            #region SQLstr
            //bool type = HttpContext.User.IsInRole("admin"); // BLR/RE/CTC/Admin
            // var val1= HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value；
            // object list = Repository.GetListByUser(UserId, Year, Season);
            // return JsonResult
            string wheresql = "";// and (AS_ID=@UserID OR ASM_ID=@UserID    OR  SD_ID=@UserID   OR  AH_ID=@UserID  OR  SVP_ID=@UserID)
            if (HttpContext.User.IsInRole("AS"))
            {
                wheresql = "   and AS_ID=@UserID ";
            }
            else if (HttpContext.User.IsInRole("ASM"))
            {
                wheresql = "   and   ASM_ID=@UserID ";
            }
            //else if (HttpContext.User.IsInRole("AH"))
            //{
            //    wheresql = "   and   AH_ID=@UserID  ";
            //}
            //else if (HttpContext.User.IsInRole("SVP"))
            //{
            //    wheresql = "   and  SVP_ID=@UserID";
            //}
            else if (HttpContext.User.IsInRole("SO"))//
            {
                wheresql = "  and  BRANCH_EN in (select branch from SIDE_PLM_BLR_Customer_group where ID in (select value from dbo.Side_Split((select Branch from SIDE_PLM_BLR_SalesOperation where SalesName in ( select [Name] from SIDE_PLM_V_Users where id=@UserID)),','))) ";
            }
            else if (HttpContext.User.IsInRole("admin") || HttpContext.User.IsInRole("BLR") || HttpContext.User.IsInRole("RE") || HttpContext.User.IsInRole("CTC"))
            {

            }

            string sqlstr = string.Format(@"	  select t.TM_SEASON,t.TM_Year,POS_CHANNEL as Channel ,t.CUSTOMER_GROUP as CUSTOMER,t.BRANCH_EN as Branch,isnull(Account_Sales,'') as ASName

,SUM(CASE WHEN PackagePOS='Y' then 1 else 0 end) MandatoryPOS
,SUM(CASE WHEN AssortmentPOS='Y' then 1 else 0 end) AssortmentPOS
,  SUM(CASE WHEN t.TM_SEASON ='Q1' AND t.BLRQ1 = 'Y' THEN 1 WHEN t.TM_SEASON = 'Q2' AND t.BLRQ2='Y' THEN 1
				                                            WHEN t.TM_SEASON = 'Q3' AND t.BLRQ3 = 'Y' THEN 1
				                                            WHEN t.TM_SEASON = 'Q4' AND t.BLRQ4='Y' THEN 1  ELSE 0 END) AS POSY,
															SUM(CASE WHEN t.TM_SEASON ='Q1' AND t.BLRQ1 = 'N' THEN 1 
				                                            WHEN t.TM_SEASON = 'Q2' AND t.BLRQ2='N' THEN 1
				                                            WHEN t.TM_SEASON = 'Q3' AND t.BLRQ3 = 'N' THEN 1
				                                            WHEN t.TM_SEASON = 'Q4' AND t.BLRQ4='N' THEN 1 
				                                            ELSE 0 END) AS POSN,
																SUM(CASE WHEN t.TM_SEASON ='Q1'   THEN 1 
				                                            WHEN t.TM_SEASON = 'Q2' THEN 1
				                                            WHEN t.TM_SEASON = 'Q3'  THEN 1
				                                            WHEN t.TM_SEASON = 'Q4'  THEN 1 
				                                            ELSE 0 END) AS POS,RePosState as PosState
 from [SIDE_vw_PLM_BLR_POS_Master_Overview] t  where t.TM_Year=@Year and t.TM_SEASON=@Season  and t.POS_Status='Y' {0}
 group by POS_CHANNEL,t.CUSTOMER_GROUP,t.BRANCH_EN ,Account_Sales,RePosState,t.TM_SEASON,t.TM_Year", wheresql);
            #endregion
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Year", Year, DbType.String, ParameterDirection.Input);
            parameters.Add("@Season", Season, DbType.String, ParameterDirection.Input);
            object list = Repository.GetListByUserRole(sqlstr, parameters);
            return Ok(list);
        }

        /// <summary>
        /// Get Single Entity
        /// </summary>
        /// <param name="TM_YEAR"></param>
        /// <param name="TM_SEASON"></param>
        /// <param name="POS_CODE"></param>
        /// <returns></returns>
        [HttpGet("GetSinglePOSDetail/{TM_YEAR}/{TM_SEASON}/{POS_CODE}")]
        public async Task<object> GetSingleEntity(string TM_YEAR, string TM_SEASON, string POS_CODE)
        {
            POS_Master entity = await Repository.GetSingelEntity(TM_YEAR, TM_SEASON, POS_CODE);
            return Ok(entity);
        }
        /// <summary>
        /// Post Entity
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task PostEntity(POS_Master entity)
        {
            await Repository.PostEntity(entity);
        }

        /// <summary>
        /// 快速添加Posmaster  根据STC_Core编号查找customer信息
        /// </summary>
        /// <returns></returns>     
        [HttpGet("NonMandatoryInsertPOS_MasterInfo")]
        public async Task<object> NonMandatoryInsertPOS_MasterInfo([FromQuery] string TM_YEAR, string TM_SEASON, string STC_Core, string POS_Status)
        {

            ResultModel result = new ResultModel();
            result.IsSuccess = false;
            try
            {
                if (string.IsNullOrEmpty(STC_Core))
                {
                    result.Message = "Ship to Code Core is Null Or Empty! ";
                    return Ok(result);
                }
                POS_Master entity = new POS_Master();//一些默认值
                entity.POS_CHANNEL = "Core";
                entity.TM_SEASON = TM_SEASON;
                entity.TM_YEAR = TM_YEAR;
                entity.STC_CORE = STC_Core;
                entity.STA_BULK_CORE = "";
                entity.POS_Status = POS_Status;
                entity.PackagePOS = "N";
                string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
                entity.Modified_User = _users.name;
                entity.Created_Date = DateTime.Now;

                //获取customerInfo
                CustomersInfo customersInfo = await CustomerRepository.GetSingleEntityByShipTo_Code(STC_Core);
                if(customersInfo==null)
                {
                    result.Message = "Ship to Code Core is Error![CustomersInfo]";
                    return Ok(result);
                }
                entity.CUSTOMER_GROUP = customersInfo.Customer_Group;
                entity.CUSTOMER_CODE = customersInfo.Customer_Code;
                entity.CUSTOMER_NAME = customersInfo.Customer_Name;
                entity.BRANCH_CN = customersInfo.branch_CN;
                entity.BRANCH_EN = customersInfo.branch_EN;
                entity.WHS_CHANNEL = customersInfo.WHS_Channel;
                entity.Product_Channel = customersInfo.Product_Channel;
               

                #region 生成Dummy Code  即POS_CODE
                string paraname ="NN"+ entity.TM_YEAR.Substring(2, 2) + entity.TM_SEASON + "-" + entity.CUSTOMER_GROUP + entity.BRANCH_EN + "-" + entity.POS_CHANNEL.Substring(0, 1);
                var maxindex = Repository.GetMaxPOSCode(paraname);
                if (!string.IsNullOrEmpty(maxindex.ToString()))
                {
                    int index = int.Parse(maxindex.ToString());
                    if (index > 0)
                    {
                        index++;
                        entity.POS_CODE = paraname + (index < 10 ? "0" + index : index + "");
                    }
                    else
                    {
                        entity.POS_CODE = paraname + "01";
                    }
                }
                else
                {
                    entity.POS_CODE = paraname + "01";
                }
                #endregion

                #region 验证 ship to Code
                if (!string.IsNullOrEmpty(entity.STC_CORE))
                {
                    entity.STC_CORE = entity.STC_CORE.ToUpper();

                }
                if (entity.STC_CORE != "TBD" && !string.IsNullOrEmpty(entity.STC_CORE))
                {
                    //  string Productchannel = Repository.GetProductchannel(entity.STC_CORE);

                    if ( entity.Product_Channel.ToLower() != "core")
                    {
                        result.Message = "Ship to Code(Core) is Error [Product_Channel]!";
                        return Ok(result);
                    }
                    int reuslt = Repository.ExistshipCode(entity.STC_CORE, 1);
                    if (reuslt > 0)
                    {
                        //ship to code 是否正确 
                        result.Message = "Ship to Code(Core) is exist!";
                        return Ok(result);
                    }

                }
                #endregion
                #region CURRENT_STATUS和 CURRENT_COMPSTATUS 和BLR 一些状态的默认值
                
                entity.BLRQ1 = "N";
                entity.COMPQ1 = "N";
                entity.BLRQ2 = "N";
                entity.COMPQ2 = "N";
                entity.BLRQ3 = "N";
                entity.COMPQ3 = "N";
                entity.BLRQ4 = "N";
                entity.COMPQ4 = "N";
                entity.CURRENT_STATUS = "N";
                entity.CURRENT_COMPSTATUS = "N";              

                #endregion
                await Repository.PostEntity(entity);
                result.Message = "ok";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {

                result.Message = "" + ex.Message;
            }

            return Ok(result);
        }

        /// <summary>
        /// Post Entity
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost("InsertPOS_MasterInfo")]
        public async Task<object> InsertPos_MasterInfo(POS_Master entity)
        {
            ResultModel result = new ResultModel();
            result.IsSuccess = false;

            try
            {
                string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
                entity.Modified_User = _users.name;
                entity.Created_Date = DateTime.Now;
                #region 生成Dummy Code  即POS_CODE
                //Dummy Code的生成规则：Dummy Year(2 digit sequential number)&Quarter+"-"+Customer Short Name+Branch+"-"+Channel short Name+2 digit sequential number
                string paraname = entity.TM_YEAR.Substring(2, 2) + entity.TM_SEASON + "-" + entity.CUSTOMER_GROUP + entity.BRANCH_EN + "-" + entity.POS_CHANNEL.Substring(0, 1);

                var maxindex = Repository.GetMaxPOSCode(paraname);
                if (!string.IsNullOrEmpty(maxindex.ToString()))
                {
                    int index = int.Parse(maxindex.ToString());
                    if (index > 0)
                    {
                        index++;
                        entity.POS_CODE = paraname + (index < 10 ? "0" + index : index + "");
                    }
                    else
                    {
                        entity.POS_CODE = paraname + "01";
                    }
                }
                else
                {
                    entity.POS_CODE = paraname + "01";
                }

                #endregion
                #region 验证 ship to Code
                if (!string.IsNullOrEmpty(entity.STC_CORE))
                {
                    entity.STC_CORE = entity.STC_CORE.ToUpper();
                    if (entity.STC_CORE != "TBD")
                    {
                        string Productchannel = Repository.GetProductchannel(entity.STC_CORE);
                        entity.Product_Channel = Productchannel;
                        if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToString().ToLower() != "core")
                        {
                            result.Message = "Ship to Code(Core) is Error!";
                            return Ok(result);
                        }
                        int reuslt = Repository.ExistshipCode(entity.STC_CORE, 1);
                        if (reuslt > 0)
                        {
                            //ship to code 是否正确 
                            result.Message = "Ship to Code(Core) is exist!";
                            return Ok(result);
                        }
                    }

                }
    
                if (!string.IsNullOrEmpty(entity.STC_OCS))
                {
                    entity.STC_OCS = entity.STC_OCS.ToUpper();
                    if (entity.STC_OCS != "TBD")
                    {
                        string Productchannel = Repository.GetProductchannel(entity.STC_OCS);
                        entity.Product_Channel = Productchannel;
                        if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "originals")
                        {
                            result.Message = "Ship to Code(Originals) is Error!";
                            return Ok(result);
                        }
                        int reuslt = Repository.ExistshipCode(entity.STC_OCS, 2);
                        if (reuslt > 0)
                        {
                            //ship to code 是否正确 
                            result.Message = "Ship to Code(Originals) is exist!";
                            return Ok(result);
                        }
                    }

                }
       
                if (!string.IsNullOrEmpty(entity.STC_NEO))
                {
                    entity.STC_NEO = entity.STC_NEO.ToUpper();
                    if (entity.STC_NEO != "TBD")
                    {
                        string Productchannel = Repository.GetProductchannel(entity.STC_NEO);
                        entity.Product_Channel = Productchannel;
                        if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "neo")
                        {
                            result.Message = "Ship to Code(Neo) is Error!";
                            return Ok(result);
                        }
                        int reuslt = Repository.ExistshipCode(entity.STC_NEO, 3);
                        if (reuslt > 0)
                        {
                            //ship to code 是否正确 
                            result.Message = "Ship to Code(Neo) is exist!";
                            return Ok(result);
                        }
                    }

                }
         
                if (!string.IsNullOrEmpty(entity.STC_KIDS))
                {
                    entity.STC_KIDS = entity.STC_KIDS.ToUpper();
                    if (entity.STC_KIDS != "TBD")
                    {
                        string Productchannel = Repository.GetProductchannel(entity.STC_KIDS);
                        entity.Product_Channel = Productchannel;
                        if (string.IsNullOrEmpty(Productchannel) || Productchannel.ToLower() != "kids")
                        {
                            result.Message = "Ship to Code(Kids) is Error!";
                            return Ok(result);
                        }
                        int reuslt = Repository.ExistshipCode(entity.STC_KIDS, 4);
                        if (reuslt > 0)
                        {
                            //ship to code 是否正确 
                            result.Message = "Ship to Code(Kids) is exist!";
                            return Ok(result);
                        }
                    }

                }
        

                #endregion

                #region CURRENT_STATUS和 CURRENT_COMPSTATUS       
             
                if (entity.TM_SEASON == "Q1")
                {
                    entity.CURRENT_STATUS = entity.BLRQ1;
                    entity.CURRENT_COMPSTATUS = entity.COMPQ1;
                }
                else if (entity.TM_SEASON == "Q2")
                {
                    entity.CURRENT_STATUS = entity.BLRQ2;
                    entity.CURRENT_COMPSTATUS = entity.COMPQ2;
                }
                else if (entity.TM_SEASON == "Q3")
                {
                    entity.CURRENT_STATUS = entity.BLRQ3;
                    entity.CURRENT_COMPSTATUS = entity.COMPQ3;
                }
                else if (entity.TM_SEASON == "Q4")
                {
                    entity.CURRENT_STATUS = entity.BLRQ4;
                    entity.CURRENT_COMPSTATUS = entity.COMPQ4;
                }

                #endregion
                await Repository.PostEntity(entity);
                result.Message = "ok";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {

                result.Message = "" + ex.Message;
            }

            return Ok(result);

        }
        /// <summary>
        /// Put Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task PutEntity(POS_Master entity)
        {
            entity.Modified_Date = DateTime.Now;
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            entity.Modified_User = _users.name;
            await Repository.PutEntity(entity);
        }
        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteEntity(int Id)
        {
            await Repository.DeleteEntity(Id);
        }
        [HttpGet("GetPreviousSeason/{year}/{season}/{pos_code}")]
        public async Task<OkObjectResult> GetPreviousSeason(string year, string season, string pos_code)
        {
            object obj = await Repository.GetPreviousSeason(year, season, pos_code);
            return Ok(obj);
        }
        /// <summary>
        /// 根据POSReview的状态判断BLR是否能编辑
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        [HttpGet("CheckPOSReviewBLRStatus/{userid}/{year}/{season}")]
        public int CheckPOSReviewBLRStatus(string userid,string year, string season)
        {           
           int obj =  Repository.CheckPOSReviewBLRStatus(int.Parse(userid), year, season);
            return obj;
        }

        [HttpGet("whetherKids")]
        public async Task<OkObjectResult> whetherKids()
        {
            //是否是Account Sales(Kids) 或者 Category Sales(Kids)
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles AS_Kids = _users.Roles.Where(t => t.RoleName == "Account Sales(Kids)").FirstOrDefault();
            Roles CS_Kids = _users.Roles.Where(t => t.RoleName == "Category Sales(Kids)").FirstOrDefault();
            bool result = false;
            if (AS_Kids != null || CS_Kids != null)
            {
                result = true;
            }
            return Ok(new { whetherApprove = result });
        }
        [HttpGet("whetherCategoryKids")]
        public async Task<OkObjectResult> whetherCategoryKids()
        {
            //是否是Account Sales(Kids) 或者 Category Sales(Kids)
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles CS_Kids = _users.Roles.Where(t => t.RoleName == "Category Sales(Kids)").FirstOrDefault();
            bool result = false;
            if (CS_Kids != null)
            {
                result = true;
            }
            return Ok(new { whetherApprove = result });
        }
        [HttpGet("whetherMB")]
        public async Task<OkObjectResult> whetherMB()
        {
            //是否是Account Sales(MB) 或者 Category Sales(MB)
            string userid = HttpContext.User.Claims.Where(item => item.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Users _users = await userRepository.GetUserAndRoles(int.Parse(userid));
            Roles AS_MB = _users.Roles.Where(t => t.RoleName == "Account Sales(MB)").FirstOrDefault();
            Roles CS_MB = _users.Roles.Where(t => t.RoleName == "Category Sales(MB)").FirstOrDefault();
            bool result = false;
            if (AS_MB != null || CS_MB != null)
            {
                result = true;
            }
            return Ok(new { whetherApprove = result });
        }
        
    }
}