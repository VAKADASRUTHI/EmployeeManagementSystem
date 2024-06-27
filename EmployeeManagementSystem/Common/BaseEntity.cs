using Newtonsoft.Json;

namespace EmployeeManagementSystem.Common
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
