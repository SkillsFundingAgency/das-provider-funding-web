using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderFunding.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderIdams
    {
        public string MetadataAddress { get; set; }
        public string Wtrealm { get; set; }
    }
}