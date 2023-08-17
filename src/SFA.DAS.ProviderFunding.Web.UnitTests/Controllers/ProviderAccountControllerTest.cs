using AutoFixture;
using Moq;
using SFA.DAS.ProviderFunding.Web.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FluentAssertions;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Controllers
{
    public class ProviderAccountControllerTest
    {
        private Fixture _fixture;
        private ProviderAccountController _sut;
        private Mock<IConfiguration>? _configuration;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _configuration = new Mock<IConfiguration>();
            _sut = new ProviderAccountController(_configuration.Object);
        }

        [Test]
        public void When_Dashboard_Called_Redirect()
        {
            // Arrange
            var url = _fixture.Create<string>();
            
            _configuration?.SetupGet(x => x[It.Is<string>(s => s == "ProviderSharedUIConfiguration:DashboardUrl")]).Returns(url);

            // Act
            var result = (RedirectResult) _sut.Dashboard();
            var actual = result;

            // Assert
            Debug.Assert(actual != null, nameof(actual) + " != null");
            actual.Url.Should().Be(url);
        }
    }
}
