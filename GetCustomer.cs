using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ar.AzureFunctions.Sales.Services;

namespace ar.AzureFunctions.Sales
{
    public class GetCustomer
    {
        private ISalesService _salesService;

        public GetCustomer(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [FunctionName("GetCustomer")]
        public async Task<IActionResult> Run( [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sales/customer")] HttpRequest req, ILogger log)
        {
            log.LogInformation("sales/customer processing a request");

            try
            {
                var criteria = req.GetQueryParameterDictionary();
                var results = await _salesService.GetCustomers(criteria);
                return (ActionResult) new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                string error = $"Error occurred processing request on sales/customer:\r\n{ex.Message}";
                log.LogError(ex,error);
                return new BadRequestObjectResult(error);
            }
        }
    }
}
