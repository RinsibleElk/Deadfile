using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Interfaces;

namespace Deadfile.Content.DesignTime
{
    class SecondPageDesignTimeViewModel : ISecondPageViewModel
    {
        public string Title { get; set; } = "Design time Second Page";
        public ICommand NavigateCommand => null;
    }
}
