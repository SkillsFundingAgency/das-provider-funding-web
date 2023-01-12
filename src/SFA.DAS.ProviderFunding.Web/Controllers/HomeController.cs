using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderFunding.Web.Infrastructure;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System;
using System.Threading.Tasks;
using CsvHelper;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

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
                Ukprn = ukprn,
                Total = data.TotalEarningsForCurrentAcademicYear,
                Levy = data.TotalLevyEarningsForCurrentAcademicYear,
                NonLevy = data.TotalNonLevyEarningsForCurrentAcademicYear,
                NonLevyGovernmentContribution = data.TotalNonLevyGovernmentContributionForCurrentAcademicYear,
                NonLevyEmployerContribution = data.TotalNonLevyEmployerContributionForCurrentAcademicYear
            };

            return View(model);
        }

        [Route("GenerateCSV", Name = RouteNames.GenerateCSV)] 
        public async Task<IActionResult> GenerateCSV(long ukprn)
        { 
            var data = await _service.GetDetails(ukprn);
          
           
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);
            
            using (var csvWriter = new CsvWriter(streamWriter,CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(CSVBuilder.ExportToCSV(data));
            }

            memoryStream.Position = 0;

            return File(memoryStream, "text/csv", "AcademicEarningsReport.csv");
        }


    }
}