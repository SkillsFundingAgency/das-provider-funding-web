using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class AcademicYearEarningsReportDataValidator : IAcademicYearEarningsReportDataValidator
    {
        public async Task<bool> Validate(AcademicYearEarningsDto earningsData, IEnumerable<ApprenticeshipDto> apprenticeshipsData)
        {
            var apprenticeshipsByLearner = apprenticeshipsData.ToDictionary(x => x.Uln);
            foreach (var learner in earningsData.Learners)
            {
                if (!long.TryParse(learner.Uln, out var uln))
                {
                    //TODO: Handle... logging?
                    return false;
                }

                if (!apprenticeshipsByLearner.TryGetValue(uln, out var apprenticeship))
                {
                    //TODO: Handle... logging?
                    return false;
                }
            }
            return true;
        }
    }
}

