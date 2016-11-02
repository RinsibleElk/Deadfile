using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Management.LocalAuthorities
{
    class LocalAuthoritiesPageDesignTimeViewModel :
        ManagementPageDesignTimeViewModel<LocalAuthorityModel>,
        ILocalAuthoritiesPageViewModel
    {
        public LocalAuthoritiesPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = new LocalAuthorityModel();
            Items = new ObservableCollection<LocalAuthorityModel>(repository.GetLocalAuthorities(null));
            Items.Add(SelectedItem);
        }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.LocalAuthorities;
        public ICommand NavigateCommand { get; } = null;
    }
}
