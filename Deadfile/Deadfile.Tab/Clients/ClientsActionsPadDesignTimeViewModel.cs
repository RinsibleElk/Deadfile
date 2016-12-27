using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Actions;

namespace Deadfile.Tab.Clients
{
    class ClientsActionsPadDesignTimeViewModel : ActionsPadDesignTimeViewModel, IClientsActionsPadViewModel
    {
        public void AddItem()
        {
            throw new NotImplementedException();
        }

        public bool CanAddItem { get; } = true;
        public bool AddItemIsVisible { get; } = true;
        public ICommand AddItemCommand { get; } = null;
    }
}
