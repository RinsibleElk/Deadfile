using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Content.Interfaces;
using Deadfile.Content.JobChildren;
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
        private readonly INavigationParameterMapper _navigationParameterMapper;
        public DeadfileNavigationService(IRegionManager regionManager, INavigationParameterMapper navigationParameterMapper)
        {
            _regionManager = regionManager;
            _navigationParameterMapper = navigationParameterMapper;
        }

        public void NavigateTo(Experience experience)
        {
            Uri uri;
            switch (experience)
            {
                case Experience.Home:
                    uri = new Uri("/" + experience + "Page", UriKind.Relative);
                    break;
                default:
                    uri = new Uri(experience + "Page", UriKind.Relative);
                    break;
            }
            _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
        }

        /// <summary>
        /// Navigate with an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="experience"></param>
        /// <param name="parameter"></param>
        public void NavigateTo<T>(Experience experience, T parameter) where T : new()
        {
            var uri = new Uri(experience + "Page", UriKind.Relative);
            var navigationParameters = _navigationParameterMapper.ConvertToNavigationParameters(parameter);
            _regionManager.RequestNavigate(RegionNames.ContentRegion, uri, navigationParameters);
        }

        public void NavigateActionsTo(Experience experience)
        {
            Uri uri;
            switch (experience)
            {
                case Experience.Home:
                    uri = new Uri("/" + experience + "ActionsPad", UriKind.Relative);
                    break;
                default:
                    uri = new Uri(experience + "ActionsPad", UriKind.Relative);
                    break;
            }
            _regionManager.RequestNavigate(RegionNames.ActionsRegion, uri);
        }

        public void NavigateJobsChildTo(JobChildExperience jobChildExperience, int jobId)
        {
            var uri = new Uri("/" + jobChildExperience + "Control", UriKind.Relative);
            _regionManager.RequestNavigate(RegionNames.JobChildRegion, uri);
        }
    }
}
