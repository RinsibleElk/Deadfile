using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Deadfile.Content.ActionsPad;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;
using Deadfile.Content.ViewModels;
using Prism.Commands;

namespace Deadfile.Content.Clients
{
    /// <summary>
    /// Clients page is simple, straightforward extension of the base actions pad, with additional action for Add.
    /// (Jobs and Invoices must be added through their respective parents.)
    /// </summary>
    sealed class ClientsActionsPadViewModel : ActionsPadViewModel, IClientsActionsPadViewModel
    {
        private readonly DelegateCommand _addItemCommand;

        public ClientsActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _addItemCommand = new DelegateCommand(AddItemAction, CanAddItem);
        }

        private Visibility _addItemVisibility = Visibility.Visible;
        public Visibility AddItemVisibility
        {
            get { return _addItemVisibility; }
            set
            {
                if (SetProperty(ref _addItemVisibility, value))
                {
                    _addItemCommand.RaiseCanExecuteChanged();
                }
            }
        }

        protected override void LockedForEditingAction(bool lockedForEditing)
        {
            base.LockedForEditingAction(lockedForEditing);
            AddItemVisibility = lockedForEditing ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool CanAddItem()
        {
            return _addItemVisibility == Visibility.Visible;
        }

        public ICommand AddItemCommand
        {
            get { return _addItemCommand; }
        }
        private void AddItemAction()
        {
            EventAggregator.GetEvent<EditItemEvent>().Publish(EditAction.Add);
        }
    }
}
