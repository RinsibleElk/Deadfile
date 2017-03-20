using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model
{
    public class DeadfileContextAbstractionFactory : IDeadfileContextAbstractionFactory
    {
        public IDeadfileContextAbstraction GetAbstraction()
        {
            return new DeadfileContextAbstraction();
        }
    }
}
