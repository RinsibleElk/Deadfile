using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model.Interfaces;

namespace Deadfile.Model.Browser
{
    public sealed class BrowserInvoice : BrowserModel
    {
        protected override void LoadChildren()
        {
            if (Mode == BrowserMode.Invoice)
            {
                foreach (var job in Repository.GetBrowserJobsForInvoice(Mode, IncludeInactiveEnabled, Id))
                    Children.Add(job);
                Children.Add(Repository.GetBrowserClientById(Mode, IncludeInactiveEnabled, ParentId));
            }
        }

        public override BrowserModelType ModelType => BrowserModelType.Invoice;

        private InvoiceStatus _status = InvoiceStatus.Created;
        public InvoiceStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private Company _company;
        public Company Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }

        public string DisplayName => $"{InvoiceReference} ({ClientName}, {Project})";

        private int _invoiceReference;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set
            {
                if (SetProperty(ref _invoiceReference, value))
                    RaisePropertyChanged(nameof(DisplayName));
            }
        }

        private string _clientName = "";
        public string ClientName
        {
            get { return _clientName; }
            set
            {
                if (SetProperty(ref _clientName, value))
                    RaisePropertyChanged(nameof(DisplayName));
            }
        }

        private string _project = "";
        public string Project
        {
            get { return _project; }
            set
            {
                if (SetProperty(ref _project, value))
                    RaisePropertyChanged(nameof(DisplayName));
            }
        }

        private DateTime _createdDate = DateTime.Today;
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { SetProperty(ref _createdDate, value); }
        }
    }
}
