using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prism.Modularity;
using Prism.Regions;
using Deadfile.Content;
using Deadfile.Content.Views;

namespace Deadfile
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        private readonly IRegionManager _regionManager;
        private static readonly string HomePageViewKey = "/" + nameof(HomePage);
        private static readonly Uri HomePageViewUri = new Uri(HomePageViewKey, UriKind.Relative);

        public Shell(IRegionManager regionManager, IModuleManager moduleManager)
        {
            _regionManager = regionManager;

            InitializeComponent();

            moduleManager.LoadModuleCompleted +=
                (s, e) =>
                {
                    if (e.ModuleInfo.ModuleName == nameof(ContentModule))
                    {
                        _regionManager.RequestNavigate(
                            RegionNames.ContentRegion,
                            HomePageViewUri);
                    }
                };
        }
    }
}
