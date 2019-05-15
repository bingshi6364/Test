using Microsoft.AspNetCore.Mvc.Filters;
using PLMSide.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLMSide.Filter
{
    public class AntiSqlInjectAttribute: Attribute,  IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.Parameters;
            foreach (var p in actionParameters)
            {
                if (p.ParameterType == typeof(string))
                {
                    if (filterContext.ActionArguments.ContainsKey(p.Name))
                    {
                        filterContext.ActionArguments[p.Name] = StringHelper.FilterSql(filterContext.ActionArguments[p.Name].ToString());
                    }
                }
            }
        }
    }
}
