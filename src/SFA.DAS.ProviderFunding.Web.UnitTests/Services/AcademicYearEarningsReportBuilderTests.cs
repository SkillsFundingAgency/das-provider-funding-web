using AutoFixture;
using FluentAssertions;
using SFA.DAS.ProviderFunding.Web.Services;
using SFA.DAS.Testing.AutoFixture;

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

        [Test, MoqAutoData]
        public void WhenBuildingEarningsAllRecordsShouldBeIncludedInData(AcademicYearEarningsDto earnings)
        {
            // Arrange

            // Act
            var actual = _sut.Build(earnings);

            // Assert
            actual.Should().HaveCount(earnings.Learners.Count);
            actual.Should().BeEquivalentTo(earnings.Learners, opts => opts.ExcludingMissingMembers());
        }
    }
}