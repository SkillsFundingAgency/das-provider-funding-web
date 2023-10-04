using System.Text.Json.Serialization;

namespace SFA.DAS.ProviderFunding.Web.Models
{
    
    public class GetProviderSummaryResult
    {
        [JsonPropertyName("canAccessService")]
        public bool CanAccessService { get; set; }
        
    }

}
