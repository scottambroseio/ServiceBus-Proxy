using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ServiceBusProxy.Function
{
    public static class Function
    {
        [FunctionName(nameof(Function))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var body = new StreamReader(req.Body).ReadToEnd();

            log.LogInformation(body);

            return new ObjectResult(true);
        }
    }
}