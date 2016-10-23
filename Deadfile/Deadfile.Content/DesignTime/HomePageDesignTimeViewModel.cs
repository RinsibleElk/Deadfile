using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Prism.Regions;

namespace Deadfile.Content.DesignTime
{
    class HomePageDesignTimeViewModel : IHomePageViewModel
    {
        public Experience Experience { get { return Experience.HomePage; } }
        public string Title { get; set; } = "Design time Home Page";
        public ICommand ClientsCommand => null;
    }
}
