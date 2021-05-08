using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System;
namespace AzEmidsFunction.Data
{
    public class Patient
    {
        public Patient() {}
        public Patient(Patient patient)
        {
            patient.Id = string.IsNullOrWhiteSpace(patient.Id) ? Guid.NewGuid().ToString() : patient.Id;
            patient.Name = string.IsNullOrWhiteSpace(patient.Name) ? "Unknown Patient-" + Guid.NewGuid() : patient.Name;
            patient.EmailId = string.IsNullOrWhiteSpace(patient.EmailId) ? "mail@example.com-" + Guid.NewGuid().ToString() : patient.EmailId;
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PatientItem : TableEntity
    {
        public PatientItem() { }
        public PatientItem(string EmailId)
        {
            PartitionKey = "Patient";
            RowKey = EmailId;
        }
        public PatientItem(Patient patient) 
        {
            PartitionKey = "Patient";
            RowKey = patient.EmailId;
            Id = patient.Id;
            Name = patient.Name;
            DateOfBirth = patient.DateOfBirth;
            EmailId = patient.EmailId;
            ImageUrl = patient.ImageUrl;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public string ImageUrl { get; set; }
    }
}
