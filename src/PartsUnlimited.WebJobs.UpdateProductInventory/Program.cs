// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PartsUnlimited.WebJobs.UpdateProductInventory
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }
        private static string Environment { get; set; }

        public static async Task Main(string[] args)
        {
            Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Environment = string.IsNullOrEmpty(Environment) ? "Development" : Environment;
            MessageWrite($"Current Environment => {Environment} with args {string.Join(',', args)}");


            var builder = new HostBuilder()
                .UseEnvironment(Environment)
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices()
                    .AddAzureStorage((configQueues) =>
                    {
                        configQueues.MaxDequeueCount = 1;
                        configQueues.BatchSize = 1;
                    },
                    (configBlobs) =>
                    {
                        configBlobs.CentralizedPoisonQueue = true;
                    });
                })
                .ConfigureHostConfiguration(BuildConfiguration)
                .ConfigureServices(ConfigureServices)
                .UseConsoleLifetime();

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }

        public static void BuildConfiguration(IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .AddUserSecrets<Program>()
                   .AddAzureKeyVaultIfAvailable();

            if (Environment.Contains("development", StringComparison.OrdinalIgnoreCase))
            {
                // Re-add User secrets so it takes precedent for local development
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(Configuration);


            var webjobsConnectionString = Configuration["Data:AzureWebJobsStorage:ConnectionString"];
            var dbConnectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            if (string.IsNullOrWhiteSpace(webjobsConnectionString))
            {
                MessageWrite("The configuration value for Azure Web Jobs Connection String is missing.");
                return;
            }

            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                MessageWrite("The configuration value for Database Connection String is missing.");
                return;
            }
        }

        internal static void MessageWrite(string message)
        {
            var msg = $"UTC:{DateTime.UtcNow} => {message}";
            Console.WriteLine(msg);
        }
    }
}