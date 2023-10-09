using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SFA.DAS.ProviderFunding.Web.Controllers;
using SFA.DAS.ProviderFunding.Web.Models.Error;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Controllers
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController _sut;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _sut = new ErrorController(_mockConfiguration.Object);
        }

        [Test]
        [TestCase("test", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", true)]
        [TestCase("pp", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", true)]
        [TestCase("local", "https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service", false)]
        [TestCase("prd", "https://services.signin.education.gov.uk/approvals/select-organisation?action=request-service", false)]
        public void WhenStatusCodeIs403Then403ViewIsReturned(string env, string helpLink, bool useDfESignIn)
        {
            //arrange
            _mockConfiguration.Setup(x => x["ResourceEnvironmentName"]).Returns(env);
            _mockConfiguration.Setup(x => x["UseDfESignIn"]).Returns(Convert.ToString(useDfESignIn));

            var result = (ViewResult)_sut.Error(403);

            Assert.That(result, Is.Not.Null);
            var actualModel = result?.Model as Error403ViewModel;
            Assert.That(actualModel?.HelpPageLink, Is.EqualTo(helpLink));
            Assert.That(actualModel?.UseDfESignIn, Is.EqualTo(useDfESignIn));
        }

        [Test]
        public void WhenStatusCodeIs404Then404ViewIsReturned()
        {
            var result = (ViewResult)_sut.Error(404);
            result.ViewName.Should().Be("404");
        }
        
        [TestCase(null)]
        [TestCase(401)]
        [TestCase(503)]
        [TestCase(405)]
        public void WhenStatusCodeIsNotHandledThenGenericErrorViewIsReturned(int? errorCode)
        {
            var result = (ViewResult)_sut.Error(errorCode);
            result.ViewName.Should().BeNull();
        }
    }
}
