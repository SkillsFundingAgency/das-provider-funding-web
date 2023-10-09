using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderFunding.Web.Models.Error;
using System;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [HideNavigationBar(hideAccountHeader: false, hideNavigationLinks: true)]
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly IConfiguration _configuration;

        public ErrorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    return View(statusCode.ToString(), new Error403ViewModel(_configuration["ResourceEnvironmentName"])
                    {
                        UseDfESignIn = _configuration["UseDfESignIn"] != null && 
                                       _configuration["UseDfESignIn"].Equals("true", StringComparison.CurrentCultureIgnoreCase),
                        DashboardLink = _configuration["ProviderSharedUIConfiguration:DashboardUrl"],
                    });
                case 404:
                    return View(statusCode.ToString());
                default:
                    return View();
            }
        }
    }
}