﻿using System;
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
        private int _invoiceReference;
        public int InvoiceReference
        {
            get { return _invoiceReference; }
            set { SetProperty(ref _invoiceReference, value); }
        }

        protected override void LoadChildren()
        {
            if (Mode == BrowserMode.Invoice)
            {
                foreach (var job in Repository.GetBrowserJobsForInvoice(Mode, IncludeInactiveEnabled, Id))
                    Children.Add(job);
                Children.Add(Repository.GetBrowserClientById(Mode, IncludeInactiveEnabled, ParentId));
            }
        }

        public override BrowserModelType ModelType
        {
            get { return BrowserModelType.Invoice; }
        }

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
    }
}
