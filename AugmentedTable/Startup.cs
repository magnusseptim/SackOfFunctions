using Microsoft.Azure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(AugmentedTable.Startup))]
namespace AugmentedTable
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var account = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            builder.Services.AddScoped(fac => account.CreateCloudTableClient());
            builder.Services.AddScoped(fac => account.CreateCloudBlobClient());
            builder.Services.AddScoped(fac => account.CreateCloudQueueClient());
        }
    }
}
