using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.DesignTime;
using Deadfile.Tab.DesignTime;

namespace Deadfile.Tab.Management.LocalAuthorities
{
    class LocalAuthoritiesPageDesignTimeViewModel : PageDesignTimeViewModel, ILocalAuthoritiesPageViewModel
    {
        public LocalAuthoritiesPageDesignTimeViewModel()
        {
            var repository = new DeadfileDesignTimeRepository();
            SelectedItem = new LocalAuthorityModel();
            Items = new ObservableCollection<LocalAuthorityModel>(repository.GetLocalAuthorities());
            Items.Add(SelectedItem);
        }

        public ObservableCollection<LocalAuthorityModel> Items { get; set; }
        public LocalAuthorityModel SelectedItem { get; set; }

        // Stuff that every page has.
        public override Experience Experience { get; } = Experience.LocalAuthorities;
    }
}
