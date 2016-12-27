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
    class ClientsActionsPadViewModel : ActionsPadViewModel, IClientsActionsPadViewModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private bool _canAddItem = true;
        private bool _addItemIsVisible = true;

        public ClientsActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator) : base(tabIdentity, eventAggregator)
        {
            AddItemCommand = new DelegateCommand(AddItem, () => CanAddItem);
        }

        protected override void LockedForEditingAction(LockedForEditingMessage lockedForEditingMessage)
        {
            base.LockedForEditingAction(lockedForEditingMessage);

            AddItemIsVisible = CanAddItem = !lockedForEditingMessage.IsLocked;
        }

        public void AddItem()
        {
            Logger.Info("Event,AddClientEvent,Send,{0},{1}", TabIdentity.TabIndex, AddClientMessage.AddClient);
            EventAggregator.GetEvent<AddClientEvent>().Publish(AddClientMessage.AddClient);
        }

        public bool CanAddItem
        {
            get { return _canAddItem; }
            set
            {
                if (value == _canAddItem) return;
                _canAddItem = value;
                NotifyOfPropertyChange(() => CanAddItem);
            }
        }

        public bool AddItemIsVisible
        {
            get { return _addItemIsVisible; }
            set
            {
                if (value == _addItemIsVisible) return;
                _addItemIsVisible = value;
                NotifyOfPropertyChange(() => AddItemIsVisible);
            }
        }

        public ICommand AddItemCommand { get; }
    }
}
