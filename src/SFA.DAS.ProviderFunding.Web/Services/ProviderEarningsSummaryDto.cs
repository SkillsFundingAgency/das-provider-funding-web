namespace SFA.DAS.ProviderFunding.Web.Services;

public class ProviderEarningsSummaryDto
{
    public decimal TotalEarningsForCurrentAcademicYear { get; set; }
    public decimal TotalLevyEarningsForCurrentAcademicYear { get; set; }
    public decimal TotalNonLevyEarningsForCurrentAcademicYear { get; set; }
    public decimal TotalNonLevyGovernmentContributionForCurrentAcademicYear { get; set; }
    public decimal TotalNonLevyEmployerContributionForCurrentAcademicYear { get; set; }
}