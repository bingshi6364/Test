using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Common;
using PLMSide.Data.Dto;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;
using PLMSide.Data.Repository;
using PLMSide.Filter;
using PLMSide.Helper;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AS,ASM,SO,admin")]
    public class SOApprovalController : ControllerBase
    {
        private readonly ISOApprovalRepository soapprovalRepository;
        private readonly IColumnAuthRepository columnAuthRepository;
        private readonly ICustomerGroupRepository customergroupRepository;
        private readonly ICTCApprovalRepository ctcsoapprovalRepository;
        private IHostingEnvironment hostingEnv;
        public SOApprovalController(ISOApprovalRepository _soapprovalRepository, IColumnAuthRepository _columnAuthRepository, ICustomerGroupRepository _customergroupRepository, ICTCApprovalRepository _ctcsoapprovalRepository, IHostingEnvironment _hostingEnv)
        {
            soapprovalRepository = _soapprovalRepository;
            columnAuthRepository = _columnAuthRepository;
            customergroupRepository = _customergroupRepository;
            ctcsoapprovalRepository = _ctcsoapprovalRepository;
            hostingEnv = _hostingEnv;
        }

        [HttpGet("GetCustomer")]
        public async Task<object> GetCustomerByGroup(string group)
        {
            //List<object> Groups = await customergroupRepository.GetCustomerGroupByGroup(group);
            //return Ok(Groups);
            List<CustomerGroup> list = await customergroupRepository.GetCustomerGroups();
            var customerilist = list.Where(item => item.Group == group).Select(item => item.Customer).Distinct();
            return Ok(customerilist);
        }

        [HttpGet("GetBranch")]
        public async Task<object> GetBranchByCustomer_group(string customer)
        {
            List<CustomerGroup> list = await customergroupRepository.GetCustomerGroups();
            var branchlist = list.Where(item=>item.Customer==customer).Select(item => item.Branch).Distinct();
            return Ok(branchlist);
        }


        [HttpGet("GetGroup")]
        public async Task<object> GetCustomerGroup()
        {
            List<CustomerGroup> list = await customergroupRepository.GetCustomerGroups();
            var grouplist = list.Select(item => item.Group).Distinct();
            return Ok(grouplist);
        }

        [HttpGet("GetAllCustomer")]
        public async Task<object> GetAllCustomer()
        {
            List<CustomerGroup> list = await customergroupRepository.GetCustomerGroups();
            var grouplist = list.Select(item => item.Customer).Distinct();
            return Ok(grouplist);
        }

        [HttpGet("Provinces")]
        public async Task<object> GetProvinces()
        {
            List<Province> list = await soapprovalRepository.GetProvinces();
            return Ok(list);
        }

        [HttpGet("Channel")]
        public async Task<object> GetChannels()
        {
            var list = await soapprovalRepository.GetChannels();
            return Ok(list);
        }



        [HttpGet("PosMaster")]
        [AntiSqlInjectAttribute]
        public async Task<object> GetPageByProcList([FromQuery] string year, string season,string group, string customer, string branch,
            string poscode, string format, string channel, string city, string posname, string province,string blr,string kids
            , int pageIndex)
        {
            string column = @"POS_CODE,POS_NAME,CUSTOMER_GROUP,BRANCH_EN,POS_CHANNEL,SUB_CHANNEL,CURRENT_STATUS,
            CURRENT_COMPSTATUS,BIG_FORMAT";
            
            StringBuilder sql = new StringBuilder(" 1=1");
            Tuple<List<POS_Master>, int> tp = new Tuple<List<POS_Master>, int>(new List<POS_Master>(), 0);
            string id = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            if (HttpContext.User.IsInRole("AS"))
            {
                sql.Append($" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where Account_SalesID={id})");
            }
            else if (HttpContext.User.IsInRole("ASM"))
            {
                sql.Append($" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where Account_SalesID IN (SELECT AS_ID FROM SIDE_PLM_BLR_Approve_Process WHERE ASM_ID={id}))");
            }
            else if (HttpContext.User.IsInRole("SO"))
            {
                sql.Append(@"AND BRANCH_EN IN(select distinct [Branch] from SIDE_PLM_BLR_Customer_Group where ID in(
                           select substring(Branch+',',number,charindex(',',Branch+',',number)-number)
                           from SIDE_PLM_BLR_SalesOperation ,master..spt_values s
                           where type='p' and number>0 and substring(','+Branch,number,1)=','" +
                           $"and SalesName='{HttpContext.User.Identity.Name}'))");
            }
            else if(roleid.Contains('4'))
            {
                sql.Append($" AND CUSTOMER_CODE IN (select Customer_Code from SIDE_PLM_BLR_CustomersInfo where kids_account_salesID={id})");
            }
            //else if (HttpContext.User.IsInRole("admin") || HttpContext.User.IsInRole("BLR") || HttpContext.User.IsInRole("RE") || HttpContext.User.IsInRole("CTC"))
            //{

            //}

            sql.Append(" AND TM_YEAR ='" + year + "'");
            sql.Append(" AND TM_SEASON ='" + season + "'");

            if (!string.IsNullOrEmpty(group))
                sql.Append(" AND WHS_CHANNEL ='" + customer + "'");

            if (!string.IsNullOrEmpty(customer))
                sql.Append(" AND CUSTOMER_GROUP ='" + customer + "'");

            if (!string.IsNullOrEmpty(branch))
                sql.Append(" AND BRANCH_EN ='" + branch + "'");

            if (!string.IsNullOrEmpty(poscode))
                sql.Append(" AND POS_CODE LIKE '%" + poscode + "%'");

            if (!string.IsNullOrEmpty(format))
                sql.Append(" AND BIG_FORMAT LIKE '%" + format + "%'");

            if (!string.IsNullOrEmpty(channel))
                sql.Append("  AND POS_CHANNEL ='" + channel + "' ");

            if (!string.IsNullOrEmpty(city))
                sql.Append(" AND City_CN LIKE'%" + city + "%'");

            if (!string.IsNullOrEmpty(posname))
                sql.Append(" AND POS_NAME LIKE '%" + posname + "%'");

            if (!string.IsNullOrEmpty(province))
                sql.Append(" AND PROVINCE_CN  like '%" + province + "%'");

            if (!string.IsNullOrEmpty(blr))
                sql.Append("  AND CURRENT_STATUS ='" + blr + "' ");

            if (!string.IsNullOrEmpty(kids))
                sql.Append("  AND Kids_in_Large_Store ='" + kids + "' ");

            tp = await soapprovalRepository.GetPageByProcList("SIDE_PLM_V_POS_Master", column, sql.ToString(), "ID", pageIndex, 20);

            var meta = new
            {
                pageTotal = tp.Item2
            };
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
            return Ok(tp.Item1.Select(posmaster => new
            {
                POS_CODE= posmaster.POS_CODE,
                POS_NAME=posmaster.POS_NAME,
                CUSTOMER_GROUP=posmaster.CUSTOMER_GROUP,
                BRANCH_EN=posmaster.BRANCH_EN,
                POS_CHANNEL=posmaster.POS_CHANNEL,
                SUB_CHANNEL=posmaster.SUB_CHANNEL,
                CURRENT_STATUS=posmaster.CURRENT_STATUS,
                CURRENT_COMPSTATUS=posmaster.CURRENT_COMPSTATUS,
                BIG_FORMAT=posmaster.BIG_FORMAT
            }));
        }

        [HttpPost("Export")]
        [AntiSqlInjectAttribute]
        public async Task<object> GetExportList([FromBody] SOApproval Model
            )
        {
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await columnAuthRepository.GetColumnAuths(roleid, "SOApproval");
            var NoEdit = new List<int>();
            var columnMap = new Dictionary<string, string>();
            for (int i = 0; i < listColumn.Count(); i++)
            {
                columnMap.Add(listColumn[i].Database_Column.ToString(), listColumn[i].Excel_Column.ToString());
                if (listColumn[i].CanEdit.ToUpper() == "N")
                {
                    NoEdit.Add(i + 1);
                }
            }             
            string userid = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            string userName = HttpContext.User.Identity.Name;
            string roleName= HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.Role).FirstOrDefault().Value;
            List<POS_Master> list = await soapprovalRepository.GetExportList(Model, roleName, userid, userName, roleid);

            Response.Headers.Add("Access-Control-Expose-Headers", "Filename");
            Response.Headers.Add("Filename", "SOApproval.xlsx");

            var fs = ExcelHelper.GetByteToExportExcel<POS_Master>(list, columnMap, new List<string>(), NoEdit, "Sheet1");

            return File(fs, "application/octet-stream", $"SOApproval{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

        }

        [HttpGet("CodeUpdate")]
        public async Task<object> CodeUpdate([FromQuery] string year,string season,string dummycode,string poscode)
        {
            string checkResult = await soapprovalRepository.CheckPosCode(year, season, dummycode.Trim(), poscode.Trim());
            if (!string.IsNullOrEmpty(checkResult))
            {
                return Ok(new { result = "Fail", message = checkResult });
            }


            return Ok();
        }

        [HttpPost("ExportSTC")]
        [AntiSqlInjectAttribute]
        public async Task<object> GetSTCExportList([FromBody] SOApproval Model
            )
        {
            string roleid= HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            string roleName = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.Role).FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await columnAuthRepository.GetColumnAuthsByName(roleName, "STC");
            var NoEdit = new List<int>();
            var columnMap = new Dictionary<string, string>();
            for (int i = 0; i < listColumn.Count(); i++)
            {
                columnMap.Add(listColumn[i].Database_Column.ToString(), listColumn[i].Excel_Column.ToString());
                if (listColumn[i].CanEdit.ToUpper() == "N")
                {
                    NoEdit.Add(i + 1);
                }
            }
            string userid = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            string userName = HttpContext.User.Identity.Name;
            //string roleName = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.Role).FirstOrDefault().Value;

            List<POS_Master> list = await soapprovalRepository.GetExportList(Model, roleName, userid, userName,roleid);

            Response.Headers.Add("Access-Control-Expose-Headers", "Filename");
            Response.Headers.Add("Filename", "STC" +
                ".xlsx");

            var fs = ExcelHelper.GetByteToExportExcel<POS_Master>(list, columnMap, new List<string>(), NoEdit, "Sheet1");

            return File(fs, "application/octet-stream", $"SOApproval{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

        }

        [HttpPost("importSTC")]
        public async Task<object> ImportSTC([FromForm(Name = "file")] IFormFile f1, string year, string season)
        // public async Task<object> Import(IFormFile f1)
        {
            string filename = ContentDispositionHeaderValue
                                .Parse(f1.ContentDisposition)
                                .FileName
                                .Trim('"');

            Dictionary<string, string> dic = new Dictionary<string, string>();
            string roleName = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.Role).FirstOrDefault().Value;
            string UserID = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await columnAuthRepository.GetColumnAuthsByName(roleName, "STC");
            var NoEdit = new List<int>();
            var columnMap = new Dictionary<string, string>();
            var columnMapExport = new Dictionary<string, string>();
            List<string> colString = new List<string>();
            for (int i = 0; i < listColumn.Count(); i++)
            {
                columnMap.Add(listColumn[i].Excel_Column.ToString(), listColumn[i].Database_Column.ToString());
                columnMapExport.Add(listColumn[i].Database_Column.ToString(), listColumn[i].Excel_Column.ToString());
                colString.Add(listColumn[i].Database_Column.ToString());
                if (listColumn[i].CanEdit.ToUpper() == "N")
                {
                    NoEdit.Add(i + 1);
                }
            }
            colString.Add("UserID");
            colString.Add("UploadDate");

            ImportHelper.ImportFile(f1, hostingEnv.ContentRootPath + @"\Upload");
            filename = hostingEnv.ContentRootPath + @"\Upload" + $@"\{filename}";
            List<POS_MasterImport> pos =ExcelHelper.GetSheetValues<POS_MasterImport>(filename, columnMap, UserID);

             List<POS_MasterImport> result = await ctcsoapprovalRepository.ImportData(pos, colString, "SIDE_PLM_STCImport");
            if (result.Count() > 0)
            {
                columnMapExport.Add("Error_Message", "ERROR MESSAGE");
                var fs = ExcelHelper.GetByteToExportExcel<POS_MasterImport>(result, columnMapExport, new List<string>(), NoEdit, "Sheet1");
                Response.Headers.Add("Access-Control-Expose-Headers", "Filename");
                Response.Headers.Add("Filename", "ErrorReport.xlsx");
                return File(fs, "application/octet-stream", $"excel{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
            else
            {
                return Ok(new { Result = "success" });
            }


        }


    }
}