using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebAPIFilters
{
    public class ResultFilterAttribute:Attribute,IResultFilter
    {
        private readonly ILogger<ResultFilterAttribute> _logger;
        private readonly Guid _guid;
        private readonly string _type;

        public ResultFilterAttribute(ILogger<ResultFilterAttribute> logger, string type="Global")
        {
            _type = type;
            _logger = logger;
            _guid = Guid.NewGuid();
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation($"{_guid} Result Filter - After - {_type}");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation($"{_guid} Result Filter - Before - {_type}");
        }
    }
}
