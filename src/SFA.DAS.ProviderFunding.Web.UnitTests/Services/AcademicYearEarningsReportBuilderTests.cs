using AutoFixture;
using FluentAssertions;
using SFA.DAS.ProviderFunding.Web.Services;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class AcademicYearEarningsReportBuilderTests
    {
        private Fixture _fixture = null!;
        private IAcademicYearEarningsReportBuilder _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new AcademicYearEarningsReportBuilder();
        }

        [Test]
        public async Task WhenLearnerUlnsAreNotFoundInApprenticeshipsDataThenDoNotIncludeRowsInData()
        {
            // Arrange
            var uln1 = _fixture.Create<long>().ToString();
            var uln2 = _fixture.Create<long>().ToString();
            var uln3 = _fixture.Create<long>().ToString();
            var learner1 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln1.ToString()).Create();
            var learner2 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln2.ToString()).Create();
            var learner3 = _fixture.Build<LearnerDto>().With(x => x.Uln, uln3.ToString()).Create();
            var earnings = _fixture.Build<AcademicYearEarningsDto>().With(x => x.Learners, new List<LearnerDto>() { learner1, learner2, learner3 }).Create();
            var apprenticeships = new List<ApprenticeshipDto>() { new ApprenticeshipDto() { Uln = uln1 }, new ApprenticeshipDto() { Uln = uln3 } };

            // Act
            var actual = await _sut.BuildAsync(earnings, apprenticeships);

            // Assert
            actual.Should().HaveCount(2);
            actual.Should().NotContain(x => x.UniqueLearningNumber == uln2);
            actual.Should().Contain(x => x.UniqueLearningNumber == uln1);
            actual.Should().Contain(x => x.UniqueLearningNumber == uln3);
        }
    }
}