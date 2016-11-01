using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Model;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Home;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Management.LocalAuthorities
{
    /// <summary>
    /// View model for the Local Authorities Experience. Allows management of the known set of local
    /// authorities.
    /// </summary>
    class LocalAuthoritiesPageViewModel : ManagementPageViewModel, ILocalAuthoritiesPageViewModel
    {
        private readonly IDeadfileRepository _repository;
        private ObservableCollection<LocalAuthorityModel> _items;
        private LocalAuthorityModel _selectedItem = new LocalAuthorityModel();

        public LocalAuthoritiesPageViewModel(IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _repository = repository;
        }

        public override void OnNavigatedTo(object parameters)
        {
            base.OnNavigatedTo(parameters);

            // Always concatenate an empty one.
            SelectedItem = new LocalAuthorityModel();
            Items = new ObservableCollection<LocalAuthorityModel>(_repository.GetLocalAuthorities());
            Items.Add(SelectedItem);
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
            SelectedItem = new LocalAuthorityModel();
            Items = new ObservableCollection<LocalAuthorityModel>();
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.LocalAuthorities;

        /// <summary>
        /// All the known local authorities.
        /// </summary>
        public ObservableCollection<LocalAuthorityModel> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public LocalAuthorityModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }
    }
}
