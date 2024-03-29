﻿using AutoFixture;
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
        private Mock<IAcademicYearEarningsReportBuilder> _academicYearEarningsReportBuilderMock;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _providerEarningsServiceMock = new Mock<IProviderEarningsService>();
            _academicYearEarningsReportBuilderMock = new Mock<IAcademicYearEarningsReportBuilder>();

            _sut = new HomeController(
                _providerEarningsServiceMock.Object,
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
            var expectedReports = _fixture.Create<List<AcademicYearEarningsReport>>();

            _providerEarningsServiceMock.Setup(_ => _.GetDetails(ukprn)).ReturnsAsync(expectedEarningsData);
            _academicYearEarningsReportBuilderMock.Setup(_ => _.Build(expectedEarningsData)).Returns(expectedReports);

            // Act
            var result = await _sut.GenerateCSV(ukprn);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<FileStreamResult>();
        }
    }
}