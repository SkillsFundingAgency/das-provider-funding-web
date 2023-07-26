using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.ProviderFunding.Web.Infrastructure;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ProviderAccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProviderAccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("signout", Name = RouteNames.ProviderSignOut)]
        public IActionResult SignOutProvider()
        {
            var useDfESignIn = _configuration["UseDfESignIn"] != null && _configuration["UseDfESignIn"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase);

            var authScheme = useDfESignIn
                ? OpenIdConnectDefaults.AuthenticationScheme
                : WsFederationDefaults.AuthenticationScheme;

            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                authScheme);
        }
    }
}