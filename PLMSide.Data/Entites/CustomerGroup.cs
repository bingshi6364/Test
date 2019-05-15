using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entites
{
    public class CustomerGroup
    {
        public int ID { get; set; }

        public string Group { get; set; }
        
        public string Customer { get; set; }

        public string Branch { get; set; }
        public string BranchCN { get; set; }
    }
}
