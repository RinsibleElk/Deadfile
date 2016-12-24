using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Model.Reporting;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Reports.UnpaidInvoices
{
    class UnpaidInvoicesPageDesignTimeViewModel :
        ReportPageDesignTimeViewModel<InvoiceModel>,
        IUnpaidInvoicesPageViewModel
    {
        public UnpaidInvoicesPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = null;
            var items = new List<InvoiceModel>();
            var children = new List<InvoiceItemModel>();
            children.Add(new InvoiceItemModel
            {
                Context = 0,
                Description = "An item of work for this invoice",
                InvoiceId = 25,
                InvoiceItemId = 256,
                NetAmount = 170
            });
            items.Add(new InvoiceModel
            {
                ChildrenList = new List<InvoiceItemModel>(children),
                ClientAddressFirstLine = "1 Rinsible Elk Mews",
                ClientAddressSecondLine = "Enfield",
                ClientAddressPostCode = "EN1 1AA",
                ClientId = 17,
                ClientName = "Professor X",
                Company = Company.PaulSamsonCharteredSurveyorLtd,
                CreatedDate = new DateTime(2016,11,25),
                NetAmount = 170,
                InvoiceId = 25,
                Description = "This is the description for invoice 25",
                InvoiceReference = 251,
                Status = InvoiceStatus.Created,
                GrossAmount = 170*1.2,
                VatRate = 20,
                VatValue = 170*0.2,
                Project = "Some work what we billed for"
            });
            Items = new ObservableCollection<InvoiceModel>(items);
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.UnpaidInvoices;

        public ICommand NavigateToClient { get; } = null;

        public ICommand NavigateToInvoice { get; } = null;
    }
}
