using Newtonsoft.Json;

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
}
