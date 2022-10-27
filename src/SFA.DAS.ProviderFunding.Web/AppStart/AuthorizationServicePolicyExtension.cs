using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    public static class AuthorizationServicePolicyExtension
    {

        private const string ProviderDaa = "DAA";
        private const string ProviderDab = "DAB";
        private const string ProviderDac = "DAC";
        private const string ProviderDav = "DAV";

        public static void AddAuthorizationServicePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    PolicyNames
                        .HasProviderAccount
                    , policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ProviderClaims.ProviderUkprn);
                        policy.RequireClaim(ProviderClaims.Service, ProviderDaa, ProviderDab, ProviderDac, ProviderDav);
                        policy.Requirements.Add(new ProviderUkPrnRequirement());
                    });
            });
        }
    }

}