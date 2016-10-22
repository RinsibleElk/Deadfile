using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;
using Prism.Regions;

namespace Deadfile.Content.DesignTime
{
    class HomePageDesignTimeViewModel : IHomePageViewModel
    {
        public string Title { get; set; } = "Design time Home Page";
        public ICommand NavigateCommand => null;
    }
}
