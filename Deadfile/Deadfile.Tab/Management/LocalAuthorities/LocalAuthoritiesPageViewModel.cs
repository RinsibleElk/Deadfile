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
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using Deadfile.Tab.Home;
using Prism.Commands;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Management.LocalAuthorities
{
    /// <summary>
    /// View model for the Local Authorities Experience. Allows management of the known set of local
    /// authorities.
    /// </summary>
    class LocalAuthoritiesPageViewModel : ManagementPageViewModel<LocalAuthorityModel>, ILocalAuthoritiesPageViewModel
    {
        private readonly IUrlNavigationService _urlNavigationService;
        private readonly IDeadfileRepository _repository;
        private readonly DelegateCommand<string> _navigateCommand;

        /// <summary>
        /// Create a new <see cref="LocalAuthoritiesPageViewModel"/>.
        /// </summary>
        /// <param name="urlNavigationService"></param>
        /// <param name="repository"></param>
        /// <param name="eventAggregator"></param>
        public LocalAuthoritiesPageViewModel(IUrlNavigationService urlNavigationService, IDeadfileRepository repository, IEventAggregator eventAggregator) : base(eventAggregator, true)
        {
            _urlNavigationService = urlNavigationService;
            _repository = repository;
            _navigateCommand = new DelegateCommand<string>(NavigateToUrl);
        }

        private void NavigateToUrl(string url)
        {
            _urlNavigationService.Navigate(url);
        }

        /// <summary>
        /// Perform the database interaction.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<LocalAuthorityModel> GetModels(string filter)
        {
            return _repository.GetLocalAuthorities(filter);
        }

        protected override void PerformDelete()
        {
            throw new NotImplementedException();
        }

        protected override void PerformSave()
        {
            _repository.SaveLocalAuthority(SelectedItem);
        }

        // Common for every journaled page (content).
        public override Experience Experience { get; } = Experience.LocalAuthorities;
        public override void EditingStatusChanged(bool editable)
        {
        }

        public ICommand NavigateCommand {
            get { return _navigateCommand; }
        }
    }
}
