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
    class InvoicesActionsPadViewModel : ActionsPadViewModel<InvoicesPageState>, IInvoicesActionsPadViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private bool _canPaidItem = true;
        private bool _paidItemIsVisible = true;

        public InvoicesActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator) : base(tabIdentity, eventAggregator)
        {
        }

        public void PrintItem()
        {
            // Send a message to perform the print.
            // Save (if locked).
            // Unlock for editing (if locked).
            // Should only be available if either unlocked or locked but no errors.
            if (CanSaveItem)
            {
                // Perform the save, and lock the item again.
                Logger.Info("Event|SaveEvent|Send|{0}|{1}", TabIdentity.TabIndex, SaveMessage.SaveAndPrint);
                EventAggregator.GetEvent<SaveEvent>().Publish(SaveMessage.SaveAndPrint);

                // Notify the other pages for the end of editing.
                Logger.Info("Event|EditActionEvent|Send|{0}|{1}", TabIdentity.TabIndex, EditActionMessage.EndEditing);
                EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
            }
            else
            {
                Logger.Info("Event|PrintEvent|Send|{0}|{1}", TabIdentity.TabIndex, PrintMessage.Print);
                EventAggregator.GetEvent<PrintEvent>().Publish(PrintMessage.Print);
            }
        }

        public bool CanPrintItem => CanPrintItemInner();
        private bool CanPrintItemInner()
        {
            if (PageState.HasFlag(InvoicesPageState.UnderEdit))
                return CanSaveItem;
            return CanEditItem;
        }
        public override bool CanSaveItem => PageState.HasFlag(InvoicesPageState.CanSave);
        public override bool SaveItemIsVisible => PageState.HasFlag(InvoicesPageState.UnderEdit);
        public override bool CanDeleteItem => PageState.HasFlag(InvoicesPageState.CanDelete);
        public override bool DeleteItemIsVisible => !PageState.HasFlag(InvoicesPageState.UnderEdit);
        public override bool CanEditItem => PageState.HasFlag(InvoicesPageState.CanEdit);
        public override bool EditItemIsVisible => !PageState.HasFlag(InvoicesPageState.UnderEdit);
        public override bool CanDiscardItem => PageState.HasFlag(InvoicesPageState.CanDiscard);
        public override bool DiscardItemIsVisible => PageState.HasFlag(InvoicesPageState.UnderEdit);
        protected override void PageStateChanged(InvoicesPageState state)
        {
            NotifyOfPropertyChange(() => CanPrintItem);
        }

        public void PaidItem()
        {
            Logger.Info("Event|PaidEvent|Send|{0}|{1}", TabIdentity.TabIndex, PaidMessage.Paid);
            EventAggregator.GetEvent<PaidEvent>().Publish(PaidMessage.Paid);
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

