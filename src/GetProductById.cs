using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MyEcommerce.Products
{
    public class GetProductById
    {
        private readonly ILogger _logger;

        public GetProductById(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductById>();
        }

        [Function("GetProductById")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        
        [CosmosDBInput(databaseName:"%DatabaseName%", 
                       containerName:"%ContainerName%", 
                       Connection = "CosmosDBConnectionString", 
                       Id = "{id}", 
                       PartitionKey = "{partitionKey}")] ProductDTO product
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var jsonString = JsonSerializer.Serialize(product);

            response.WriteString(jsonString);

            return response;
        }
    }
}
