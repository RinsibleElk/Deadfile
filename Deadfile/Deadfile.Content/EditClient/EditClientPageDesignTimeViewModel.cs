using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Navigation;
using Deadfile.Model;

namespace Deadfile.Content.EditClient
{
    class EditClientPageDesignTimeViewModel : IEditClientPageViewModel
    {
        public EditClientPageDesignTimeViewModel()
        {
            Title = "New Client Design Time";
            ClientModelUnderEdit = new ClientModel();
        }

        public Experience Experience { get { return Experience.EditClient; } }

        public string Title { get; set; }

        public ClientModel ClientModelUnderEdit { get; set; }
    }
}
