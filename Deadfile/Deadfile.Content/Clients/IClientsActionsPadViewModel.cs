using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Deadfile.Model;

namespace Deadfile.Content.Clients
{
    public interface IClientsActionsPadViewModel
    {
        ICommand AddClientCommand { get; }
        ICommand EditClientCommand { get; }
        ICommand SaveClientCommand { get; }
        ICommand DeleteClientCommand { get; }
        ICommand DiscardClientCommand { get; }
        Visibility AddClientVisibility { get; }
        Visibility EditClientVisibility { get; }
        Visibility SaveClientVisibility { get; }
        Visibility DiscardClientVisibility { get; }
        Visibility DeleteClientVisibility { get; }
    }
}
