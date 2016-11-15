using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Invoices;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Navigation parameters for the <see cref="InvoicesPageViewModel"/>.
    /// </summary>
    struct ClientAndInvoice
    {
        /// <summary>
        /// Navigate to the <see cref="InvoicesPageViewModel"/> for the specified client and invoice. The invoice may not exist, this will be specified by the
        /// <see cref="InvoiceId"/> being <see cref="ModelBase.NewModelId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="invoiceId"></param>
        public ClientAndInvoice(int clientId, int invoiceId)
        {
            ClientId = clientId;
            InvoiceId = invoiceId;
        }

        /// <summary>
        /// The client for this job.
        /// </summary>
        public int ClientId { get; }

        /// <summary>
        /// Uniquely specifies the invoice. May be NewModelId if this is for creation.
        /// </summary>
        public int InvoiceId { get; }
    }
}
