using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace TriggeredSemaphore
{
    public static class TriggeredSemaphore
    {
        private static Semaphore pool;
        private static int padding;
        private static volatile string message;

        [FunctionName("TriggeredSemaphore")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Triggered Semaphore started");
            int number;
            try
            {
                number = Convert.ToInt32(req.Form["number"]);
            }
            catch (Exception ex)
            {
                log.Error($"Randomness was choosen...Not at all of course, exception was found : {ex.Message}");
                number = new Random().Next(0, 9);
            }

            if (number < 3)
            {
                return new BadRequestObjectResult("We won't play like that. " +
                    "If you would like to see result, put number larger than 3");
            }
            pool = new Semaphore(0, number - 2);

            var parent = Task.Factory.StartNew(() =>
            {
                foreach (var x in Enumerable.Range(1,number))
                {
                    Task.Factory.StartNew((object num) => PrintThreadMessageAndWait((int)num, pool,log), x, TaskCreationOptions.AttachedToParent);
                }
            });

            pool.Release(number - 2);
            parent.Wait();

            return new OkObjectResult(message);
        }

        private static void PrintThreadMessageAndWait(int number, Semaphore pool, TraceWriter log)
        {
            HandleMessageResponse($"Thread {Thread.CurrentThread.ManagedThreadId} runned as a {number.ToString()} start his work", log);

            pool.WaitOne();

            int pad = Interlocked.Add(ref padding, 100);

            HandleMessageResponse($"Thread {number} enters the semaphore.", log);
            Thread.Sleep(1000 * number + padding);

            HandleMessageResponse($"Padding : {padding}", log);
            HandleMessageResponse($"Thread {number} releases the semaphore.", log);
            HandleMessageResponse($"Thread {number} previous semaphore count: {pool.Release()}", log);
        }

        private static void HandleMessageResponse(string text, TraceWriter log)
        {
            log.Info(text);
            message += "\r\n";
            message += text;
        }
    }
}
