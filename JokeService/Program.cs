using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokeService.Models;

namespace JokeService {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) => {
                    IConfiguration configuration = hostContext.Configuration;
                    WorkerOptions options = configuration.GetSection("Configuration").Get<WorkerOptions>();
                    services.AddSingleton(options);

                    services.AddHostedService<Worker>();
                });
    }
}
