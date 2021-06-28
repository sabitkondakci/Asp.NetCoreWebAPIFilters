using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebAPIFilters.Controllers;

namespace WebAPIFilters
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment,IHostEnvironment hostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ResultFilterAttribute>();

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("StartsWithZero", typeof(CustomConstraint));
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new ActionFilterAttribute("GlobalAction"));
                //options.Filters.Add(new ResourceFilterAttribute("GlobalResource"));
                options.Filters.AddService<ResultFilterAttribute>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Error handling on page base, system keeps running well.
            app.UseStatusCodePages(new StatusCodePagesOptions()
            {
                HandleAsync = (ctx) =>
                {
                    if (ctx.HttpContext.Response.StatusCode == 404)
                    {
                        return Task.FromException(
                            new Exception("Bad Request!, Range Has To Be Between 0 - 1000 inclusive"));
                    }

                    return Task.FromResult(0);
                }
            });

            //for investigation of endpoints
            app.Use(async (context,next) =>
            {
                var endpoint = context.GetEndpoint();
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/hello/{name:alpha:minlength(2)?}", async context =>
                {
                    var name = context.GetRouteValue("name");
                    await context.Response.WriteAsync($"Hello {name}");
                });

                endpoints.MapGet("/custom/{name:StartsWithZero?}", async context =>
                {
                    var name = context.GetRouteValue("name");
                    await context.Response.WriteAsync($"Hello From Custom {name}");
                });
            });
        }
    }
}
