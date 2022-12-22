using AutoFixture;
using FluentAssertions;
using RichardSzalay.MockHttp;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Text.Json;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class ProviderEarningsServiceTests
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private MockHttpMessageHandler _mockHttp = null!;
        private Fixture _fixture = null!;
        private IProviderEarningsService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            var client = new HttpClient(_mockHttp);
            client.BaseAddress = new Uri(OuterApiBaseAddress);

            _sut = new ProviderEarningsService(client);
        }

        [Test]
        public async Task WheGetSummaryThenDataFromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetProviderEarningsSummaryResponse>();

            _mockHttp.When($"{OuterApiBaseAddress}/{ukprn}/summary")
                .Respond("application/json", JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GetSummary(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected.Summary);
        }

        [Test]
        public async Task WhenGenerateCSVDataIsReturnedFromOuterApi()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetAcademicEarningsResponse>();

            _mockHttp.When($"{OuterApiBaseAddress}/{ukprn}/GenerateCSV")
                .Respond("application/json", JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GenerateCSV(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected.AcademicYearEarnings);
        }

    }
}
