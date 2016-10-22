using System;
using Deadfile.Content.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Deadfile.Content
{
    public class ContentModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        public ContentModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
        }

        public void Initialize()
        {
            _unityContainer.RegisterType<HomePage>();
            _unityContainer.RegisterTypeForNavigation<HomePage>();
            _unityContainer.RegisterType<SecondPage>();
            _unityContainer.RegisterTypeForNavigation<SecondPage>();

            _regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(NavigationBar));
        }
    }
}
