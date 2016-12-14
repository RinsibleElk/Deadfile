using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Prism.Commands;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Management.TodoReport
{
    /// <summary>
    /// View model for the Unbilled Clients Experience. Allows management of the known set of local
    /// authorities.
    /// </summary>
    class TodoReportPageViewModel : ManagementPageViewModel<JobTaskModel>, ITodoReportPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<JobTaskModel> _navigateToJob;

        /// <summary>
        /// Create a new <see cref="TodoReportPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public TodoReportPageViewModel(TabIdentity tabIdentity,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToJob = new DelegateCommand<JobTaskModel>(PerformNavigateToJob);
        }

        private void PerformNavigateToJob(JobTaskModel clientModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Job, SelectedItem.ClientId, SelectedItem.JobId);
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
            return _repository.GetJobTasks(EndDate, filter);
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

        public ICommand NavigateToJob
        {
            get { return _navigateToJob; }
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
    }
}
