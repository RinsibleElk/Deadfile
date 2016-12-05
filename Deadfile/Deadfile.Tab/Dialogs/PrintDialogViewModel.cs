using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;

namespace Deadfile.Tab.Dialogs
{
    class PrintDialogViewModel : IPrintDialogViewModel
    {
        private readonly object _context;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly DelegateCommand<PrintDialogView> _closeCommand;
        private readonly DelegateCommand _printCommand;
        private readonly ObservableCollection<string> _printers;
        public PrintDialogViewModel(object context, IDialogCoordinator dialogCoordinator)
        {
            _context = context;
            _dialogCoordinator = dialogCoordinator;
            _closeCommand = new DelegateCommand<PrintDialogView>(CloseAction);
            _printCommand = new DelegateCommand(PerformPrint);
            _printers = new ObservableCollection<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                _printers.Add(printer);
            }
        }

        public ObservableCollection<string> Printers
        {
            get { return _printers; }
        }

        public string SelectedPrinter { get; set; } = null;

        private void PerformPrint()
        {
            throw new NotImplementedException();
        }

        private void CloseAction(PrintDialogView dialog)
        {
            _dialogCoordinator.HideMetroDialogAsync(_context, dialog);
        }

        public ICommand CloseCommand { get { return _closeCommand; } }
        public ICommand PrintCommand { get { return _printCommand; } }
    }
}
