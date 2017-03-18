using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockNavigationContainer : INavigationContainer
    {
        private readonly IReadOnlyDictionary<string, object> _viewModels;

        public MockNavigationContainer(IReadOnlyDictionary<string, object> viewModels)
        {
            _viewModels = viewModels;
        }

        public object GetInstance(string key)
        {
            return _viewModels[key];
        }
    }
}
