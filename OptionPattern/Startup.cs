using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OptionPattern.AutoMapper;
using OptionPattern.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

[assembly: FunctionsStartup(typeof(OptionPattern.Startup))]

namespace OptionPattern
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false); // Just for test purpose

            builder.Services.AddAutoMapper(Assembly.GetAssembly(this.GetType()));
        }
    }
}
