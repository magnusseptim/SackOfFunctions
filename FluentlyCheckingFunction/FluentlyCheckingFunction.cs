using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using FluentlyCheckingFunction.Model;
using FluentlyCheckingFunction.Validator;
using FluentAssertions;
using System.Collections.Generic;

namespace FluentlyCheckingFunction
{
    public static class FluentlyCheckingFunction
    {
        [FunctionName("FluentlyCheckingFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError,"No data was send");

            log.Info("C# HTTP trigger function processed a request.");
            OrderModel order;
            OrderModelValidator validator = new OrderModelValidator();
            log.Info("Fluent Validator is running");

            try
            {
                if (req.Method == HttpMethod.Post)
                {

                    order = req.Content.IsMimeMultipartContent() ? await ReadMultipartContent(req.Content) : null;
                    order = req.Content.IsFormData() ? await ReadFormContent(req.Content) : order;

                    FluentValidation.Results.ValidationResult result = validator.Validate(order);
                    result.IsValid.Should().BeTrue();
                    response = req.CreateResponse(HttpStatusCode.OK, "Object structure and data was fine");
                }
            }
            catch (Exception ex)
            {
                response = req.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
            return response;
        }


        private async static Task<OrderModel> ReadFormContent(HttpContent content)
        {
            OrderModel order = new OrderModel();
            var data = await content.ReadAsFormDataAsync();
            order.ID = Guid.Parse(data.Get("ID"));
            order.Warename = data.Get("WareName");
            order.Quantity = int.Parse(data.Get("Quantity"));
            order.Unit = data.Get("Unit");

            return order;
        }
        private async static Task<OrderModel> ReadMultipartContent(HttpContent content)
        {
            OrderModel order = new OrderModel();
            var provider = new MultipartMemoryStreamProvider();
            await content.ReadAsMultipartAsync(provider);
            List<(string key, string data)> preprocessedContent = new List<(string key, string data)>();

            foreach (var x in provider.Contents)
            {
                preprocessedContent.Add((key: x.Headers.ContentDisposition.Name.Replace("\"", ""), data: x.ReadAsStringAsync().Result));
            }

            order.ID = Guid.Parse(preprocessedContent.Where(x => x.key == "ID").Select(x => x.data).FirstOrDefault());
            order.Warename = preprocessedContent.Where(x => x.key == "Warename").Select(x => x.data).FirstOrDefault();
            order.Quantity = int.Parse(preprocessedContent.Where(x => x.key == "Quantity").Select(x => x.data).FirstOrDefault());
            order.Unit = preprocessedContent.Where(x => x.key == "Unit").Select(x => x.data).FirstOrDefault();
            return order;
        }
    }
}
