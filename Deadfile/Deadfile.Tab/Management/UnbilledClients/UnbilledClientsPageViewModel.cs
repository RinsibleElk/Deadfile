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

namespace Deadfile.Tab.Management.UnbilledClients
{
    /// <summary>
    /// View model for the Unbilled Clients Experience. Generates a readonly report of Unbilled Clients.
    /// </summary>
    class UnbilledClientsPageViewModel : ReportPageViewModel<UnbilledClientModel>, IUnbilledClientsPageViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly TabIdentity _tabIdentity;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<UnbilledClientModel> _navigateToClient;

        /// <summary>
        /// Create a new <see cref="UnbilledClientsPageViewModel"/>.
        /// </summary>
        /// <param name="tabIdentity"></param>
        /// <param name="dialogCoordinator"></param>
        /// <param name="printService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public UnbilledClientsPageViewModel(TabIdentity tabIdentity,
            IDialogCoordinator dialogCoordinator,
            IPrintService printService,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator) : base(printService, dialogCoordinator, eventAggregator, false)
        {
            _tabIdentity = tabIdentity;
            _repository = repository;
            _navigateToClient = new DelegateCommand<UnbilledClientModel>(PerformNavigateToClient);
        }

        private void PerformNavigateToClient(UnbilledClientModel clientModel)
        {
            var packet = new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, clientModel.ClientId);
            Logger.Info("Event,SelectedItemEvent,Send,{0},{1}", _tabIdentity, packet);
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(packet);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<UnbilledClientModel> GetModels(string filter)
        {
            return _repository.GetUnbilledClients(filter);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.UnbilledClients;

        public ICommand NavigateToClient => _navigateToClient;

        protected override DataGrid GetVisual(object view)
        {
            return ((UnbilledClientsPageView) view).Report;
        }
    }
}
