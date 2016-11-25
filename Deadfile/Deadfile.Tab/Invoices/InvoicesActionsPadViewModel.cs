using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Invoices
{
    /// <summary>
    /// Actions pad providing specific actions for the Invoices experience.
    /// </summary>
    class InvoicesActionsPadViewModel : ActionsPadViewModel, IInvoicesActionsPadViewModel
    {
        private bool _canPrintItem = true;
        private bool _canPaidItem = true;
        private bool _printItemIsVisible = true;
        private bool _paidItemIsVisible = true;

        public InvoicesActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public void PrintItem()
        {
            // Send a message to perform the print.
            // Save (if locked).
            // Unlock for editing (if locked).
            // Should only be available if either unlocked or locked but no errors.
        }

        public bool CanPrintItem
        {
            get { return _canPrintItem; }
            set
            {
                if (value == _canPrintItem) return;
                _canPrintItem = value;
                NotifyOfPropertyChange(() => CanPrintItem);
            }
        }

        public void PaidItem()
        {
            // Send a message to perform the print.
            // Save (if locked).
            // Unlock for editing (if locked).
            // Should only be available if either unlocked or locked but no errors.
        }

        public bool CanPaidItem
        {
            get { return _canPaidItem; }
            set
            {
                if (value == _canPaidItem) return;
                _canPaidItem = value;
                NotifyOfPropertyChange(() => CanPaidItem);
            }
        }

        public bool PrintItemIsVisible
        {
            get { return _printItemIsVisible; }
            set
            {
                if (value == _printItemIsVisible) return;
                _printItemIsVisible = value;
                NotifyOfPropertyChange(() => PrintItemIsVisible);
            }
        }

        public bool PaidItemIsVisible
        {
            get { return _paidItemIsVisible; }
            set
            {
                if (value == _paidItemIsVisible) return;
                _paidItemIsVisible = value;
                NotifyOfPropertyChange(() => PaidItemIsVisible);
            }
        }
    }

}

