using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ProviderFunding.Web.Extensions;

namespace SFA.DAS.ProviderFunding.Web.Middleware
{
    public class MinimumServiceClaimRequirementHandler : AuthorizationHandler<MinimumServiceClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumServiceClaimRequirement requirement)
        {
            if(context.User.HasPermission(requirement.MinimumServiceClaim)) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}