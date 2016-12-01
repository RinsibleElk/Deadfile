using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Tab.Actions;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Clients
{
    class ClientsActionsPadViewModel : ActionsPadViewModel, IClientsActionsPadViewModel
    {
        private bool _canAddItem = true;
        private bool _addItemIsVisible = true;

        public ClientsActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        protected override void LockedForEditingAction(LockedForEditingMessage lockedForEditingMessage)
        {
            base.LockedForEditingAction(lockedForEditingMessage);

            AddItemIsVisible = CanAddItem = !lockedForEditingMessage.IsLocked;
        }

        public void AddItem()
        {
            EventAggregator.GetEvent<AddClientEvent>().Publish(new AddClientMessage());
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
    }
}
