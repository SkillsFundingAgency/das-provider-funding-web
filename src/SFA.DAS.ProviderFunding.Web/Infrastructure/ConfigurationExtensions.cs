using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.ProviderFunding.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationExtensions
    {
        public static bool IsDev(this IConfiguration configuration)
        {
            return configuration["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
        public static bool IsLocal(this IConfiguration configuration)
        {
            return configuration["EnvironmentName"].StartsWith("LOCAL", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}