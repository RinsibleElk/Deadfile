using System;
using System.Linq;
using Deadfile.Content.Navigation;
using Prism.Regions;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestNavigationParameterMapper
    {
        [Fact]
        public void TestConvertFromInt()
        {
            var navigationParameterMapper = new NavigationParameterMapper();
            var navigationParameters = navigationParameterMapper.ConvertToNavigationParameters(4);
            var actualString = navigationParameters.ToString();
            var expectedString = "?Value=4";
            Assert.Equal(expectedString, actualString);
        }

        [Fact]
        public void TestConvertToInt()
        {
            var navigationParameterMapper = new NavigationParameterMapper();
            var navigationParameters = new NavigationParameters();
            var expectedValue = 7;
            navigationParameters.Add("Value", expectedValue);
            var actualValue = navigationParameterMapper.ConvertToUserType<int>(navigationParameters);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
