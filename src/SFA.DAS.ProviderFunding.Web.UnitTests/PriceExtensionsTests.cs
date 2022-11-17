using System.Web;
using SFA.DAS.ProviderFunding.Web.Extension;

namespace SFA.DAS.ProviderFunding.Web.UnitTests
{
    public class PriceExtensionsTests
    {
        [Test]
        public void ToGdsMoneyAmount_FormatsDecimalValue()
        {
            const string expected = "&#163;1,234,567.89"; // £163;1,234,567.89
            var actual = HttpUtility.HtmlEncode((1234567.89m).ToGdsMoneyFormat());

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}