namespace SFA.DAS.ProviderFunding.Web.Services;

public static class OuterApiRoutes
{
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

        public static string GetProviderDetails(long ukprn)
        {
            return $"api/providers/{ukprn}";
        }
    }
}