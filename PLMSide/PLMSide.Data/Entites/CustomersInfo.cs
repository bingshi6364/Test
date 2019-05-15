using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entites
{
    public class CustomersInfo
    {
        public int Id { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Group { get; set; }
        public string branch_EN { get; set; }
        public string branch_CN { get; set; }
        public string WHS_Channel { get; set; }
        public string Account_Sales { get; set; }
        public string Account_SalesID { get; set; }
        public string Category_Sales { get; set; }
        public string Category_SalesID { get; set; }
        public string kids_account_sales { get; set; }
        public string kids_account_salesID { get; set; }
        public string Product_Channel { get; set; }
        public string Status { get; set; }
    }
}
