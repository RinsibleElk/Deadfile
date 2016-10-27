using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Deadfile.Content.ActionsPad;
using Deadfile.Model;

namespace Deadfile.Content.Clients
{
    interface IClientsActionsPadViewModel : IActionsPadViewModel
    {
        ICommand AddItemCommand { get; }
        Visibility AddItemVisibility { get; }
    }
}
