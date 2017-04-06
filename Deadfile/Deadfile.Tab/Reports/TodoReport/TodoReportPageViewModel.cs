using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
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

namespace Deadfile.Tab.Reports.TodoReport
{
    /// <summary>
    /// View model for the TodoReport Experience. Generates a readonly report of JobTasks.
    /// </summary>
    class TodoReportPageViewModel : ReportPageViewModel<JobTaskModel>, ITodoReportPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<JobTaskModel> _navigateToJob;
        private readonly DelegateCommand<JobTaskModel> _navigateToClient;

        /// <summary>
        /// Create a new <see cref="TodoReportPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public TodoReportPageViewModel(TabIdentity tabIdentity,
            IDeadfileDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToJob = new DelegateCommand<JobTaskModel>(PerformNavigateToJob);
            _navigateToClient = new DelegateCommand<JobTaskModel>(PerformNavigateToClient);
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(7);
        }

        private void PerformNavigateToJob(JobTaskModel jobTaskModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Job, jobTaskModel.ClientId, jobTaskModel.JobId);
            Logger.Info("Event|SelectedItemEvent|Send|{0}|{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        private void PerformNavigateToClient(JobTaskModel jobTaskModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, jobTaskModel.ClientId);
            Logger.Info("Event|SelectedItemEvent|Send|{0}|{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<JobTaskModel> GetModels(string filter)
        {
            return _repository.GetJobTasks(StartDate, EndDate, filter, IncludeInactive);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.TodoReport;

        public ICommand NavigateToClient => _navigateToClient;

        public ICommand NavigateToJob => _navigateToJob;
        protected override DataGrid GetVisual(object view)
        {
            return ((TodoReportPageView) view).Report;
        }
    }
}
