using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entities
{
    public class BLR_POSReview
    {
        public int ID { get; set; }
        public string POSCode { get; set; }
        public string AccountSales { get; set; }
        public string AccountSalesName { get; set; }
        public DateTime AccountSalesCreateTime { get; set; }
        public string ASM_ID { get; set; }
        public string ASM_Name { get; set; }
        public DateTime ASM_ApproveTime { get; set; }
        public string Season { get; set; }
        public string Year { get; set; }
        public string PosState { get; set; }
    }
}
