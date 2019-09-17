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


[assembly: FunctionsStartup(typeof(OptionPattern.Startup))]

namespace OptionPattern
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false)
                .Build(); // Just for test purpose

            builder.Services.Configure<BasicOptions>(con => config.GetSection("BasicOptions").Bind(con));

            builder.Services.AddAutoMapper(Assembly.GetAssembly(this.GetType()));
        }
    }
}
