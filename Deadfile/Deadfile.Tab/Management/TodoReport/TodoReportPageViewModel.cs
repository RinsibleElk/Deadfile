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

namespace Deadfile.Tab.Management.TodoReport
{
    /// <summary>
    /// View model for the TodoReport Experience. Generates a readonly report of JobTasks.
    /// </summary>
    class TodoReportPageViewModel : ManagementPageViewModel<JobTaskModel>, ITodoReportPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IPrintService _printService;
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
            IDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _printService = printService;
            _repository = repository;
            _navigateToJob = new DelegateCommand<JobTaskModel>(PerformNavigateToJob);
            _navigateToClient = new DelegateCommand<JobTaskModel>(PerformNavigateToClient);
        }

        private void PerformNavigateToJob(JobTaskModel jobTaskModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Job, jobTaskModel.ClientId, jobTaskModel.JobId);
            Logger.Info("Event,SelectedItemEvent,Send,{0},{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        private void PerformNavigateToClient(JobTaskModel jobTaskModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, jobTaskModel.ClientId);
            Logger.Info("Event,SelectedItemEvent,Send,{0},{1}", _tabIdentity, packet);
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

        protected override void PerformDelete()
        {
            throw new NotImplementedException();
        }

        protected override void PerformSave()
        {
            throw new NotImplementedException();
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.TodoReport;
        public override void EditingStatusChanged(bool editable)
        {
            throw new NotImplementedException();
        }

        public ICommand NavigateToClient => _navigateToClient;

        public ICommand NavigateToJob => _navigateToJob;

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (value.Equals(_startDate)) return;
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);

                RefreshModels();
            }
        }

        private DateTime _endDate = DateTime.Today.AddDays(7.0);
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value.Equals(_endDate)) return;
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);

                RefreshModels();
            }
        }

        private bool _includeInactive;
        public bool IncludeInactive
        {
            get { return _includeInactive; }
            set
            {
                if (value == _includeInactive) return;
                _includeInactive = value;
                NotifyOfPropertyChange(() => IncludeInactive);

                RefreshModels();
            }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _view = (TodoReportPageView)view;
        }

        private TodoReportPageView _view;
        public void Print()
        {
            var visual = _view.Report;
            _printService.PrintVisual(visual);
        }
    }
}
