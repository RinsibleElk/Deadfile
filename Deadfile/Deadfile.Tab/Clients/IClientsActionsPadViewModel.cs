using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Actions;

namespace Deadfile.Tab.Clients
{
    interface IClientsActionsPadViewModel : IActionsPadViewModel
    {
        void AddItem();
        bool CanAddItem { get; }
        bool AddItemIsVisible { get; }
    }
}
