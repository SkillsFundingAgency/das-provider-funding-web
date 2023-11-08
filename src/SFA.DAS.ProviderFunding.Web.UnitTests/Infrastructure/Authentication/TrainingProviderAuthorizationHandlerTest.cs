using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
            ProviderAccountResponse apiResponse,
            [Frozen] Mock<ITrainingProviderService> outerApiService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            TrainingProviderAllRolesRequirement requirement,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessService = true;
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            var context = new AuthorizationHandlerContext(new[] { requirement }, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);


            outerApiService.Setup(x => x.CanProviderAccessService(ukprn)).ReturnsAsync(apiResponse.CanAccessService);

            //Act
            var actual = await handler.IsProviderAuthorized(context);

            //Assert
            actual.Should().BeTrue();
        }


        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Is_InValid_And_False_Returned(
            long ukprn,
            ProviderAccountResponse apiResponse,
            [Frozen] Mock<ITrainingProviderService> outerApiService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            TrainingProviderAllRolesRequirement requirement,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.CanAccessService = false;
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
            var context = new AuthorizationHandlerContext(new[] { requirement }, claimsPrinciple, null);
            var responseMock = new FeatureCollection();
            var httpContext = new DefaultHttpContext(responseMock);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            outerApiService.Setup(x => x.CanProviderAccessService(ukprn)).ReturnsAsync(apiResponse.CanAccessService);

            //Act
            var actual = await handler.IsProviderAuthorized(context);

            //Assert
            actual.Should().BeFalse();
        }
    }
}
