using SFA.DAS.ProviderFunding.Web.Extension;

namespace SFA.DAS.ProviderFunding.Web.Tests
{
    public class PriceExtensionsTests
    {
        [Test]
        public void ToGdsMoneyAmount_FormatsDecimalValue()
        {
            const string expected = "£1,234,567.89";
            var actual = (1234567.89m).ToGdsMoneyFormat();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}