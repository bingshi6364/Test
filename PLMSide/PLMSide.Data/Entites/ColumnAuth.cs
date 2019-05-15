using System;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data.Entites
{
    public class ColumnAuth
    {
        public string Excel_Column { get; set; }

        public string Database_Column { get; set; }

        public string CanEdit { get; set; }
    }
}
