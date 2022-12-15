namespace SFA.DAS.ProviderFunding.Web.Services;

public static class OuterApiRoutes
{
    public static class Provider
    {
        public static string GetEarningsSummary(long ukprn)
        {
            return $"{ukprn}/summary";
        }

        public static string GetAcademicYearEarningsCSV(long ukprn)
        {
            return $"{ukprn}/GenerateCSV";
        }


    }
}