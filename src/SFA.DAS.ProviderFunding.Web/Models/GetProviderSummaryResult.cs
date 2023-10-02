namespace SFA.DAS.ProviderFunding.Web.Models
{
    public class GetProviderSummaryResult
    {
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactUrl { get; set; }
        public int ProviderTypeId { get; set; }
        public int StatusId { get; set; }
        public bool CanAccessApprenticeshipService { get; set; }
        public ProviderAddress Address { get; set; }
    }

    public class ProviderAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
