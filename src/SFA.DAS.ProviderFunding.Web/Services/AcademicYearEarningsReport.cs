using SFA.DAS.ProviderFunding.Web.Models;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class AcademicYearEarningsReport
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string UinqueLearningNumber { get; set; }
        public FundingType FundingType { get; set; }
        public decimal OnProgrammeEarnings_Jan { get; set; }
        public decimal OnProgrammeEarnings_Feb { get; set; }
        public decimal OnProgrammeEarnings_Mar { get; set; }
        public decimal OnProgrammeEarnings_Apr { get; set; }
        public decimal OnProgrammeEarnings_May { get; set; }
        public decimal OnProgrammeEarnings_Jun { get; set; }
        public decimal OnProgrammeEarnings_Jul { get; set; }
        public decimal OnProgrammeEarnings_Aug { get; set; }
        public decimal OnProgrammeEarnings_Sep { get; set; }
        public decimal OnProgrammeEarnings_Oct { get; set; }
        public decimal OnProgrammeEarnings_Nov { get; set; }
        public decimal OnProgrammeEarnings_Dec { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }

    }
}
