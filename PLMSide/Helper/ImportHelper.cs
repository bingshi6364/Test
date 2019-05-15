using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLMSide.Data.Entites;

namespace PLMSide.Helper
{
    public static class ImportHelper
    {
        public static void ImportFile(IFormFile f1, string filepath)
        {
            string filename = ContentDispositionHeaderValue
                                .Parse(f1.ContentDisposition)
                                .FileName
                                .Trim('"');

            //IE 
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);

           // string filepath = hostingEnv.ContentRootPath + @"\Upload";
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
        }
    }
}
