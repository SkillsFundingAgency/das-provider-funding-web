namespace SFA.DAS.ProviderFunding.Web.Extension
{
    public static class PriceExtensions
    {
        public static string ToGdsMoneyFormat(this decimal value)
        {
            return $"£{value:n2}";
        }
    }
}
