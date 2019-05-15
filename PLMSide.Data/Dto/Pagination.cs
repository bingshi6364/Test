using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Dto
{
    public class Pagination
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string OrderString { get;set; }

    }
}
