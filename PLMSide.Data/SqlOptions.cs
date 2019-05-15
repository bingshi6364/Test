using PLMSide.Common;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;

namespace PLMSide.Data
{
    public class SqlOptions
    {
        public static string test1 = Appsettings.app(new string[] { "SqlServer", "ConnectionString" });


    }
}
