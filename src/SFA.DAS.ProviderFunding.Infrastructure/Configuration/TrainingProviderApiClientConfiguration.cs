using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderFunding.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public record TrainingProviderApiClientConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}