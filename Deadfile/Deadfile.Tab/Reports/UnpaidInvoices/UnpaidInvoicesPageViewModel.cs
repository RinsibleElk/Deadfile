using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
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

namespace Deadfile.Tab.Reports.UnpaidInvoices
{
    /// <summary>
    /// View model for the Unpaid Invoices Experience. Generates a readonly report of Unpaid Invoices.
    /// </summary>
    class UnpaidInvoicesPageViewModel : ReportPageViewModel<InvoiceModel>, IUnpaidInvoicesPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<InvoiceModel> _navigateToClient;
        private readonly DelegateCommand<InvoiceModel> _navigateToInvoice;

        /// <summary>
        /// Create a new <see cref="UnpaidInvoicesPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public UnpaidInvoicesPageViewModel(TabIdentity tabIdentity,
            IDeadfileDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToClient = new DelegateCommand<InvoiceModel>(PerformNavigateToClient);
            _navigateToInvoice = new DelegateCommand<InvoiceModel>(PerformNavigateToInvoice);
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
            return _repository.GetUnpaidInvoices(filter);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.UnpaidInvoices;

        public ICommand NavigateToClient => _navigateToClient;

        public ICommand NavigateToInvoice => _navigateToInvoice;

        protected override DataGrid GetVisual(object view)
        {
            return ((UnpaidInvoicesPageView) view).Report;
        }
    }
}
