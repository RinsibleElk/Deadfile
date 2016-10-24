using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Deadfile.Content.Events;
using Deadfile.Content.ViewModels;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Browser
{
    public sealed class BrowserPaneViewModel : ViewModelBase, IBrowserPaneViewModel
    {
        private readonly IDeadfileRepository _repository;
        public BrowserPaneViewModel(IEventAggregator eventAggregator, IDeadfileRepository repository) : base(eventAggregator)
        {
            _repository = repository;
            Clients = new ObservableCollection<BrowserClient>(_repository.GetBrowserClients(null));
        }

        private BrowserModel _selectedItem;
        public BrowserModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    switch (_selectedItem.ModelType)
                    {
                        case BrowserModelType.Client:
                            var clientModel = _repository.GetClientById(_selectedItem.Id);
                            EventAggregator.GetEvent<SelectedClientEvent>().Publish(clientModel);
                            break;
                        case BrowserModelType.Job:
                            var jobModel = _repository.GetJobById(_selectedItem.Id);
                            EventAggregator.GetEvent<SelectedJobEvent>().Publish(jobModel);
                            break;
                        case BrowserModelType.Invoice:
                            var invoiceModel = _repository.GetInvoiceById(_selectedItem.Id);
                            EventAggregator.GetEvent<SelectedInvoiceEvent>().Publish(invoiceModel);
                            break;
                    }
                }
            }
        }

        private ObservableCollection<BrowserClient> _clients;
        public ObservableCollection<BrowserClient> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private string _filterText = "";

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    Clients = new ObservableCollection<BrowserClient>(_repository.GetBrowserClients(_filterText));
                }
            }
        }
    }
}
