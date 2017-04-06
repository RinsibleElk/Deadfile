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

namespace Deadfile.Tab.Reports.CurrentApplications
{
    /// <summary>
    /// View model for the Unpaid Invoices Experience. Generates a readonly report of Unpaid Invoices.
    /// </summary>
    class CurrentApplicationsPageViewModel : ReportPageViewModel<CurrentApplicationModel>, ICurrentApplicationsPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<CurrentApplicationModel> _navigateToJob;

        /// <summary>
        /// Create a new <see cref="CurrentApplicationsPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public CurrentApplicationsPageViewModel(TabIdentity tabIdentity,
            IDeadfileDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToJob = new DelegateCommand<CurrentApplicationModel>(PerformNavigateToJob);
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(7);
        }

        private void PerformNavigateToJob(CurrentApplicationModel applicationModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Job, applicationModel.ClientId, applicationModel.JobId);
            Logger.Info("Event|SelectedItemEvent|Send|{0}|{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<CurrentApplicationModel> GetModels(string filter)
        {
            return _repository.GetCurrentApplications(filter);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.CurrentApplications;

        public ICommand NavigateToJob => _navigateToJob;

        protected override DataGrid GetVisual(object view)
        {
            return ((CurrentApplicationsPageView) view).Report;
        }
    }
}
