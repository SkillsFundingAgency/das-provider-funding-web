using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderFunding.Web.Infrastructure;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    [Route("{ukprn}")]
    public class HomeController : Controller
    {
        private readonly IProviderEarningsService _service;

        public HomeController(IProviderEarningsService service)
        {
            _service = service;
        }

        [Route("", Name = RouteNames.ProviderServiceStartDefault, Order = 0)]
        public async Task<IActionResult> Index(long ukprn)
        {
            var data = await _service.GetSummary(ukprn);

            var model = new IndicativeEarningsReportViewModel
            {
                Total = data.TotalEarningsForCurrentAcademicYear,
                Levy = data.TotalLevyEarningsForCurrentAcademicYear,
                NonLevy = data.TotalNonLevyEarningsForCurrentAcademicYear,
                NonLevyGovernmentContribution = data.TotalNonLevyGovernmentContributionForCurrentAcademicYear,
                NonLevyEmployerContribution = data.TotalNonLevyEmployerContributionForCurrentAcademicYear
            };

            return View(model);
        }
    }
}