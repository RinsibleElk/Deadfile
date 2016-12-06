using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Invoices;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Prism.Commands;

namespace Deadfile.Tab.Dialogs
{
    class PrintDialogViewModel : IPrintDialogViewModel
    {
        private readonly object _context;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly string _tempFile;
        private readonly DelegateCommand<PrintDialogView> _closeCommand;
        private readonly DelegateCommand _printCommand;
        private readonly ObservableCollection<string> _printers;
        public PrintDialogViewModel(object context, IDialogCoordinator dialogCoordinator, string tempFile)
        {
            _context = context;
            _dialogCoordinator = dialogCoordinator;
            _tempFile = tempFile;
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
            try
            {
                Pdf.PrintPdfToPrinter(_tempFile, SelectedPrinter);
                File.Delete(_tempFile);
            }
            catch (Exception)
            {

            }
        }

        public ICommand CloseCommand { get { return _closeCommand; } }
        public ICommand PrintCommand { get { return _printCommand; } }

        private class Pdf
        {
            private static string GetApplicationPath()
            {
                var printApplicationRegistryPaths = new[]
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Acrobat.exe",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\AcroRD32.exe"
                };
                foreach (var printApplicationRegistryPath in printApplicationRegistryPaths)
                {
                    using (var regKeyAppRoot = Registry.LocalMachine.OpenSubKey(printApplicationRegistryPath))
                    {
                        if (regKeyAppRoot == null)
                        {
                            continue;
                        }

                        var applicationPath = (string)regKeyAppRoot.GetValue(null);

                        if (!string.IsNullOrEmpty(applicationPath))
                        {
                            return applicationPath;
                        }
                    }
                }
                return null;
            }
            public static void PrintPdfToPrinter(string pdfFileName, string printerName)
            {
                var printApplicationPath = GetApplicationPath();
                // Print to Acrobat
                const string flagNoSplashScreen = "/s";
                const string flagOpenMinimized = "/h";

                var flagPrintFileToPrinter = $"/t \"{pdfFileName}\" \"{printerName}\"";

                var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);

                var startInfo = new ProcessStartInfo
                {
                    FileName = printApplicationPath,
                    Arguments = args,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var process = Process.Start(startInfo);

                // Close Acrobat regardless of version
                if (process != null)
                {
                    // yeeeah, this doesn't work.
                    process.WaitForInputIdle();
                    process.CloseMainWindow();
                    process.Kill();
                }
            }
        }
    }
}
