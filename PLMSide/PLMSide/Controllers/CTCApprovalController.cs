using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using PLMSide.Common;
using PLMSide.Data.Dto;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;
using PLMSide.Helper;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTCApprovalController : ControllerBase
    {
        private readonly ISOApprovalRepository _soapprovalRepository;
        private readonly ICTCApprovalRepository _ctcapprovalRepository;
        private IColumnAuthRepository _IColumnAuthRepository;
        private IHostingEnvironment _hostingEnv;

        public CTCApprovalController(ISOApprovalRepository soapprovalRepository, ICTCApprovalRepository ctcapprovalRepository, IColumnAuthRepository IColumnAuthRepository, IHostingEnvironment hostingEnv)
        {
            _soapprovalRepository = soapprovalRepository;
            _ctcapprovalRepository = ctcapprovalRepository;
            _IColumnAuthRepository = IColumnAuthRepository;
            _hostingEnv = hostingEnv;
        }


        [HttpGet("Channels")]
        public async Task<object> GetChannels()
        {
            List<string> list = await _soapprovalRepository.GetChannels();
            return Ok(list);
        }

        [HttpGet("CustomerBranch")]
        public async Task<object> GetCustomerBranch()
        {
            List<CustomerGroup> list = await _soapprovalRepository.GetCustomerBranch();
            return Ok(list);
        }

        [HttpGet("Provinces")]
        public async Task<object> GetProvinces()
        {
            List<Province> list = await _soapprovalRepository.GetProvinces();
            return Ok(list);
        }

        [HttpPost]
        public async Task<object> GetExportList([FromBody] CTCApproval Model)
        {
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await _IColumnAuthRepository.GetColumnAuths(roleid, "CTCApproval");
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
            List<POS_Master> list = await _ctcapprovalRepository.GetExportList(Model);

            var fs = ExcelHelper.GetByteToExportExcel<POS_Master>(list, columnMap, new List<string>(), NoEdit, "Sheet1");
            Response.Headers.Add("Access-Control-Expose-Headers", "Filename");
            Response.Headers.Add("Filename", "PosType.xlsx");
            return File(fs, "application/octet-stream", $"excel导出测试{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        [HttpPost("Page")]
        public async Task<object> GetPageByProcList([FromBody] CTCApproval Model)
        {
            Tuple<List<POS_Master>,int> tp = await _ctcapprovalRepository.GetPageByProcList(Model);
            var meta = new
            {
                pageTotal = tp.Item2
            };
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));
            return Ok(tp.Item1.Select(posmaster => new
            {
                POS_CODE = posmaster.POS_CODE,
                POS_NAME = posmaster.POS_NAME,
                CUSTOMER_GROUP = posmaster.CUSTOMER_GROUP,
                BRANCH_EN = posmaster.BRANCH_EN,
                POS_CHANNEL = posmaster.POS_CHANNEL,
                SEGMENTATION= posmaster.Seg_Marketplace_recommended,
                OCS_GRADING= posmaster.OCS_Grading_recommended,
                POS_TYPE= posmaster.POS_Type_recommended
            }
            ));
        }

        [HttpPost("import")]
        public async Task<object> Import([FromForm(Name = "file")] IFormFile f1,string year,string season)
       // public async Task<object> Import(IFormFile f1)
        {
                string filename = ContentDispositionHeaderValue
                                    .Parse(f1.ContentDisposition)
                                    .FileName
                                    .Trim('"');
            
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            string UserID = HttpContext.User.Claims.Where(item => item.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await _IColumnAuthRepository.GetColumnAuths(roleid, "CTCApproval");
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
            ImportHelper.ImportFile(f1, _hostingEnv.ContentRootPath + @"\Upload");
            filename = _hostingEnv.ContentRootPath + @"\Upload" + $@"\{filename}";
            //List<POS_MasterImport> pos = GetSheetValues(filename, columnMap,"123");
            List<POS_MasterImport> pos = ExcelHelper.GetSheetValues<POS_MasterImport>(filename, columnMap, UserID);
            List<POS_MasterImport> result = await _ctcapprovalRepository.ImportData(pos, colString, "SIDE_PLM_GradingImport");
            if(result.Count()>0)
            {
                columnMapExport.Add("Error_Message", "ERROR MESSAGE");
                var fs = ExcelHelper.GetByteToExportExcel<POS_MasterImport>(result, columnMapExport, new List<string>(), NoEdit, "Sheet1");
                Response.Headers.Add("Access-Control-Expose-Headers", "Filename");
                Response.Headers.Add("Filename", "ErrorReport.xlsx");
                return File(fs, "application/octet-stream", $"excel{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
            else {
                return Ok(new { Result = "success"});
            }

          
        }

       
    }

}