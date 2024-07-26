using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;
using SFA.DAS.ProviderFunding.Web.Extensions;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class ProviderEarningsServiceTests
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private MockHttpMessageHandler _mockHttp = null!;
        private Fixture _fixture = null!;
        private IProviderEarningsService _sut = null!;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            var client = new HttpClient(_mockHttp);
            client.BaseAddress = new Uri(OuterApiBaseAddress);
            _mockHttpContextAccessor = GetMockIHttpContextAccessor();

            _sut = new ProviderEarningsService(client, _mockHttpContextAccessor.Object);
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
        public async Task WhenGetDetailsThenDataIsReturnedFromOuterApi()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetAcademicEarningsResponse>();

            _mockHttp.When($"{OuterApiBaseAddress}/{ukprn}/detail")
                .Respond("application/json", JsonSerializer.Serialize(expected));

            // Act
            var actual = await _sut.GetDetails(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected.AcademicYearEarnings);
        }

        private static Mock<IHttpContextAccessor> GetMockIHttpContextAccessor()
        {
            BearerTokenProvider.SetSigningKey("abcdefghijklmnopqrstuv123456789==");

            var contextMock = new Mock<HttpContext>();
            var claimsPrincipalMock = new Mock<ClaimsPrincipal>();

            // Create a list of claims for the authenticated user
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            // Add more claims as needed for testing
        };

            // Setup the ClaimsPrincipal to return the authenticated user
            claimsPrincipalMock.Setup(m => m.Identity!.IsAuthenticated).Returns(true);
            claimsPrincipalMock.Setup(m => m.Claims).Returns(claims);

            contextMock.Setup(ctx => ctx.User).Returns(claimsPrincipalMock.Object);
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            contextAccessorMock.Setup(x => x.HttpContext).Returns(contextMock.Object);
            return contextAccessorMock;
        }
    }
}
