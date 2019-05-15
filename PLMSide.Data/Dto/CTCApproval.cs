using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Dto
{
    public class CTCApproval: Pagination
    {
        public string Year { get; set; }

        public string Season { get; set; }

        public string Group { get; set; }

        public string Branch { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Channel { get; set; }

        public string Sellspacebegin { get; set; }

        public string Sellspaceend { get; set; }

        public string Range { get; set; }
    }
}
