using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFunding.Web.Controllers;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Controllers
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ErrorController();
        }

        [Test]
        public void WhenStatusCodeIs403Then403ViewIsReturned()
        {
            var result = (ViewResult)_sut.Error(403);
            result.ViewName.Should().Be("403");
        }

        [Test]
        public void WhenStatusCodeIs404Then404ViewIsReturned()
        {
            var result = (ViewResult)_sut.Error(404);
            result.ViewName.Should().Be("404");
        }
        
        [TestCase(null)]
        [TestCase(401)]
        [TestCase(405)]
        public void WhenStatusCodeIsNotHandledThenGenericErrorViewIsReturned(int? errorCode)
        {
            var result = (ViewResult)_sut.Error(errorCode);
            result.ViewName.Should().BeNull();
        }
    }
}
