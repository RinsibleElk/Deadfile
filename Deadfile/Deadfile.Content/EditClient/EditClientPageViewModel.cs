using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.ViewModels;
using Deadfile.Model;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.EditClient
{
    public sealed class EditClientPageViewModel : ParameterisedContentViewModelBase<int>, IEditClientPageViewModel
    {
        public EditClientPageViewModel(
            IEventAggregator eventAggregator,
            IDeadfileNavigationService navigationService,
            INavigationParameterMapper navigationParameterMapper)
            : base(eventAggregator, navigationService, navigationParameterMapper)
        {
        }

        public override Experience Experience { get { return Experience.EditClient; } }

        public override void OnNavigatedTo(NavigationContext navigationContext, int clientId)
        {
            base.OnNavigatedTo(navigationContext, clientId);

            if (clientId == ClientModel.NewClientId)
            {
                Title = "New Client";
            }
            else
            {
                Title = "Edit Client";
            }
        }
    }
}
