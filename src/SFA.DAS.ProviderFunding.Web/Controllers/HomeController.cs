using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderFunding.Web.Infrastructure;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using System.Text;

namespace SFA.DAS.ProviderFunding.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    [Route("{ukprn}")]
    public class HomeController : Controller
    {
        private readonly IProviderEarningsService _providerEarningsService;
        private readonly IApprenticeshipsService _apprenticeshipsService;
        private readonly IAcademicYearEarningsReportBuilder _academicYearEarningsReportBuilder;

        public HomeController(
            IProviderEarningsService providerEarningsService,
            IApprenticeshipsService apprenticeshipsService,
            IAcademicYearEarningsReportBuilder academicYearEarningsReportBuilder)
        {
            _providerEarningsService = providerEarningsService;
            _apprenticeshipsService = apprenticeshipsService;
            _academicYearEarningsReportBuilder = academicYearEarningsReportBuilder;
        }

        [Route("", Name = RouteNames.ProviderServiceStartDefault, Order = 0)]
        public async Task<IActionResult> Index(long ukprn)
        {
            var data = await _providerEarningsService.GetSummary(ukprn);

            var model = new IndicativeEarningsReportViewModel
            {
                Ukprn = ukprn,
                Total = data.TotalEarningsForCurrentAcademicYear,
                Levy = data.TotalLevyEarningsForCurrentAcademicYear,
                NonLevy = data.TotalNonLevyEarningsForCurrentAcademicYear,
                NonLevyGovernmentContribution = data.TotalNonLevyGovernmentContributionForCurrentAcademicYear,
                NonLevyEmployerContribution = data.TotalNonLevyEmployerContributionForCurrentAcademicYear
            };

            return View(model);
        }

        [Route("generate-csv", Name = RouteNames.GenerateCSV)]
        public async Task<IActionResult> GenerateCSV(long ukprn)
        {
            var academicYearEarningsData = await _providerEarningsService.GetDetails(ukprn);
            var apprenticeshipsData = await _apprenticeshipsService.GetAll(ukprn);

            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                var report = _academicYearEarningsReportBuilder.Build(academicYearEarningsData, apprenticeshipsData);
                csvWriter.WriteRecords(report);
            }

            memoryStream.Position = 0;

            return File(memoryStream, "text/csv", "AcademicEarningsReport.csv");
        }
    }
}