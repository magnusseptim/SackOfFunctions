using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure;
using System.Text;
using System.Collections.Generic;
using AugmentedTable.Model;
using Newtonsoft.Json;

namespace AugmentedTable
{
    public class AugmentedInsert
    {
        private readonly CloudTableClient tableClient;
        private readonly CloudBlobClient blobClient;
        private readonly CloudQueueClient queueClient;

        public AugmentedInsert(
            CloudTableClient tableClient,
            CloudBlobClient blobClient,
            CloudQueueClient queueClient)
        {
            this.tableClient = tableClient;
            this.blobClient = blobClient;
            this.queueClient = queueClient;
        }

        [FunctionName("AugmentedInsert")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var table = tableClient.GetTableReference(Environment.GetEnvironmentVariable("tableName"));
                var queue = queueClient.GetQueueReference(Environment.GetEnvironmentVariable("queueName"));
                var blobContainer = blobClient.GetContainerReference(Environment.GetEnvironmentVariable("blobStorageName"));
                List<TableResult> results = new List<TableResult>();

                StringBuilder builder = new StringBuilder();
                List<SampleModel.Data> samples = new List<SampleModel.Data>();

                var message = await queue.GetMessageAsync();

                if (message != null)
                {
                    samples = SampleModel.Data.FromJson<List<SampleModel.Data>>(message.AsString);
                }

                await blobContainer.CreateIfNotExistsAsync();

                foreach (var item in samples)
                {
                    HubItem hItem = new HubItem();
                    var blockBlob = blobContainer.GetBlockBlobReference(item.Id);
                    await blockBlob.UploadTextAsync(JsonConvert.SerializeObject(item));
                    hItem.BlobUri = blockBlob.Uri.ToString();

                    var insertData = TableOperation.Insert(hItem);
                    await table.CreateIfNotExistsAsync();
                    results.Add(await table.ExecuteAsync(insertData));
                }

                // Protip : Remove your message from queue, Azure Queue will not do this for you automatically
                await queue.DeleteMessageAsync(message);

                return new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("It allways may be worst");
            }
        }

    }
}
