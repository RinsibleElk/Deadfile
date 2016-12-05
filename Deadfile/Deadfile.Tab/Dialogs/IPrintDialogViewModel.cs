using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Tab.Dialogs
{
    interface IPrintDialogViewModel
    {
        ICommand CloseCommand { get; }
        ICommand PrintCommand { get; }
        ObservableCollection<string> Printers { get; }
        string SelectedPrinter { get; set; }
    }
}
