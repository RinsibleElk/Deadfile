using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Clients;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Navigation parameters for the <see cref="ClientsPageViewModel"/>.
    /// </summary>
    struct ClientNavigationKey
    {
        /// <summary>
        /// Navigate to the <see cref="ClientsPageViewModel"/> for the specified client. The client may not exist, this will be specified by the
        /// <see cref="ClientId"/> being <see cref="ModelBase.NewModelId"/>
        /// </summary>
        /// <param name="clientId"></param>
        public ClientNavigationKey(int clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// The client for this job.
        /// </summary>
        public int ClientId { get; }
    }
}
