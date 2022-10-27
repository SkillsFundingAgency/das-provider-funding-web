using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderFunding.Web.Extensions
{
    public static class BuildModelPropertyOrderDictionary
    {
        public static Dictionary<string, int> BuildPropertyOrderDictionary<T>(this T model)
        {
            var itemCount = 0;
            var propertyOrderDictionary = model.GetType().GetProperties().Select(c => new
            {
                Order = itemCount++,
                c.Name
            }).ToDictionary(key => key.Name, value => value.Order);
            return propertyOrderDictionary;
        }
    }
}