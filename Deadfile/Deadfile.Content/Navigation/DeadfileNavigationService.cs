using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Prism.Regions;

namespace Deadfile.Content.Navigation
{
    /// <summary>
    /// Implementation of <see cref="IDeadfileNavigationService"/> that uses the <see cref="IRegionManager"/> to actually navigate the
    /// various content regions.
    /// </summary>
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
            switch (experience)
            {
                case Experience.HomePage:
                    uri = new Uri("/" + experience, UriKind.Relative);
                    break;
                default:
                    uri = new Uri(experience.ToString(), UriKind.Relative);
                    break;
            }
            _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
        }

        public void NavigateBrowserTo(Experience experience)
        {
            Uri uri;
            switch (experience)
            {
                case Experience.HomePage:
                    uri = new Uri("/" + experience.ToString().Replace("Page", "") + "BrowserPane", UriKind.Relative);
                    break;
                default:
                    uri = new Uri(experience.ToString().Replace("Page", "") + "BrowserPane", UriKind.Relative);
                    break;
            }
            _regionManager.RequestNavigate(RegionNames.BrowserRegion, uri);
        }

        public void NavigateActionsTo(Experience experience)
        {
            Uri uri;
            switch (experience)
            {
                case Experience.HomePage:
                    uri = new Uri("/" + experience.ToString().Replace("Page", "") + "ActionsPad", UriKind.Relative);
                    break;
                default:
                    uri = new Uri(experience.ToString().Replace("Page", "") + "ActionsPad", UriKind.Relative);
                    break;
            }
            _regionManager.RequestNavigate(RegionNames.ActionsRegion, uri);
        }
    }
}
