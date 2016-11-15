using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Jobs;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Navigation parameters for the <see cref="JobsPageViewModel"/>.
    /// </summary>
    struct ClientAndJob
    {
        /// <summary>
        /// Navigate to the <see cref="JobsPageViewModel"/> for the specified client and job. The job may not exist, this will be specified by the
        /// <see cref="JobId"/> being <see cref="ModelBase.NewModelId"/>.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="jobId"></param>
        public ClientAndJob(int clientId, int jobId)
        {
            ClientId = clientId;
            JobId = jobId;
        }

        /// <summary>
        /// The client for this job.
        /// </summary>
        public int ClientId { get; }

        /// <summary>
        /// Uniquely specifies the job. May be NewModelId if this is for creation.
        /// </summary>
        public int JobId { get; }
    }
}
