using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderFunding.Web.Controllers;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Diagnostics;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private Fixture _fixture;
        private HomeController _sut;
        private Mock<IProviderEarningsService> _providerEarningsServiceMock;
        private Mock<IApprenticeshipsService> _apprenticeshipsServiceMock;
        private Mock<IAcademicYearEarningsReportBuilder> _academicYearEarningsReportBuilderMock;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _providerEarningsServiceMock = new Mock<IProviderEarningsService>();
            _apprenticeshipsServiceMock = new Mock<IApprenticeshipsService>();
            _academicYearEarningsReportBuilderMock = new Mock<IAcademicYearEarningsReportBuilder>();

            _sut = new HomeController(
                _providerEarningsServiceMock.Object,
                _apprenticeshipsServiceMock.Object,
                _academicYearEarningsReportBuilderMock.Object);
        }

        [Test]
        public async Task WheGetSummaryThenDataFromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<ProviderEarningsSummaryDto>();

            _providerEarningsServiceMock.Setup(_ => _.GetSummary(ukprn)).ReturnsAsync(expected);

            // Act
            var result = (ViewResult)await _sut.Index(ukprn);
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
            var expectedEarningsData = _fixture.Create<AcademicYearEarningsDto>();
            var expectedApprenticeshipsData = _fixture.Create<IEnumerable<ApprenticeshipDto>>();
            var expectedReports = _fixture.Create<List<AcademicYearEarningsReport>>();

            _providerEarningsServiceMock.Setup(_ => _.GetDetails(ukprn)).ReturnsAsync(expectedEarningsData);
            _apprenticeshipsServiceMock.Setup(_ => _.GetAll(ukprn)).ReturnsAsync(expectedApprenticeshipsData);
            _academicYearEarningsReportBuilderMock.Setup(_ => _.Build(expectedEarningsData, expectedApprenticeshipsData)).Returns(expectedReports);

            // Act
            var result = await _sut.GenerateCSV(ukprn);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<FileStreamResult>(result);
        }
    }
}