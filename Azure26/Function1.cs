using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle;
using System.Net.Http;
using System.Net;
using Shared25.Models;
using Shared25.Service;

namespace Azure26
{
    public static class SwaggerFunctions
    {
        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public static Task<HttpResponseMessage> Swagger(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")] HttpRequestMessage req,
                [SwashBuckleClient] ISwashBuckleClient swasBuckleClient)
        {
            return Task.FromResult(swasBuckleClient.CreateSwaggerJsonDocumentResponse(req));
        }
        [SwaggerIgnore]
        [FunctionName("SwaggerUI")]
        public static Task<HttpResponseMessage> SwaggerUI(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")] HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swasBuckleClient)
        {
            return Task.FromResult(swasBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
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
