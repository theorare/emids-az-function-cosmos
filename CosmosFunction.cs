using AzEmidsFunction.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace AzEmidsFunction
{
    public static class GetAll
    {
        [FunctionName("GetAll")]
        public static async Task<IEnumerable<Patient>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Get")]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function to get all patient data from Cosmos DB");

            IDocumentDBRepository<Patient> Respository = new DocumentDBRepository<Patient>();
            return await Respository.GetItemsAsync("Patient");
        }
    }

    public static class GetPatient
    {
        [FunctionName("GetPatient")]
        public static async Task<Patient> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Get/{emailId}")]HttpRequest req, TraceWriter log, string id, string emailId)
        {
            log.Info("C# HTTP trigger function to get a single patient data from Cosmos DB");

            IDocumentDBRepository<Patient> Respository = new DocumentDBRepository<Patient>();
            var patients = await Respository.GetItemsAsync(d => d.Id == id && d.EmailId == emailId, "Patient");
            Patient patient = new Patient();
            foreach (var emp in patients)
            {
                patient = emp;
                break;
            }
            return patient;
        }
    }

    public static class CreateOrUpdate
    {
        [FunctionName("CreateOrUpdate")]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "post", "put", Route = "CreateOrUpdate")]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function to create or update a patient record into Cosmos DB");
            try
            {
                IDocumentDBRepository<Patient> Respository = new DocumentDBRepository<Patient>();
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(requestBody);
                if (patient is Patient && !string.IsNullOrWhiteSpace(patient.Id))
                {
                    patient.Id = Guid.NewGuid().ToString();
                    await Respository.CreateItemAsync(patient, "Patient");
                }
                else
                {
                    await Respository.UpdateItemAsync(patient.Id, patient, "Patient");
                }
                return true;
            }
            catch
            {
                log.Info("Error occured while creating a record into Cosmos DB");
                return false;
            }

        }
    }

    public static class Delete
    {
        [FunctionName("Delete")]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Delete/{id}/{cityName}")]HttpRequest req, TraceWriter log, string id, string cityName)
        {
            log.Info("C# HTTP trigger function to delete a record from Cosmos DB");

            IDocumentDBRepository<Patient> Respository = new DocumentDBRepository<Patient>();
            try
            {
                await Respository.DeleteItemAsync(id, "Patient", cityName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
