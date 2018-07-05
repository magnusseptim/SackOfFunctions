
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Web.Http;

namespace SackOfFunctions
{
    public static class ShakehandFunction
    {
        [FunctionName("ShakehandFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            StringValues name;
            StringValues surname;
            IActionResult result = new InternalServerErrorResult();

            try
            {
                log.Info("C# HTTP trigger shakehand function processed a request");
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                dynamic reqData = JsonConvert.DeserializeObject(requestBody);

                bool hasValues = req.Query.TryGetValue("name", out name) && req.Query.TryGetValue("surname", out surname);
                result = !hasValues ? BuildBadReques(name, surname) : new OkObjectResult($"Hello, {name.FirstOrDefault() ?? reqData?.name } " +
                                                                                                $"{surname.FirstOrDefault() ?? reqData?.surname} ");
            }
            catch (Exception ex)
            {
                log.Error($"I am glad to present you this exception message :  {ex.Message}");
                result = new BadRequestObjectResult(ex);
            }
            return result;
        }

        private static IActionResult BuildBadReques(StringValues name, StringValues surname)
        {
            return (name.Any() || surname.Any())
                        ? new BadRequestObjectResult("Whats the heck is wrong with you ? Check name and surname params!") 
                        : null;
        }
    }
    
}
