using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PLMSide.Data.Entites
{
   public class ResultModel
    {
       public ResultModel()
       {
           IsSuccess = false;
           Message = "";
       }
       public bool isLogin
       {
           get;
           set;
       }

       public string Message
       {
           get;
           set;

       }

       public string ApMessage
       {
           get;
           set;

       }

       public string StatusCode
       {
           get;
           set;

       }

       public bool IsSuccess
       {
           get;
           set;
       }
       public object Data
       {
           get;
           set;
       }
    }
}
