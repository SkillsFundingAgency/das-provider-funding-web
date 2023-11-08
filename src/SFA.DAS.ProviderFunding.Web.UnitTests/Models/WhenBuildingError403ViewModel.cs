using SFA.DAS.ProviderFunding.Web.Models.Error;

namespace SFA.DAS.ProviderFunding.Web.UnitTests.Models
{
    public class WhenBuildingError403ViewModel
    {
        [Test]
        public void Then_The_HelpPage_Link_Is_Correct_For_Production_Environment()
        {
            var actual = new Error403ViewModel("prd");

            Assert.That(actual.HelpPageLink, Is.Not.Null);
            Assert.That(actual.HelpPageLink, Is.EqualTo("https://services.signin.education.gov.uk/approvals/select-organisation?action=request-service"));
        }
        [Test]
        public void Then_The_HelpPage_Link_Is_Correct_For_Non_Production_Environment()
        {
            var actual = new Error403ViewModel("test");

            Assert.That(actual.HelpPageLink, Is.Not.Null);
            Assert.That(actual.HelpPageLink, Is.EqualTo("https://test-services.signin.education.gov.uk/approvals/select-organisation?action=request-service"));
        }
    }
}
