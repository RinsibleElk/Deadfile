using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public sealed class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random;
        public RandomNumberGenerator()
        {
            _random = new Random();
        }
        public int Next(int max)
        {
            return _random.Next(max);
        }
    }
}
