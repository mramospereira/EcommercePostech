using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MyEcommerce.Products;

namespace MyEcommerce.GetProductsByCategory
{
    public class GetProductsByCategory
    {
        private readonly ILogger _logger;

        public GetProductsByCategory(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProductsByCategory>();
        }

        [Function("GetProductsByCategory")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,

        [CosmosDBInput(databaseName:"%DatabaseName%", 
                       containerName:"%ContainerName%", 
                       Connection = "CosmosDBConnectionString",
                       //SqlQuery = "SELECT * FROM c WHERE c.Category = {category}",
                       //SqlQuery = "SELECT * FROM c WHERE c.Category = {category} and c.Deleted = true",
                       PartitionKey = "{partitionKey}")] List<ProductDTO> products
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var jsonString = JsonSerializer.Serialize(products);

            response.WriteString(jsonString);

            return response;
        }
    }
}
