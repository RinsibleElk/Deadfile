using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Clients
{
    interface IClientsPageViewModel : IEditableItemViewModel<ClientModel>, IPageViewModel
    {
        void AddNewJob();
        bool CanAddNewJob { get; }
        void InvoiceClient();
        bool CanInvoiceClient { get; }
        void EmailClient();
        bool CanEmailClient { get; }
    }
}
