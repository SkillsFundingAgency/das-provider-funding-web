using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ProviderFunding.Infrastructure;
using SFA.DAS.ProviderFunding.Infrastructure.Configuration;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationProviderExtensions
    {
        public static void AddAndConfigureProviderAuthentication(this IServiceCollection services, ProviderIdams idams)
        {
            var cookieOptions = new Action<CookieAuthenticationOptions>(options =>
            {
                options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.AccessDeniedPath = "/error/403";
                options.Cookie.Name = $"SFA.DAS.ProviderFunding.Web.Auth";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme =
                        WsFederationDefaults.AuthenticationScheme;
                })
                .AddWsFederation(options =>
                {
                    options.MetadataAddress = idams.MetadataAddress;
                    options.Wtrealm = idams.Wtrealm;
                    options.CallbackPath = "/{ukprn}/provider-funding";
                    options.Events.OnSecurityTokenValidated = async (ctx) =>
                    {
                        await PopulateProviderClaims(ctx.HttpContext, ctx.Principal);
                    };
                }).AddCookie(cookieOptions);

        }

        private static Task PopulateProviderClaims(HttpContext httpContext, ClaimsPrincipal principal)
        {
            var providerId = principal.Claims.First(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
            var displayName = principal.Claims.First(c => c.Type.Equals(ProviderClaims.DisplayName)).Value;
            httpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType, providerId);
            httpContext.Items.Add(ProviderClaims.DisplayName, displayName);
            principal.Identities.First().AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, providerId));
            principal.Identities.First().AddClaim(new Claim(ProviderClaims.DisplayName, displayName));
            return Task.CompletedTask;
        }
    }
}