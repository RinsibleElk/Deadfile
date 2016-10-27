using Deadfile.Content.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.DesignTime;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Deadfile.Content.ActionsPad;

namespace Deadfile.Content.Clients
{
    /// <summary>
    /// Clients page is simple so just straightforward override of the regular actions pad.
    /// </summary>
    sealed class ClientsActionsPadDesignTimeViewModel : ActionsPadDesignTimeViewModel, IClientsActionsPadViewModel
    {
        public ICommand AddItemCommand { get; } = null;
        public Visibility AddItemVisibility { get; } = Visibility.Visible;
    }
}
