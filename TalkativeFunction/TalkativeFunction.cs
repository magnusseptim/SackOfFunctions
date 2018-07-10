using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace TalkativeFunction
{
    public static class TalkativeFunction
    {
        [FunctionName("TalkativeFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            IActionResult result;
            try
            {
                Random rand = new Random();
                string url = req.Form["url"];
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("url",url)
                });
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendAsync), new ModelHelper(url, formContent));
                string resultMessage = $"Chaos (or randomness) is the only truth answer {rand.Next().ToString()}";
                log.Info(resultMessage);
                result =  new OkObjectResult(resultMessage);
                
            }
            catch (Exception ex)
            {
                log.Error($"I am glad to present you this exception message :  {ex.Message}");
                result = new BadRequestObjectResult(ex);
            }

            return result;
        }

        private static async void SendAsync(object data)
        {
            if (data is ModelHelper)
            {
                using (var client = new HttpClient())
                {
                    ModelHelper unwrappedData = data as ModelHelper;
                    await client.PostAsync("http://" + unwrappedData.Url, unwrappedData.Form);
                }
            }
        }
    }

    internal class ModelHelper
    {
        public ModelHelper(string url, FormUrlEncodedContent form)
        {
            this.Url = url;
            this.Form = form;
        }

        internal string Url { get; set; }
        internal FormUrlEncodedContent Form { get; set; }
    }
}
