using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Undo;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Clients
{
    public class ClientsPageViewModel : EditableItemContentViewModelBase<ClientModel>, IClientsPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand _addNewJobCommand;

        public ClientsPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            IDeadfileRepository repository,
            INavigationParameterMapper mapper)
            : base(eventAggregator, navigationService, mapper)
        {
            _repository = repository;
            _addNewJobCommand = new DelegateCommand(AddNewJobAction);
        }

        private void AddNewJobAction()
        {
        }

        public override void EditingStatusChanged(bool editable)
        {
            AddNewJobVisibility = (Editable) ? Visibility.Collapsed : (SelectedItem.ClientId == ModelBase.NewModelId) ? Visibility.Collapsed : Visibility.Visible;
        }

        public override Experience Experience
        {
            get { return Experience.Clients; }
        }

        public override void PerformSave()
        {
            try
            {
                _repository.SaveClient(SelectedItem);
            }
            catch (Exception)
            {
                //TODO Do something. Like raise a dialog box or something. Then clean up.
                throw;
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext, int selectedId)
        {
            base.OnNavigatedTo(navigationContext, selectedId);

            AddNewJobVisibility = (Editable) ? Visibility.Collapsed : (SelectedItem.ClientId == ModelBase.NewModelId) ? Visibility.Collapsed : Visibility.Visible;
        }

        public override ClientModel GetModelById(int id)
        {
            return _repository.GetClientById(id);
        }

        private Visibility _addNewJobVisibility;
        public Visibility AddNewJobVisibility
        {
            get { return _addNewJobVisibility; }
            set { SetProperty(ref _addNewJobVisibility, value); }
        }

        public ICommand AddNewJobCommand
        {
            get { return _addNewJobCommand; }
        }
    }
}
