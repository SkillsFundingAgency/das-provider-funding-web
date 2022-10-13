using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFunding.Web.Configuration;
using SFA.DAS.ProviderFunding.Web.Configuration.Routing;
using SFA.DAS.ProviderFunding.Web.Extensions;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [Route(RoutePaths.AccountRoutePath)]
    public class DashboardController : Controller
    {
        private readonly DashboardOrchestrator _orchestrator;

        public DashboardController(DashboardOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("", Name = RouteNames.Dashboard_Get)]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await _orchestrator.GetDashboardViewModelAsync(User.ToVacancyUser());
            return View(vm.HasEmployerReviewPermission ? ViewNames.DashboardWithReview : ViewNames.DashboardNoReview, vm);
        }
    }
}