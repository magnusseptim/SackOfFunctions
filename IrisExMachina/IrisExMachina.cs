using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using IrisExLibStd.Validator;
using IrisExLibStd.Model;
using System.IO;
using Newtonsoft.Json;
using FluentAssertions;
using IrisExLibStd.Prediction;
using System;

namespace IrisExMachina
{
    public static class IrisExMachina
    {
        [FunctionName("IrisExMachina")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            ActionResult response = new OkResult();
            IrisModelValidator validator = new IrisModelValidator();
            try
            {
                log.Info("IrisExMachina started");

                string requestBody = new StreamReader(req.Body).ReadToEnd();
                IrisModel requestData = JsonConvert.DeserializeObject<IrisModel>(requestBody);
                FluentValidation.Results.ValidationResult result = validator.Validate(requestData);
                result.IsValid.Should().BeTrue();
                PEngine engine = new PEngine("iris-data.txt");
                var trainedModel = engine.Train();
                IrisPredictedModel predicted = trainedModel.Predict(requestData);


                response = new OkObjectResult($"Predicted flower type { predicted.PredictedLabel}");
            }
            catch (Exception ex)
            {
                response = new BadRequestObjectResult(ex.Message);
                log.Error(ex.Message);
            }
            return response;
        }
    }
}
