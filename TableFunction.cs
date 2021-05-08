using AzEmidsFunction.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzEmidsFunction {
    
    public static class GetAllPatients {
        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=stemidsbth;AccountKey=UsfIbVyTv5WZfC/G/NrNDarhA41IoU+xG8tMzLOGipH8fXzECylPkdSGM5B17Xo91GZnYArtatkPxlWLo2XThg==;EndpointSuffix=core.windows.net";
        [FunctionName("GetAllPatients")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="patients")] HttpRequest req
            ,ILogger log)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("PatientTestTable");
            ITableDBRepository<Patient> respository = new TableDBRepository<Patient>();
            var patientItems = await respository.GetAllMessages(table, "Patient");
            return new OkObjectResult(patientItems);
        }
    }
}