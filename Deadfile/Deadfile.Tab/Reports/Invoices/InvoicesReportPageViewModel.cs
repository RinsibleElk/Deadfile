using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Entity;
using Deadfile.Infrastructure;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Model.Reporting;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Deadfile.Tab.Home;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Reports.Invoices
{
    /// <summary>
    /// View model for the All Invoices Experience. Generates a readonly report of All Invoices.
    /// </summary>
    class InvoicesReportPageViewModel : ReportPageViewModel<InvoiceModel>, IInvoicesReportPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly IExcelService _excelService;
        private readonly DelegateCommand<InvoiceModel> _navigateToClient;
        private readonly DelegateCommand<InvoiceModel> _navigateToInvoice;

        /// <summary>
        /// Create a new <see cref="InvoicesReportPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="excelService"></param>
        public InvoicesReportPageViewModel(TabIdentity tabIdentity,
            IDeadfileDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IExcelService excelService) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _excelService = excelService;
            _navigateToClient = new DelegateCommand<InvoiceModel>(PerformNavigateToClient);
            _navigateToInvoice = new DelegateCommand<InvoiceModel>(PerformNavigateToInvoice);
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;
            IncludeInactive = false;
        }

        private void PerformNavigateToClient(InvoiceModel invoiceModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, invoiceModel.ClientId);
            Logger.Info("Event|SelectedItemEvent|Send|{0}|{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        private void PerformNavigateToInvoice(InvoiceModel invoiceModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Invoice, invoiceModel.ClientId, invoiceModel.InvoiceId);
            Logger.Info("Event|SelectedItemEvent|Send|{0}|{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<InvoiceModel> GetModels(string filter)
        {
            Company? company = null;
            switch (CompanyFilter)
            {
                case CompanyForFilter.Imagine3DLtd:
                    company = Company.Imagine3DLtd;
                    break;
                case CompanyForFilter.PaulSamsonCharteredSurveyorLtd:
                    company = Company.PaulSamsonCharteredSurveyorLtd;
                    break;
            }
            return _repository.GetInvoices(company, StartDate, EndDate, filter, IncludeInactive);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.InvoicesReport;

        public ICommand NavigateToClient => _navigateToClient;

        public ICommand NavigateToInvoice => _navigateToInvoice;
        public void ExportToExcel()
        {
            _excelService.Export(this);
        }

        protected override DataGrid GetVisual(object view)
        {
            return ((InvoicesReportPageView) view).Report;
        }

        public DataGrid DataGrid => (DataGrid) Visual;
    }
}
