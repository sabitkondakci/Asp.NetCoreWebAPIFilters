using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIFilters.Controllers;
using System.Text.Json;

namespace WebAPIFilters
{
    internal class ResourceFilterAttribute : Attribute,IResourceFilter,IOrderedFilter
    {
        private readonly string _type;

        public ResourceFilterAttribute(string type,int order=0)
        {
            Order = order;
            _type = type;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine(Quick.StringFormatter("Resource Ended", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }
        internal class Identity
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var id = new Identity()
            {
                FirstName = "Inan",
                LastName = "Arkan",
                DateOfBirth = DateTime.Now
            };

            var json = JsonSerializer.Serialize(id);

            context.Result = new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = 200
            };
            Console.WriteLine(Quick.StringFormatter("Resource Started", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }

        public int Order { get; }
    }

    internal class ResourceFilterAsyncAttribute:Attribute,IAsyncResourceFilter,IOrderedFilter
    {
        private readonly string _type;

        public ResourceFilterAsyncAttribute(string type,int order)
        {
            Order = order;
            _type = type;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine(Quick.StringFormatter("Resource Started Async", $"{++Quick.Count}", _type, $"{Order}")+"\n");
            await next();
            Console.WriteLine(Quick.StringFormatter("Resource Ended Async", $"{++Quick.Count}", _type, $"{Order}")+"\n");
        }

        public int Order { get; }
    }
}
