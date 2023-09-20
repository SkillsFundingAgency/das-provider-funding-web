using AutoFixture;
using FluentAssertions;
using RichardSzalay.MockHttp;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Text.Json;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Services
{
    public class TrainingProviderServiceTest
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private MockHttpMessageHandler _mockHttp = null!;
        private Fixture _fixture = null!;
        private ITrainingProviderService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            var client = new HttpClient(_mockHttp)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            };
            _sut = new TrainingProviderService(client);
        }

        [Test]
        public async Task WhenGetProviderDetailsThenDataFromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetProviderResponseItem>();

            _mockHttp.When($"{OuterApiBaseAddress}/providers/{ukprn}")
                .Respond("application/json", JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
