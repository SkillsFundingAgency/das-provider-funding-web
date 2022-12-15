using System.Collections.Generic;


namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class LearnerDto
    {
        public string Uln { get; set; }
        //ask tom how to import the funding type dll
        //public FundingType FundingType { get; set; }
        public List<OnProgrammeEarning> OnProgrammeEarnings { get; set; }
        public decimal TotalOnProgrammeEarnings { get; set; }
    }
}
