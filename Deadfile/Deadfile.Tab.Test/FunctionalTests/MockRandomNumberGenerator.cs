using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockRandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;
        public MockRandomNumberGenerator(int seed)
        {
            _random = new Random(seed);
        }
        public int Next(int max)
        {
            return _random.Next(max);
        }
    }
}
