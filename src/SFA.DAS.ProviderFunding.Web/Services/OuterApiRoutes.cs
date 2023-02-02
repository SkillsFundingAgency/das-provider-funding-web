namespace SFA.DAS.ProviderFunding.Web.Services;

public static class OuterApiRoutes
{
    //TODO: Add route for apprenticeships API
    public static class Provider
    {
        public static string GetEarningsSummary(long ukprn)
        {
            return $"{ukprn}/summary";
        }

        public static string GetAcademicYearEarnings(long ukprn)
        {
            return $"{ukprn}/detail";
        }
    }
}