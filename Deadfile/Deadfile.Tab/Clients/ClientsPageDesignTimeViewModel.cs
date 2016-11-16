using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.DesignTime;
using Deadfile.Tab.Events;

namespace Deadfile.Tab.Clients
{
    class ClientsPageDesignTimeViewModel : PageDesignTimeViewModel, IClientsPageViewModel
    {
        public void AddNewJob()
        {
            throw new NotImplementedException();
        }
        public void InvoiceClient()
        {
            throw new NotImplementedException();
        }
        public bool CanAddNewJob { get; } = true;
        public bool CanInvoiceClient { get; } = true;
        public ClientModel SelectedItem { get; set; } = new ClientModel() {FirstName = "Oliver", LastName = "Samson"};
        public bool Editable { get; } = true;
        public List<string> Errors { get; } = new List<string>();
        public Experience Experience
        {
            get { return Experience.Clients; }
        }

        public bool ShowActionsPad { get; } = true;
    }
}
