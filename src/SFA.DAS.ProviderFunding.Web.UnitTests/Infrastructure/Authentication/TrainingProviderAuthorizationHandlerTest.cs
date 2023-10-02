using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authentication;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;
using SFA.DAS.ProviderFunding.Web.Models;
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
            GetProviderSummaryResult apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessApprenticeshipService = true;
            var claims = new List<Claim>
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContextAccessor.Setup(x => x.HttpContext!.User).Returns(claimsPrincipal);
            trainingProviderService.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_InValid_And_False_Returned(
            long ukprn,
            GetProviderSummaryResult apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessApprenticeshipService = false;
            var claims = new List<Claim>
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            httpContextAccessor.Setup(x => x.HttpContext!.User).Returns(claimsPrincipal);
            trainingProviderService.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_Null_And_False_Returned(
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
            trainingProviderService.Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync((GetProviderSummaryResult)null!);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }
    }
}
