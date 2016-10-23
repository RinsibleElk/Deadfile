using System;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Deadfile.Content
{
    /// <summary>
    /// The ContentModule is currently the only module in Deadfile. (And probably always will be the only module?)
    /// </summary>
    public class ContentModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// Create the module.
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="unityContainer"></param>
        public ContentModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// Register the views for this module.
        /// </summary>
        public void Initialize()
        {
            // Journaled. These support navigation back and forward via the NavigationBar.
            _unityContainer.RegisterType<HomePage>();
            _unityContainer.RegisterTypeForNavigation<HomePage>();
            _unityContainer.RegisterType<SecondPage>();
            _unityContainer.RegisterTypeForNavigation<SecondPage>();
            _unityContainer.RegisterType<ThirdPage>();
            _unityContainer.RegisterTypeForNavigation<ThirdPage>();
            _unityContainer.RegisterType<FourthPage>();
            _unityContainer.RegisterTypeForNavigation<FourthPage>();
            _unityContainer.RegisterType<ClientsPage>();
            _unityContainer.RegisterTypeForNavigation<ClientsPage>();

            // This excludes the navigation region and NavigationBar from registering with the Journal.
            _regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(NavigationBar));
        }
    }
}
