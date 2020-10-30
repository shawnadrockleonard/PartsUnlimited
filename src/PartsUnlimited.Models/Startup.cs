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

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            WebHostEnvironment = env;
            Configuration = configuration;
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

            services.AddTransient<IAppSettingEntity, ConfigAppSettingEntity>(config =>
            {
                var connection = Configuration.Get<ConfigAppSettingEntity>();
                return connection;
            });

        }

        //Configure is required by 'ef migrations add' command.
        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
