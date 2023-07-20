using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.DfESignIn.Auth.Interfaces;
using SFA.DAS.ProviderFunding.Web.Infrastructure.Authorization;

namespace SFA.DAS.ProviderFunding.Web.AppStart;

public class CustomServiceRole : ICustomServiceRole
{
    public string RoleClaimType => ProviderClaims.Service;

    // <inherit-doc/>
    public CustomServiceRoleValueType RoleValueType => CustomServiceRoleValueType.Code;
}