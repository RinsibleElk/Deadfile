using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Deadfile.Content.ViewModels
{
    public class FourthPageViewModel : ViewModelBase, IFourthPageViewModel
    {
        public FourthPageViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Title = "This is the last page";
        }
    }
}
