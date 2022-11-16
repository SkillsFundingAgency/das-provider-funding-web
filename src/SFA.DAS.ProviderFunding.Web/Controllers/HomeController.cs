using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Infrastructure;
using SFA.DAS.ProviderFunding.Web.Models;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    [Route("{ukprn}")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Route("", Name = RouteNames.ProviderServiceStartDefault, Order = 0)]
        public IActionResult Index(long ukprn)
        {
            // var ukprn = HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;
            var model = new IndicativeEarningsReportViewModel
            {
                Total = 2825558,
                Levy = 1412453,
                NonLevy = 701003,
                NonLevyGovernmentContribution = 670952,
                NonLevyEmployerContribution = 41150
            };


            return View(model);
        }
    }
}