using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using JokeService.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace JokeService {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions _options;

        public Worker(ILogger<Worker> logger, WorkerOptions options) {
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {

                var client = new RestClient("https://api.ratesapi.io/api");
                var request = new RestRequest("latest", DataFormat.Json);

                var response = client.Get(request);
                if(response.IsSuccessful == true) {
                    RatesModel rates = JsonConvert.DeserializeObject<RatesModel>(response.Content);

                    StringBuilder myStringBuilder = new StringBuilder($"{DateTime.Now.ToString()} US: {rates.rates.USD} Great Britain: {rates.rates.GBP}");

                    System.IO.TextWriter tw;
                    tw = new StreamWriter(Path.Combine(_options.OutputFolder, "Rates.txt"), true);
                    tw.WriteLine(myStringBuilder);
                    tw.Close();
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
