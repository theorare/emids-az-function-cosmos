using AzEmidsFunction.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AzEmidsFunction
{

    public static class GetAllPatients
    {
        //const string connectionString = "DefaultEndpointsProtocol=https;AccountName=stemidsbth;AccountKey=UsfIbVyTv5WZfC/G/NrNDarhA41IoU+xG8tMzLOGipH8fXzECylPkdSGM5B17Xo91GZnYArtatkPxlWLo2XThg==;EndpointSuffix=core.windows.net";

        [FunctionName("GetAllPatients")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "patients")] HttpRequest req
            , ILogger log)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("PatientTestTable");
            ITableDBRepository<Patient> respository = new TableDBRepository<Patient>();
            var patientItems = await respository.GetAllPatientInformation(table, "Patient");
            return new OkObjectResult(patientItems);
        }

        [FunctionName("GetAPatient")]
        public static async Task<IActionResult> GetPatientAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "patient/{emailId}")] HttpRequest req
            , ILogger log)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("PatientTestTable");
            ITableDBRepository<Patient> respository = new TableDBRepository<Patient>();
            var emailId = (req.HttpContext.GetRouteValue("emailId") as string) ?? string.Empty;
            var patientItems = await respository.GetAPatientInformation(table, "Patient", emailId);
            if (patientItems is null)
                return new NoContentResult();
            return new OkObjectResult(patientItems);
        }

        [FunctionName("CreateOrUpdateAPatient")]
        public static async Task<object> CreateOrUpdateAPatient(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "patient/create")] HttpRequestMessage req
            , ILogger log)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            var patient = JsonConvert.DeserializeObject<Patient>(jsonContent);
            if (patient is null)
                return new BadRequestResult();
            log.LogInformation($"Patient {patient.Name} received with email {patient.EmailId} at UTC time {System.DateTime.UtcNow}");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("PatientTestTable");
            ITableDBRepository<Patient> respository = new TableDBRepository<Patient>();
            var patientItems = await respository.CreateAPatientInformation(table, "Patient", patient);
            return (object)new OkObjectResult(patient);
        }
    }
}