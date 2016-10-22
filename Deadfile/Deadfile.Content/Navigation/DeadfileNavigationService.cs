using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Regions;

namespace Deadfile.Content.Navigation
{
    public sealed class DeadfileNavigationService : IDeadfileNavigationService
    {
        private readonly IRegionManager _regionManager;
        public DeadfileNavigationService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void NavigateTo(Experience experience)
        {
            Uri uri;
            if (experience == Experience.HomePage)
            {
                uri = new Uri("/" + experience, UriKind.Relative);
            }
            else
            {
                uri = new Uri (experience.ToString(), UriKind.Relative);
            }
            _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
        }
    }
}
