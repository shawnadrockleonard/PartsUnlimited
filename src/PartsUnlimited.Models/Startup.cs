// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartsUnlimited.Models.Extensions;
using System;

namespace PartsUnlimited.Models
{
    public class Startup
    {
        private IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            WebHostEnvironment = env;
            Configuration = BuildConfiguration();
        }

        private IConfiguration BuildConfiguration()
        {
            //Below code demonstrates usage of multiple configuration sources. For instance a setting say 'setting1' is found in both the registered sources, 
            //then the later source will win. By this way a Local config can be overridden by a different setting while deployed remotely.
            var builder = new ConfigurationBuilder()
              .AddJsonFile("config.json")
              .AddJsonFile($"config.{WebHostEnvironment?.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables()
              .AddAzureKeyVaultIfAvailable();

            if (WebHostEnvironment.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("AdminRole");

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            return builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration[ConfigurationPath.Combine("ConnectionStrings", "DefaultConnectionString")];
            if (!String.IsNullOrEmpty(sqlConnectionString))
            {
                services.AddEntityFrameworkSqlServer()
                      .AddDbContext<PartsUnlimitedContext>(options =>
                      {
                          options.UseSqlServer(sqlConnectionString);
                      });
            }

            services.AddTransient<IAppSettingEntity, AppSettingEntity>(config =>
            {
                var connection = Configuration.Get<AppSettingEntity>();
                return connection;
            });

        }

        //Configure is required by 'ef migrations add' command.
        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
