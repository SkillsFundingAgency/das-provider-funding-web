using AutoFixture;
using FluentAssertions;
using RichardSzalay.MockHttp;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Text.Json;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class ApprenticeshipsServiceTests
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private MockHttpMessageHandler _mockHttp = null!;
        private Fixture _fixture = null!;
        private IApprenticeshipsService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            var client = new HttpClient(_mockHttp);
            client.BaseAddress = new Uri(OuterApiBaseAddress);

            _sut = new ApprenticeshipsService(client);
        }

        [Test]
        public async Task WheGetApprenticeshipsThenDataFromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetApprenticeshipsResponse>();

            _mockHttp.When($"{OuterApiBaseAddress}/{ukprn}/apprenticeships")
                .Respond("application/json", JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GetAll(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected.Apprenticeships);
        }
    }
}
