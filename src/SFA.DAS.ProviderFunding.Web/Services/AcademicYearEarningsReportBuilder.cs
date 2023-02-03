﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderFunding.Web.Models;

namespace SFA.DAS.ProviderFunding.Web.Services
{
    public class AcademicYearEarningsReportBuilder : IAcademicYearEarningsReportBuilder
    {
        private readonly IAcademicYearEarningsReportDataValidator _academicYearEarningsReportDataValidator;

        public AcademicYearEarningsReportBuilder(IAcademicYearEarningsReportDataValidator academicYearEarningsReportDataValidator)
        {
            _academicYearEarningsReportDataValidator = academicYearEarningsReportDataValidator;
        }

        public async Task<List<AcademicYearEarningsReport>> BuildAsync(AcademicYearEarningsDto earningsData, IEnumerable<ApprenticeshipDto> apprenticeshipsData)
        {
            var isDataValid = await _academicYearEarningsReportDataValidator.Validate(earningsData, apprenticeshipsData);
            if (!isDataValid)
            {
                //TODO: Handle this scenario
            }

            var report = new List<AcademicYearEarningsReport>();

            var apprenticeshipsByLearner = apprenticeshipsData.ToDictionary(x => x.Uln.ToString());

            foreach (var learner in earningsData.Learners)
            {
                report.Add(new AcademicYearEarningsReport
                {
                    FamilyName = apprenticeshipsByLearner[learner.Uln].LastName,
                    GivenName = apprenticeshipsByLearner[learner.Uln].FirstName,
                    UniqueLearningNumber = learner.Uln,
                    FundingType = learner.FundingType,
                    OnProgrammeEarnings_Jan = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 1)?.Amount ?? 0,
                    OnProgrammeEarnings_Feb = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 2)?.Amount ?? 0,
                    OnProgrammeEarnings_Mar = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 3)?.Amount ?? 0,
                    OnProgrammeEarnings_Apr = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 4)?.Amount ?? 0,
                    OnProgrammeEarnings_May = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 5)?.Amount ?? 0,
                    OnProgrammeEarnings_Jun = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 6)?.Amount ?? 0,
                    OnProgrammeEarnings_Jul = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 7)?.Amount ?? 0,
                    OnProgrammeEarnings_Aug = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 8)?.Amount ?? 0,
                    OnProgrammeEarnings_Sep = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 9)?.Amount ?? 0,
                    OnProgrammeEarnings_Oct = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 10)?.Amount ?? 0,
                    OnProgrammeEarnings_Nov = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 11)?.Amount ?? 0,
                    OnProgrammeEarnings_Dec = learner.OnProgrammeEarnings.SingleOrDefault(q => q.DeliveryPeriod == 12)?.Amount ?? 0,
                    TotalOnProgrammeEarnings = learner.TotalOnProgrammeEarnings
                });
            }
            return report;
        }
    }
}