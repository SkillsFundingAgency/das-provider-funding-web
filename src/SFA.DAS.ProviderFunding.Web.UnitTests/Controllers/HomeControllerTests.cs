using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderFunding.Web.Controllers;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Diagnostics;

namespace SFA.DAS.ProviderFunding.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        private Fixture _fixture = null!;
        private HomeController _sut = null!;
        private Mock<IProviderEarningsService> _serviceMock = null!;
        
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _serviceMock = new Mock<IProviderEarningsService>();

            _sut = new HomeController(_serviceMock.Object);
        }

        [Test]
        public async Task WheGetSummaryThenDataFromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<ProviderEarningsSummaryDto>();

            _serviceMock.Setup(_ => _.GetSummary(ukprn)).ReturnsAsync(expected);

            // Act
            var result = (ViewResult) await _sut.Index(ukprn);
            var actual = result.Model as IndicativeEarningsReportViewModel;

            // Assert
            Debug.Assert(actual != null, nameof(actual) + " != null");
            actual.Total.Should().Be(expected.TotalEarningsForCurrentAcademicYear);
            actual.Levy.Should().Be(expected.TotalLevyEarningsForCurrentAcademicYear);
            actual.NonLevy.Should().Be(expected.TotalNonLevyEarningsForCurrentAcademicYear);
            actual.NonLevyEmployerContribution.Should().Be(expected.TotalNonLevyEarningsForCurrentAcademicYearEmployer);
            actual.NonLevyGovernmentContribution.Should().Be(expected.TotalNonLevyEarningsForCurrentAcademicYearGovernment);
        }


        [Test]
        public async Task WhenGenerateCSVDataIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<AcademicYearEarningsDto>();

            _serviceMock.Setup(_ => _.GetDetails(ukprn)).ReturnsAsync(expected);

            // Act
            var result = (FileStreamResult)await _sut.GenerateCSV(ukprn);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<FileStreamResult>(result);
        }
    }
}
