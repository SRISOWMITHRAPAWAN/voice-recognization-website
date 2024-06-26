using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared25.Models;
using Shared25.Service;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FunctionAppSwagger
{
    public static class Functions
    {
        [FunctionName("TestSwagger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("This is an Http trigger function to test Swagger.");

            string responseMessage = "This is an Http trigger function to test Swagger.";

            return new OkObjectResult(responseMessage);
        }
        static MstService mstService = new MstService();

        [FunctionName("GetMstDetails")]
        public static IActionResult MstDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var listresponse = mstService.GetAllMstDetails();
                return new OkObjectResult(listresponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving MST details: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [FunctionName("InsertMstMapping")]
        public static async Task<IActionResult> InsertMstMapping(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "InsertMstMapping")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var mstMappingDetails = JsonConvert.DeserializeObject<MstMappingEntity>(requestBody);
                var result = mstService.AddMstMapping(mstMappingDetails);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting MST mapping details: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }

   

}