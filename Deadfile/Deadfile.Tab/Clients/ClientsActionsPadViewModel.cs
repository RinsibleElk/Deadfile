using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Tab.Actions;
using Deadfile.Tab.Events;
using Prism.Commands;
using Prism.Events;

namespace Deadfile.Tab.Clients
{
    class ClientsActionsPadViewModel : ActionsPadViewModel<ClientsPageState>, IClientsActionsPadViewModel
    {
        public ClientsActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator) : base(tabIdentity, eventAggregator)
        {
            AddItemCommand = new DelegateCommand(AddItem, () => CanAddItem);
        }

        public bool CanAddItem => !PageState.HasFlag(ClientsPageState.UnderEdit);
        public bool AddItemIsVisible => !PageState.HasFlag(ClientsPageState.UnderEdit);
        public override bool CanSaveItem => PageState.HasFlag(ClientsPageState.CanSave);
        public override bool SaveItemIsVisible => PageState.HasFlag(ClientsPageState.UnderEdit);
        public override bool CanDeleteItem => PageState.HasFlag(ClientsPageState.CanDelete);
        public override bool DeleteItemIsVisible => !PageState.HasFlag(ClientsPageState.UnderEdit);
        public override bool CanEditItem => PageState.HasFlag(ClientsPageState.CanEdit);
        public override bool EditItemIsVisible => !PageState.HasFlag(ClientsPageState.UnderEdit);
        public override bool CanDiscardItem => PageState.HasFlag(ClientsPageState.CanDiscard);
        public override bool DiscardItemIsVisible => PageState.HasFlag(ClientsPageState.UnderEdit);
        protected override void PageStateChanged(ClientsPageState state)
        {
        }

        public void AddItem()
        {
            Logger.Info("Event|AddClientEvent|Send|{0}|{1}", TabIdentity.TabIndex, AddClientMessage.AddClient);
            EventAggregator.GetEvent<AddClientEvent>().Publish(AddClientMessage.AddClient);
        }

        public ICommand AddItemCommand { get; }
    }
}
