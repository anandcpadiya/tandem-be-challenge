using Newtonsoft.Json;

namespace TandemBEProject.Models
{
    public class UserModel
    {
        public Guid? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string? EmailAddress { get; set; }
    }
}
