using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderFunding.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderFundingApiOptions : IApimClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string ApiVersion { get; set; }
    }
}