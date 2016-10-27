using System;
using System.Linq;
using Deadfile.Content.Navigation;
using Prism.Regions;
using Xunit;

namespace Deadfile.Content.Test
{
    public enum TestInfo
    {
        Blah
    }
    public class TestClass
    {
        public int Foo { get; set; }
        public double Bar { get; set; }
        public string Baz { get; set; }
        public TestInfo Cad { get; set; }
    }

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

        [Fact]
        public void TestConvertFromClass()
        {
            var navigationParameterMapper = new NavigationParameterMapper();
            var navigationParameters =
                navigationParameterMapper.ConvertToNavigationParameters(new TestClass()
                {
                    Bar = 4.0,
                    Baz = "Something",
                    Foo = -4,
                    Cad = TestInfo.Blah
                });
            var actualString = navigationParameters.ToString();
            var expectedString = "?Foo=-4&Bar=4&Baz=Something&Cad=Blah";
            Assert.Equal(expectedString, actualString);
        }

        [Fact]
        public void TestConvertToClass()
        {
            var navigationParameterMapper = new NavigationParameterMapper();
            var expectedValue = new TestClass()
            {
                Bar = 4.0,
                Baz = "Something",
                Foo = -4,
                Cad = TestInfo.Blah
            };
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("Foo", -4);
            navigationParameters.Add("Bar", 4.0);
            navigationParameters.Add("Baz", "Something");
            navigationParameters.Add("Cad", TestInfo.Blah);
            var actualValue = navigationParameterMapper.ConvertToUserType<TestClass>(navigationParameters);
            Assert.Equal(expectedValue.Foo, actualValue.Foo);
            Assert.Equal(expectedValue.Bar, actualValue.Bar);
            Assert.Equal(expectedValue.Baz, actualValue.Baz);
            Assert.Equal(expectedValue.Cad, actualValue.Cad);
        }
    }
}
