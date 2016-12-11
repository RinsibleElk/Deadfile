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

namespace Deadfile.Tab.Management.UnbilledClients
{
    /// <summary>
    /// View model for the Unbilled Clients Experience. Allows management of the known set of local
    /// authorities.
    /// </summary>
    class UnbilledClientsPageViewModel : ManagementPageViewModel<UnbilledClientModel>, IUnbilledClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<UnbilledClientModel> _navigateToClient;

        /// <summary>
        /// Create a new <see cref="UnbilledClientsPageViewModel"/>.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public UnbilledClientsPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator, false)
        {
            _repository = repository;
            _navigateToClient = new DelegateCommand<UnbilledClientModel>(PerformNavigateToClient);
        }

        private void PerformNavigateToClient(UnbilledClientModel clientModel)
        {
            EventAggregator.GetEvent<SelectedItemEvent>()
                .Publish(new SelectedItemPacket(BrowserModelType.Client, ModelBase.NewModelId, clientModel.ClientId));
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<UnbilledClientModel> GetModels(string filter)
        {
            return _repository.GetUnbilledClients(filter);
        }

        protected override void PerformSave()
        {
            throw new NotImplementedException();
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.UnbilledClients;
        public override void EditingStatusChanged(bool editable)
        {
            throw new NotImplementedException();
        }

        public ICommand NavigateToClient
        {
            get { return _navigateToClient; }
        }
    }
}
