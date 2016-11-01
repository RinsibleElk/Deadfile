using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Deadfile.Tab.Common;

namespace Deadfile.Tab.Management.LocalAuthorities
{
    interface ILocalAuthoritiesPageViewModel : IPageViewModel
    {
        ObservableCollection<LocalAuthorityModel> Items { get; set; }
        LocalAuthorityModel SelectedItem { get; set; }
    }
}
