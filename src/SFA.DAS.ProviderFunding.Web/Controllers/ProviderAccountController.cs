using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFunding.Web.Infrastructure;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ProviderAccountController : ControllerBase
    {
        [Route("signout", Name = RouteNames.ProviderSignOut)]
        public IActionResult SignOutProvider()
        {
            return SignOut(
                new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "",
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                WsFederationDefaults.AuthenticationScheme);
        }
    }
}