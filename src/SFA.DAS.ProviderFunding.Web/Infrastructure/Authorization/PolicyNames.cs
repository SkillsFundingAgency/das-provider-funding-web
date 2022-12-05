using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class PolicyNames
    {
        public static string HasProviderAccount => nameof(HasProviderAccount);
    }
}