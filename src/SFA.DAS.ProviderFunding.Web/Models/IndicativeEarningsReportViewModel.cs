namespace SFA.DAS.ProviderFunding.Web.Models;

public class IndicativeEarningsReportViewModel
{
  public long Ukprn { get; set; }
    public decimal Total { get; set; }
    public decimal Levy { get; set; }
    public decimal NonLevy { get; set; }
    public decimal NonLevyGovernmentContribution { get; set; }
    public decimal NonLevyEmployerContribution { get; set; }
}