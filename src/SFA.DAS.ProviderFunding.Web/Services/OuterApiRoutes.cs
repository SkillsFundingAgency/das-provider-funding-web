namespace SFA.DAS.ProviderFunding.Web.Services;

public static class OuterApiRoutes
{
    public static class Provider
    {
        public static string GetEarningsSummary(long ukprn)
        {
            return $"{ukprn}/summary";
        }
    }
}