using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using PLMSide.Common;
using PLMSide.Data.Entites;
using PLMSide.Data.Entities;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersInfoController : Controller
    {
        private readonly ICustomersInfoRepository Repository;
        private readonly IColumnAuthRepository ColumnAuthRepository;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IUserRepository userRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_repository"></param>
        /// <param name="_columnAuthRepository"></param>
        /// <param name="_hostingEnv"></param>
        /// <param name="_userRepository"></param>
        public CustomersInfoController(ICustomersInfoRepository _repository, IColumnAuthRepository _columnAuthRepository, IHostingEnvironment _hostingEnv, IUserRepository _userRepository)
        {
            Repository = _repository;
            ColumnAuthRepository = _columnAuthRepository;
            hostingEnv = _hostingEnv;
            userRepository = _userRepository;
        }
        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<object> GetEntities(string code, string name, string group, string whs_channel,string Account_Sales, int currentPageIndex = 1)
        {
            string whereStr = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                whereStr = string.Format("Customer_Code like '%{0}%'", code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" Customer_Name like '%{0}%'", name);
            }
            if (!string.IsNullOrEmpty(group))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" Customer_Group like '%{0}%'", group);
            }

            if (!string.IsNullOrEmpty(whs_channel))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" WHS_Channel like '%{0}%'", whs_channel);
            }

            if (!string.IsNullOrEmpty(Account_Sales))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" Account_Sales like '%{0}%'", Account_Sales);
            }

            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = "1=1";
            }
            Tuple<List<CustomersInfo>, int> tp = await Repository.GetEntities(whereStr, currentPageIndex);
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(new { pageTotal = tp.Item2 }));
            return Ok(tp.Item1);
        }
        /// <summary>
        /// get single entity
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<object> GetSingleEntity(int Id)
        {
            CustomersInfo entity = await Repository.GetSingelEntity(Id);
            return Ok(entity);
        }
        /// <summary>
        /// 获取所有AccountSales角色的所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAccountSalesByRole/{channle}")]
        public async Task<object> GetAccountSalesByRole()
        {
            List<Users> users_accountSales = await Repository.GetAccountSalesByRole();
            return Ok(users_accountSales);
        }

        /// <summary>
        /// get single entity 
        ///根据[SIDE_PLM_STA] 的ID查找customer信息 Customer =Y
        /// </summary>
        /// <param name="ShipTo_Code"></param>
        /// <returns></returns>
        [HttpGet("GetSingleEntityByShipTo_Code/{ShipTo_Code}")]
        public async Task<object> GetSingleEntityByShipTo_Code(string ShipTo_Code)
        {            
            return Ok(await Repository.GetSingleEntityByShipTo_Code(ShipTo_Code));
        }
       

        /// <summary>
        /// get single entity
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetSingleEntityByCustomer_Code/{Customer_Code}")]
        public async Task<object> GetSingleEntityByCustomer_Code(string Customer_Code)
        {
            CustomersInfo entity = await Repository.GetSingleEntityByCustomer_Code(Customer_Code);
            return Ok(entity);
        }
        /// <summary>
        /// Post Entity
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task PostEntity(CustomersInfo entity)
        {
            await Repository.PostEntity(entity);
        }
        /// <summary>
        /// Put Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task PutEntity(CustomersInfo entity)
        {
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

        [HttpPost("Export")]
        public async Task<object> Export(RequestBody body)
        {
            string code=body.code;
            string name=body.name;
            string group=body.group;
            string whs_channel=body.whs_channel;
            string roleid = HttpContext.User.Claims.Where(item => item.Type == "RoleID").FirstOrDefault().Value;
            List<ColumnAuth> listColumn = await ColumnAuthRepository.GetColumnAuths(roleid, "CustomersInfo");
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
            if (!string.IsNullOrEmpty(code))
            {
                whereStr = string.Format("Customer_Code like '%{0}%'", code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" Customer_Name like '%{0}%'", name);
            }
            if (!string.IsNullOrEmpty(group))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" Customer_Group like '%{0}%'", group);
            }

            if (!string.IsNullOrEmpty(whs_channel))
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr += " and ";
                }
                whereStr += string.Format(" WHS_Channel like '%{0}%'", whs_channel);
            }

            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = "1=1";
            }
            List<CustomersInfo> list = await Repository.GetEntities(whereStr);
            #endregion
            var fs = ExcelHelper.GetByteToExportExcel<CustomersInfo>(list, columnMap, new List<string>(), NoEdit, "Sheet1");

            return File(fs, "application/octet-stream", $"excel导出测试{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        [HttpPost("import")]
        public ActionResult Import([FromForm(Name = "file")] IFormFile f1)
        {
            string filename = ContentDispositionHeaderValue
                                .Parse(f1.ContentDisposition)
                                .FileName
                                .Trim('"');

            Dictionary<string, string> dic = new Dictionary<string, string>();

            //IE 
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            string filepath = hostingEnv.ContentRootPath + @"\Upload";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filename = filepath + $@"\{filename}";
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            using (FileStream fs = System.IO.File.Create(filename))
            {
                f1.CopyTo(fs);
                fs.Flush();
            }
            List<CustomersInfo> list;
            List<string> errorList = new List<string>();
            try
            {
                list = GetSheetValues(filename, userRepository, ref errorList);
                ModifyDataBase(list, Repository);
            }
            catch (Exception)
            {

                throw;
            }
           
            return Ok(new { count = filepath, filename = filename, errorList });
        }

        public static List<CustomersInfo> GetSheetValues(string filepath, IUserRepository user_repository, ref List<string> errorList)
        {
            FileInfo file = new FileInfo(filepath);
            if (file != null)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    //获取表格的列数和行数
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    var Customers = new List<CustomersInfo>();
                    Users _user;
                    for (int row = 1; row <= rowCount; row++)
                    {
                        if (row == 1)
                        {
                            continue;
                        }
                        CustomersInfo customer = new CustomersInfo();
                        if (string.IsNullOrEmpty(valueOf(worksheet.Cells[row, 1].Value)))
                        {
                            errorList.Add($"第{row}行缺少Customer_Code,此条数据未导入");
                            continue;
                        }
                        if (string.IsNullOrEmpty(valueOf(worksheet.Cells[row, 2].Value)))
                        {
                            errorList.Add($"第{row}行缺少Customer_Name,此条数据未导入");
                            continue;
                        }
                        customer.Customer_Code = valueOf(worksheet.Cells[row, 1].Value);
                        customer.Customer_Name = valueOf(worksheet.Cells[row, 2].Value);
                        customer.Customer_Group = valueOf(worksheet.Cells[row, 3].Value);
                        customer.branch_EN = valueOf(worksheet.Cells[row, 4].Value);
                        customer.branch_CN = valueOf(worksheet.Cells[row, 5].Value);
                        customer.WHS_Channel = valueOf(worksheet.Cells[row, 6].Value);

                        _user = user_repository.GetUserByEmail(valueOf(worksheet.Cells[row, 7].Value));
                        customer.Account_Sales = _user == null ? "" : _user.name;
                        customer.Account_SalesID = _user == null ? "" : _user.ID.ToString();

                        _user = user_repository.GetUserByEmail(valueOf(worksheet.Cells[row, 8].Value));
                        customer.Category_Sales = _user == null ? "" : _user.name;
                        customer.Category_SalesID = _user == null ? "" : _user.ID.ToString();

                        customer.Status = valueOf(worksheet.Cells[row, 9].Value);

                        _user = user_repository.GetUserByEmail(valueOf(worksheet.Cells[row, 10].Value));
                        customer.kids_account_sales = _user == null ? "" : _user.name;
                        customer.kids_account_salesID = _user == null ? "" : _user.name;
                        customer.Product_Channel = valueOf(worksheet.Cells[row, 11].Value);
                        Customers.Add(customer);
                    }
                    return Customers;
                }
            }
            return null;
        }
        public static string valueOf(object obj)
        {
            return (obj == null) ? "" : obj.ToString();
        }
        public static void ModifyDataBase(List<CustomersInfo> customersInfo, ICustomersInfoRepository cutomer_repository)
        {
            foreach (var item in customersInfo)
            {
                try
                {
                    cutomer_repository.PutEntityByCode(item);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
    }
    public class RequestBody {
        public string code { get; set; }
        public string name { get; set; }
        public string group { get; set; }
        public string whs_channel { get; set; }
    }
}