using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authentication;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Infrastructure.Authentication
{
    public class TrainingProviderAuthorizationHandlerTest
    {
        [Test, MoqAutoData]
        public async Task Then_The_ProviderStatus_Is_Valid_And_True_Returned(
            long ukprn,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var claims = new List<Claim>
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContextAccessor.Setup(x => x.HttpContext!.User).Returns(claimsPrincipal);
            trainingProviderService.Setup(x => x.CanProviderAccessService(ukprn)).ReturnsAsync(true);

            //Act
            var actual = await handler.IsProviderAuthorized(context);

            //Assert
            actual.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_InValid_And_False_Returned(
            long ukprn,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var claims = new List<Claim>
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContextAccessor.Setup(x => x.HttpContext!.User).Returns(claimsPrincipal);
            trainingProviderService.Setup(x => x.CanProviderAccessService(ukprn)).ReturnsAsync(false);

            //Act
            var actual = await handler.IsProviderAuthorized(context);

            //Assert
            actual.Should().BeFalse();
        }
    }
}
