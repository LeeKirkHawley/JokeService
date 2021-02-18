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


namespace JokeService {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger) {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {

                var client = new RestClient("https://api.ratesapi.io/api");
                //client.Authenticator = new HttpBasicAuthenticator("username", "password");

                var request = new RestRequest("latest", DataFormat.Json);

                var response = client.Get(request);
                if(response.IsSuccessful == true) {
                    RatesModel rates = JsonConvert.DeserializeObject<RatesModel>(response.Content);

                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
