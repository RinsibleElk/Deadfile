using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Events;
using Prism.Events;

namespace Deadfile.Tab.Clients
{
    public class ClientsPageViewModel : EditableItemViewModel<int, ClientModel>, IClientsPageViewModel
    {
        private bool _canAddNewJob = false;
        private readonly IDeadfileRepository _repository;

        public ClientsPageViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository) : base(eventAggregator)
        {
            _repository = repository;
        }

        /// <summary>
        /// User hits the button for Add New Job.
        /// </summary>
        public void AddNewJob()
        {
            // Navigate to the Jobs page with the specified Client and no job given.
            EventAggregator.GetEvent<AddNewJobEvent>().Publish(SelectedItem.ClientId);
        }

        public bool CanAddNewJob
        {
            get { return _canAddNewJob; }
            set
            {
                if (value == _canAddNewJob) return;
                _canAddNewJob = value;
                NotifyOfPropertyChange(() => CanAddNewJob);
            }
        }

        public override void OnNavigatedTo(int selectedId)
        {
            base.OnNavigatedTo(selectedId == 0 ? ModelBase.NewModelId : selectedId);

            CanAddNewJob = !Editable;
        }

        public override ClientModel GetModel(int id)
        {
            ClientModel clientModel;
            if (id == 0 || id == ModelBase.NewModelId)
            {
                clientModel = new ClientModel();
                DisplayName = "Clients";
            }
            else
            {
                clientModel = _repository.GetClientById(id);
                if (clientModel.ClientId == ModelBase.NewModelId)
                    DisplayName = "Clients";
                else
                    DisplayName = clientModel.FullName;
            }
            EventAggregator.GetEvent<DisplayNameEvent>().Publish(DisplayName);
            return clientModel;
        }

        public override void EditingStatusChanged(bool editable)
        {
            CanAddNewJob = (!Editable) && (SelectedItem.ClientId != ModelBase.NewModelId);
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

        public Experience Experience
        {
            get { return Experience.Clients; }
        }

        public bool ShowActionsPad { get; } = true;
    }
}
