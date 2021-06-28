using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPIFilters.Controllers;

namespace WebAPIFilters
{
    internal class ActionFilterAttribute : Attribute, IActionFilter, IOrderedFilter
    {
        private readonly string _type;

        static ActionFilterAttribute()
        {
            Console.WriteLine("Static Method Was Triggered!");
        }

        public ActionFilterAttribute(string type, int order = 0)
        {
            Order = order;
            _type = type;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(Quick.StringFormatter("Action Ended", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(Quick.StringFormatter("Action Started", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }

        public int Order { get; }
    }


    internal class ActionFilterAsyncAttribute : Attribute, IAsyncActionFilter, IOrderedFilter
    {
        private readonly string _type;

        public ActionFilterAsyncAttribute(string type, int order = 0)
        {
            Order = order;
            _type = type;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine(Quick.StringFormatter("Action Started Async",$"{++Quick.Count}",_type,$"{Order}")+"\n");
            await next();
            Console.WriteLine(Quick.StringFormatter("Action Ended Async", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }

        public int Order { get; }
    }
}