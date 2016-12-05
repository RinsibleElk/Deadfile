using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.Tab.Dialogs
{
    class PrintDialogDesignTimeViewModel : IPrintDialogViewModel
    {
        public ICommand CloseCommand { get; } = null;
        public ICommand PrintCommand { get; } = null;
        public ObservableCollection<string> Printers { get; } = new ObservableCollection<string>();
        public string SelectedPrinter { get; set; } = null;
    }
}
