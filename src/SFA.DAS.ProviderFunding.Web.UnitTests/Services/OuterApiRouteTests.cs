using FluentAssertions;
using SFA.DAS.ProviderFunding.Web.Services;
using System.Reflection;

namespace SFA.DAS.ProviderFunding.Web.Tests.Services
{
    public class OuterApiRouteTests
    {
        [Test]
        public void NoOuterApiRoutesArePrefixedWithSlash()
        {
            var types = typeof(OuterApiRoutes).GetNestedTypes();
            types.Length.Should().BeGreaterOrEqualTo(1);

            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

                methods.Length.Should().BeGreaterOrEqualTo(1);

                foreach (var method in methods)
                {
                    var parameterInfos = method.GetParameters();
                    var parameters = new object?[parameterInfos.Length];
                    for (var i = 0; i < parameterInfos.Length; i++)
                    {
                        parameters[i] = GetDefaultValue(parameterInfos[i].GetType());
                    }

                    var result = method.Invoke(null, parameters) as string;
                    result.Should().NotBeNullOrEmpty();
                    result.Should().NotStartWith("/");
                }
            }
        }

        private static object? GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}