using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Protected;
using SFA.DAS.ProviderFunding.Web.Models;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Net;
using System.Text.Json;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Services
{
    public class TrainingProviderServiceTest
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private Mock<HttpMessageHandler> _mockHttpsMessageHandler = null!;
        private Fixture _fixture = null!;
        private TrainingProviderService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttpsMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Test]
        public async Task When_ProviderDetails_Found_Then_Data_FromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetProviderSummaryResult>();

            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expected)),
                    RequestMessage = new HttpRequestMessage()
                });
            var httpClient = new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            };
            _sut = new TrainingProviderService(httpClient);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task When_ProviderDetails_NotFound_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(""),
                    RequestMessage = new HttpRequestMessage()
                });
            var httpClient = new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            };
            _sut = new TrainingProviderService(httpClient);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }

        [Test]
        public async Task When_ProviderDetails_InternalServerError_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();

            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(""),
                    RequestMessage = new HttpRequestMessage()
                });
            var httpClient = new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            };
            _sut = new TrainingProviderService(httpClient);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }
    }
}
