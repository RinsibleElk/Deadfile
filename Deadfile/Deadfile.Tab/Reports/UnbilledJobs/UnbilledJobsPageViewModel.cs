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

namespace Deadfile.Tab.Reports.UnbilledJobs
{
    /// <summary>
    /// View model for the Unbilled Jobs Experience. Generates a readonly report of Unbilled Jobs.
    /// </summary>
    class UnbilledJobsPageViewModel : ReportPageViewModel<UnbilledJobModel>, IUnbilledJobsPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<UnbilledJobModel> _navigateToClient;
        private readonly DelegateCommand<UnbilledJobModel> _navigateToJob;

        /// <summary>
        /// Create a new <see cref="UnbilledJobsPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public UnbilledJobsPageViewModel(TabIdentity tabIdentity,
            IDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToClient = new DelegateCommand<UnbilledJobModel>(PerformNavigateToClient);
            _navigateToJob = new DelegateCommand<UnbilledJobModel>(PerformNavigateToJob);
        }

        private void PerformNavigateToClient(UnbilledJobModel jobModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, jobModel.ClientId);
            Logger.Info("Event,SelectedItemEvent,Send,{0},{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        private void PerformNavigateToJob(UnbilledJobModel jobModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Job, jobModel.ClientId, jobModel.JobId);
            Logger.Info("Event,SelectedItemEvent,Send,{0},{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<UnbilledJobModel> GetModels(string filter)
        {
            return _repository.GetUnbilledJobs(filter);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.UnbilledJobs;

        public ICommand NavigateToClient => _navigateToClient;

        public ICommand NavigateToJob => _navigateToJob;

        protected override DataGrid GetVisual(object view)
        {
            return ((UnbilledJobsPageView) view).Report;
        }
    }
}
