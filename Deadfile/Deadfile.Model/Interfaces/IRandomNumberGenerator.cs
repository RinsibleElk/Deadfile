using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Interfaces
{
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Get a non-negative random integer strictly less than the value of <see cref="max"/>.
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        int Next(int max);
    }
}
