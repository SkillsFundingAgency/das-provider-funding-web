using System.Collections.Generic;
using SFA.DAS.ProviderFunding.Web.Models;


namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class LearnerDto
    {
        public string Uln { get; set; }
        public FundingType FundingType { get; set; }
        public List<OnProgrammeEarning> OnProgrammeEarnings { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }
    }
}
