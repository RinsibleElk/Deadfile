using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    /// <summary>
    /// Repository for accessing data from the DeadfileContext.
    /// </summary>
    public interface IDeadfileRepository
    {
        /// <summary>
        /// Synchronous for now.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClientModel> GetClients();
    }
}
