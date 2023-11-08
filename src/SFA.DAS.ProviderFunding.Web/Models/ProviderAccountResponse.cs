using System.Text.Json.Serialization;

namespace SFA.DAS.ProviderFunding.Web.Models
{
    public class ProviderAccountResponse
    {
        [JsonPropertyName("canAccessService")]
        public bool CanAccessService { get; set; }
    }
}
