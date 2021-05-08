using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzEmidsFunction.Data
{
    public class Patient
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PatientItem : TableEntity {
        public PatientItem() {}
        public PatientItem(string EmailId) {
            PartitionKey = "Patient";
            RowKey = EmailId;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public string ImageUrl { get; set; }
    }
}
