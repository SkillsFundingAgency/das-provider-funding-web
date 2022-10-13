using System.Security.Claims;
using SFA.DAS.ProviderFunding.Web.Configuration;

namespace SFA.DAS.ProviderFunding.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderRecruitClaims.IdamsUserDisplayNameClaimTypeIdentifier);
        }

        public static string GetEmailAddress(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderRecruitClaims.IdamsUserEmailClaimTypeIdentifier);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderRecruitClaims.IdamsUserNameClaimTypeIdentifier);
        }

        public static long GetUkprn(this ClaimsPrincipal user)
        {
            var ukprn = user.FindFirstValue(ProviderRecruitClaims.IdamsUserUkprnClaimsTypeIdentifier);

            return long.Parse(ukprn);
        }

        public static VacancyUser ToVacancyUser(this ClaimsPrincipal user)
        {
            return new VacancyUser
            {
                UserId = user.GetUserName(),
                Name = user.GetDisplayName(),
                Email = user.GetEmailAddress(),
                Ukprn = user.GetUkprn()
            };
        }
    }
}