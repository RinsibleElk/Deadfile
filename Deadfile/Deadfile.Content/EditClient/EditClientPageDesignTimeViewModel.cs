using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Navigation;

namespace Deadfile.Content.EditClient
{
    class EditClientPageDesignTimeViewModel : IEditClientPageViewModel
    {
        public EditClientPageDesignTimeViewModel()
        {
            Title = "New Client Design Time";
        }

        public Experience Experience { get { return Experience.EditClient; } }

        public string Title { get; set; }
    }
}
