using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Events;

namespace Deadfile.Content.ViewModels
{
    public sealed class HomeActionsPadViewModel : ViewModelBase, IHomeActionsPadViewModel
    {
        public HomeActionsPadViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
