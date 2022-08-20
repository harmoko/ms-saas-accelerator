﻿using System;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Marketplace.Metering;
using Microsoft.Marketplace.SaaSAccelerator.DataAccess.Context;
using Microsoft.Marketplace.SaaSAccelerator.DataAccess.Contracts;
using Microsoft.Marketplace.SaaSAccelerator.DataAccess.Services;
using Microsoft.Marketplace.SaaSAccelerator.Services.Configurations;
using Microsoft.Marketplace.SaaSAccelerator.Services.Contracts;
using Microsoft.Marketplace.SaaSAccelerator.Services.Services;
using Microsoft.Marketplace.SaaSAccelerator.Services.Utilities;

namespace Microsoft.Marketplace.SaaSAccelerator.MeteredTriggeredJob
{
    class Program
    {
        static void Main (string[] args)
        {

            Console.WriteLine($"MeteredExecutor Webjob Started at: {DateTime.Now}");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var config = new SaaSApiClientConfiguration()
            {
                AdAuthenticationEndPoint = configuration["SaaSApiConfiguration:AdAuthenticationEndPoint"],
                ClientId = configuration["SaaSApiConfiguration:ClientId"],
                ClientSecret = configuration["SaaSApiConfiguration:ClientSecret"],
                GrantType = configuration["SaaSApiConfiguration:GrantType"],
                Resource = configuration["SaaSApiConfiguration:Resource"],
                TenantId = configuration["SaaSApiConfiguration:TenantId"],
                SupportMeteredBilling = Convert.ToBoolean(configuration["SaaSApiConfiguration:SupportMeteredBilling"])
            };

            var creds = new ClientSecretCredential(config.TenantId.ToString(), config.ClientId.ToString(), config.ClientSecret);

            var services = new ServiceCollection()
                            .AddDbContext<SaasKitContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                            .AddScoped<ISchedulerFrequencyRepository, SchedulerFrequencyRepository>()
                            .AddScoped<IMeteredPlanSchedulerManagementRepository, MeteredPlanSchedulerManagementRepository>()
                            .AddScoped<ISchedulerManagerViewRepository, SchedulerManagerViewRepository>()
                            .AddScoped<ISubscriptionUsageLogsRepository, SubscriptionUsageLogsRepository>()
                            .AddSingleton<IMeteredBillingApiService>(new MeteredBillingApiService(new MarketplaceMeteringClient(creds), config, new MeteringApiClientLogger()))
                            .AddSingleton<Executor, Executor>()
                            .BuildServiceProvider();

            services
                .GetService<Executor>()
                    .Execute();
            Console.WriteLine($"MeteredExecutor Webjob Ended at: {DateTime.Now}");

        }
    }
}