
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using OptionPattern.Model;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace OptionPattern
{
    public class OptionPattern
    {
        private readonly BasicOptions _options;
        private readonly IMapper _mapper;

        public  OptionPattern(
            IOptionsMonitor<BasicOptions> optionsAccessor,
            IMapper mapper)
        {
            _options = optionsAccessor.CurrentValue;
            _mapper = mapper;
        }

        [FunctionName("OptionPattern")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                log.LogWarning($"You are use {_options.Enviroment.Name} Env");

                if(_options.Enviroment.Standalone)
                {
                    log.LogInformation($"Session run as standalone");
                }

                var reduced = _mapper.Map<BasicOptionsDto>(_options);
                return new OkObjectResult(JsonConvert.SerializeObject(reduced));
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, "Sometings bad happend, Harry...");
            }

            return new BadRequestResult();
        }
    }
}
