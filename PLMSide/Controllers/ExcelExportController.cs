using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PLMSide.Data.Entites;
using PLMSide.Data.IRepository;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        private IColumnAuthRepository _IColumnAuthRepository;
        private IHostingEnvironment _hostingEnv;
        public ExcelExportController(IColumnAuthRepository IColumnAuthRepository, IHostingEnvironment hostingEnv)
        {
            _IColumnAuthRepository = IColumnAuthRepository;
            _hostingEnv = hostingEnv;
        }

        //[HttpGet]
        //[Route("{type}")]
        //public async Task<object> ExcelExport(string type)
        //{
        //    List<ColumnAuth> list = await _IColumnAuthRepository.GetColumnAuths("14",type);
        //    var NoEdit = new List<int>();
        //    var columnMap = new Dictionary<string, string>();
        //    StringBuilder selectcolumn = new StringBuilder();
        //    for (int i=0;i<list.Count();i++)
        //    {
        //        selectcolumn.Append(list[i].Database_Column + ",");
        //        columnMap.Add(list[i].Database_Column.ToString(), list[i].Excel_Column.ToString());
        //        if (list[i].CanEdit.ToUpper()=="N")
        //        {
        //            NoEdit.Add(i+1);
        //        }
        //    }
        //    string a = "POS_CHANNEL";
        //    string select = selectcolumn.ToString();
        //    List<POS_Master> list1 = 

        //    var fs = GetByteToExportExcel<POS_Master>(list1, columnMap, new List<string>(), NoEdit, "Sheet1");
        //    return File(fs, "application/octet-stream", $"excel导出测试{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        //}

        private static ExcelPackage CreateExcelPackage<T>(List<T> datas, Dictionary<string, string> columnNames, List<string> outOfColumns,List<int> NoEditColumnsIndex, string sheetName = "Sheet1")
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var titleRow = 0;
            //worksheet.Cells["F:F"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //worksheet.Cells["F:F"].Style.Fill.BackgroundColor.SetColor(Color.Red);
            foreach(var index in NoEditColumnsIndex)
            {
                string Cells = $"{ConvertToExcelIndex(index)}:{ConvertToExcelIndex(index)}";
                worksheet.Cells[Cells].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[Cells].Style.Fill.BackgroundColor.SetColor(Color.Red);
            }
            //获取要反射的属性,加载首行
            Type myType = typeof(T);
            List<PropertyInfo> myPro = new List<PropertyInfo>();
            int i = 1;
            foreach (string key in columnNames.Keys)
            {
                PropertyInfo p = myType.GetProperty(key);
                myPro.Add(p);

                worksheet.Cells[1 + titleRow, i].Value = columnNames[key];
                i++;
            }

            int row = 2 + titleRow;
            foreach (T data in datas)
            {
                int column = 1;
                foreach (PropertyInfo p in myPro.Where(info => !outOfColumns.Contains(info.Name)))
                {
                    worksheet.Cells[row, column].Value = p == null ? "" : Convert.ToString(p.GetValue(data, null));
                    column++;
                }
                row++;
            }
       
            return package;
        }

        public static Byte[] GetByteToExportExcel<T>(List<T> datas, Dictionary<string, string> columnNames, List<string> outOfColumn, List<int> NoEditColumnIndex, string sheetName = "Sheet1")
        {
            using (var fs = new MemoryStream())
            {
                using (var package = CreateExcelPackage<T>(datas, columnNames, outOfColumn, NoEditColumnIndex, sheetName))
                {
                    package.SaveAs(fs);
                    return fs.ToArray();
                }
            }
        }

        public static string ConvertToExcelIndex(int index)
        {
            if (index <= 0) return string.Empty;
            string valueToRetrun = string.Empty;
            if (index < 27) return valueToRetrun + GetChar(index);

            int modRst = index % 26; //26个英文字母
            if (modRst == 0) //是26的倍数，十位特殊处理，个位固定	
                return valueToRetrun + GetChar(index / 26 - 1) + 'Z';
            return valueToRetrun + GetChar(index / 26) + GetChar(index % 26);
        }

        private static char GetChar(int num)
        {
            if (num > 26 || num < 0) throw new Exception("error");
            if (num == 0) return 'Z';
            return (char)(num + 64);
        }
    }
}



