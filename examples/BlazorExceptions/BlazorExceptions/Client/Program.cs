using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorExceptions.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<MyHttpClient>();

            await builder.Build().RunAsync();
        }
    }

    public class MyHttpClient
    {
        private HttpClient _httpClient;

        public MyHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(string url)
        {
            Console.WriteLine("req: " + url);

            var response = await _httpClient.GetAsync(url);

            Console.WriteLine("res: " + response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var responseEx = await response.Content.ReadAsStringAsync();
                var exception = JsonConvert.DeserializeObject<Exception>(responseEx, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                });
                Console.WriteLine("type: " + exception.GetType().FullName);
                throw exception;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
