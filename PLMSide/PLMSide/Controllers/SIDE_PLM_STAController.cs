using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLMSide.Common;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SIDE_PLM_STAController : Controller
    {
        private ISIDE_PLM_STARepository repository;
        private readonly IColumnAuthRepository ColumnAuthRepository;
        public SIDE_PLM_STAController(ISIDE_PLM_STARepository _SIDE_PLM_STARepository, IColumnAuthRepository _columnAuthRepository)
        {
            repository = _SIDE_PLM_STARepository;
            ColumnAuthRepository = _columnAuthRepository;
        }

        [HttpGet]
        public async Task<object> GetEntitiesByPaging(string CustomerCode, string createData_Start, string createData_End, int currentPageIndex = 1) {
            string whereStr = string.Empty;
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("Customer_Code like '%{0}%'", CustomerCode);
            }
            //开始时间不为空 结束时间为空
            if (!string.IsNullOrEmpty(createData_Start) && string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate>= '{0}'", createData_Start);
            }
            //开始时间为空 结束时间不为空
            if (string.IsNullOrEmpty(createData_Start) && !string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate<= '{0}'", createData_End);
            }
            //开始时间不为空 结束时间不为空
            if (!string.IsNullOrEmpty(createData_Start) && !string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate>= '{0}' and CreateDate<= '{1}'", createData_Start, createData_End);
            }
            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = "1=1";
            }
            Tuple<List<SIDE_PLM_STA>, int> tp = await repository.GetEntitiesByPaging(whereStr, currentPageIndex);
            foreach (var item in tp.Item1)
            {
                item.CreateDate=DateTime.Parse(item.CreateDate).ToString("yyyy-MM-dd");
            } 
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }

        [HttpPost("Export")]
        public async Task<object> Export(STA_RequestBody body)
        {
            string CustomerCode = body.CustomerCode;
            string createData_Start = body.createData_Start;
            string createData_End = body.createData_End;
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await ColumnAuthRepository.GetColumnAuths(roleid, "SIDE_PLM_STA");
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
            #region 获取导出数据
            string whereStr = string.Empty;
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("Customer_Code like '%{0}%'", CustomerCode);
            }
            //开始时间不为空 结束时间为空
            if (!string.IsNullOrEmpty(createData_Start) && string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate>= '{0}'", createData_Start);
            }
            //开始时间为空 结束时间不为空
            if (string.IsNullOrEmpty(createData_Start) && !string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate<= '{0}'", createData_End);
            }
            //开始时间不为空 结束时间不为空
            if (!string.IsNullOrEmpty(createData_Start) && !string.IsNullOrEmpty(createData_End))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr = string.Format("CreateDate>= '{0}' and CreateDate<= '{1}'", createData_Start, createData_End);
            }
            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = "1=1";
            }
            List<SIDE_PLM_STA> list = await repository.GetEntities(whereStr);
            foreach (var item in list)
            {
                item.CreateDate= DateTime.Parse(item.CreateDate).ToString("yyyy-MM-dd");
            }
            #endregion
            var fs = ExcelHelper.GetByteToExportExcel<SIDE_PLM_STA>(list, columnMap, new List<string>(), NoEdit, "Sheet1");

            return File(fs, "application/octet-stream", $"SIDE_PLM_STA{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }
    }
    public class STA_RequestBody
    {
        public string CustomerCode { get; set; }
        public string createData_Start { get; set; }
        public string createData_End { get; set; }
    }
}