using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class CSVBuilder
    {
        public static List<AcademicYearEarningsReport> ExportToCSV(AcademicYearEarningsDto data)
        {
            foreach (var obj in data.Learners)
            {
                return new List<AcademicYearEarningsReport>
                {
                    new AcademicYearEarningsReport
                    {

                    FamilyName = "FamilyName",
                    GivenName = "GivenName",
                    UinqueLearningNumber = obj.Uln,
                    OnProgrammeEarnings_Jan = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Feb = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 2).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Mar = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 3).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Apr = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 4).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_May = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 5).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Jun = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 6).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Jul = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 7).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Aug = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 8).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Sep = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 9).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Oct = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 10).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Nov = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 11).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    OnProgrammeEarnings_Dec = obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 12).Any() ? obj.OnProgrammeEarnings.Where(q => q.DeliveryPeriod == 1).First().Amount : 0,
                    TotalOnProgrammeEarnings = obj.TotalOnProgrammeEarnings
                    }
                };
            }
            return new List<AcademicYearEarningsReport>();
        }
    }
}

