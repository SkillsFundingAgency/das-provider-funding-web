using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class RouteNames
    {
        public const string ProviderServiceStartDefault = "default";

        public const string ProviderSignOut = "provider-signout";
        public const string GenerateCSV = "generate-csv";
    }
}