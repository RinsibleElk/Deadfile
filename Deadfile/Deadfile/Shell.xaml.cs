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
using Deadfile.Content.Interfaces;
using Deadfile.Content.Views;
using Deadfile.Content.Navigation;

namespace Deadfile
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        private readonly IDeadfileNavigationService _navigationService;

        public Shell(IDeadfileNavigationService navigationService, IModuleManager moduleManager)
        {
            _navigationService = navigationService;

            InitializeComponent();

            moduleManager.LoadModuleCompleted +=
                (s, e) =>
                {
                    if (e.ModuleInfo.ModuleName == nameof(ContentModule))
                    {
                        _navigationService.NavigateTo(Experience.HomePage);
                    }
                };
        }
    }
}
