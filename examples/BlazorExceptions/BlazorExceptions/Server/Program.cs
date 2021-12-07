using BlazorExceptions.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlazorExceptions.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        
        private JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        };
        
        public override Task OnExceptionAsync(ExceptionContext context)
        {

            switch (context.Exception)
            {
                case WeatherUnavailableException :
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;                    
                    break;
                
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;                    
                    break;
            }

            context.HttpContext.Response.ContentType = "application/json";
            return context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(context.Exception, _settings));
        }
    }
}
