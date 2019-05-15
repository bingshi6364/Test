using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace PLMSide.Common
{
    public static class ExcelHelper
    {
        private static ExcelPackage CreateExcelPackage<T>(List<T> datas, Dictionary<string, string> columnNames, List<string> outOfColumns, List<int> NoEditColumnsIndex, string sheetName = "Sheet1")
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var titleRow = 0;
            //worksheet.Cells["F:F"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //worksheet.Cells["F:F"].Style.Fill.BackgroundColor.SetColor(Color.Red);
            foreach (var index in NoEditColumnsIndex)
            {
                string Cells = $"{ConvertToExcelIndex(index)}:{ConvertToExcelIndex(index)}";
                worksheet.Cells[Cells].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[Cells].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
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

        public static List<T> GetSheetValues<T>(string filepath, Dictionary<string, string> dic, string UserID)  
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
                    var persons = new List<T>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Type t = typeof(T);
                        T pos = System.Activator.CreateInstance<T>();
                        for (int col = 1; col <= ColCount; col++)
                        {
                            string columnName = worksheet.Cells[1, col].Value.ToString();
                            string fieldName = "";
                            dic.TryGetValue(columnName, out fieldName);
                            var pro = t.GetProperty(fieldName);
                            if (pro != null)
                            {
                                t.GetProperty(fieldName).SetValue(pos, worksheet.Cells[row, col].Value == null ? "" : worksheet.Cells[row, col].Value.ToString());
                            }

                        }
                        t.GetProperty("UserID").SetValue(pos, UserID);
                        t.GetProperty("UploadDate").SetValue(pos, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //pos.UserID = UserID;
                        //pos.UploadDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                        persons.Add((T)pos);
                    }
                    return persons;
                }
            }
            return null;
        }
    }
}
