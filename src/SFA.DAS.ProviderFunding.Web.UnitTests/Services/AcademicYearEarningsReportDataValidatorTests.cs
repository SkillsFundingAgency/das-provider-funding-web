using AutoFixture;
using FluentAssertions;
using SFA.DAS.ProviderFunding.Web.Services;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class AcademicYearEarningsReportDataValidatorTests
    {
        private Fixture _fixture = null!;
        private IAcademicYearEarningsReportDataValidator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new AcademicYearEarningsReportDataValidator();
        }

        [Test]
        public async Task WhenLearnerUlnsAreNotValidNumbersThenReturnFalse()
        {
            // Arrange
            var learner1 = _fixture.Build<LearnerDto>().With(x => x.Uln, "invalid").Create();
            var learner2 = _fixture.Build<LearnerDto>().With(x => x.Uln, "10223372036854775807").Create();
            var learner3 = _fixture.Build<LearnerDto>().With(x => x.Uln, "").Create();
            var earnings = _fixture.Build<AcademicYearEarningsDto>().With(x => x.Learners, new List<LearnerDto>() { learner1, learner2, learner3 }).Create();
            var apprenticeships = _fixture.Create<List<ApprenticeshipDto>>();

            // Act
            var actual = await _sut.Validate(earnings, apprenticeships);

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public async Task WhenLearnerUlnsAreNotFoundInApprenticeshipsDataThenReturnFalse()
        {
            // Arrange
            var uln1 = _fixture.Create<long>();
            var uln2 = _fixture.Create<long>();
            var uln3 = _fixture.Create<long>();
            var learner1 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln1.ToString()).Create();
            var learner2 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln2.ToString()).Create();
            var learner3 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln3.ToString()).Create();
            var earnings = _fixture.Build<AcademicYearEarningsDto>().With(x => x.Learners, new List<LearnerDto>() { learner1, learner2, learner3 }).Create();
            var apprenticeships = new List<ApprenticeshipDto>() { new ApprenticeshipDto() { Uln = uln1}, new ApprenticeshipDto() { Uln = uln2 } };

            // Act
            var actual = await _sut.Validate(earnings, apprenticeships);

            // Assert
            actual.Should().BeFalse();
        }
    }
}
