using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.ProviderFunding.Infrastructure.Enums;
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
        public async Task Then_The_Provider_Is_Valid_And_True_Returned(
            long ukprn,
            GetProviderResponseItem apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.ProviderTypeId = (int)ProviderTypeIdentifier.MainProvider;
            apiResponse.StatusId = (int)ProviderStatusType.Active;
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            }, "TestAuthType"));
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(principal);
            trainingProviderService
                .Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Provider_Is_InValid_And_False_Returned(
            long ukprn,
            GetProviderResponseItem apiResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            apiResponse.ProviderTypeId = (int)ProviderTypeIdentifier.EPAO;
            apiResponse.StatusId = (int)ProviderStatusType.ActiveButNotTakingOnApprentices;
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            }, "TestAuthType"));
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(principal);
            trainingProviderService
                .Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Provider_Is_NotFound_And_False_Returned(
            long ukprn,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            AuthorizationHandlerContext context,
            TrainingProviderAuthorizationHandler handler)
        {
            //Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new(ProviderClaims.ProviderUkprn, ukprn.ToString()),
            }, "TestAuthType"));
            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(principal);
            trainingProviderService
                .Setup(x => x.GetProviderDetails(ukprn)).ReturnsAsync((GetProviderResponseItem)null);

            //Act
            var actual = await handler.IsProviderAuthorized(context, true);

            //Assert
            actual.Should().BeFalse();
        }
    }
}
