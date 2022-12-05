using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    public static class ProviderStubAuthentication
    {
        [ExcludeFromCodeCoverage]
        public static void AddProviderStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
                "Provider-stub",
                options => { });
        }
    }
}