using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI.Attributes;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    
    [HideNavigationBar(hideAccountHeader: false, hideNavigationLinks: true)]
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 403:
                case 404:
                    return View(statusCode.ToString());
                default:
                    return View();
            }
        }
    }
}