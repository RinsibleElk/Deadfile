using System;
using Deadfile.Content.Browser;
using Deadfile.Content.Clients;
using Deadfile.Content.Home;
using Deadfile.Content.Interfaces;
using Deadfile.Content.Navigation;
using Deadfile.Content.Quotes;
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
            _unityContainer.RegisterType<ClientsPage>();
            _unityContainer.RegisterTypeForNavigation<ClientsPage>();

            // This excludes the browser region from Journaling.
            _regionManager.RegisterViewWithRegion(RegionNames.BrowserRegion, typeof(BrowserPane));

            // This excludes the actions region from Journaling.
            _regionManager.RegisterViewWithRegion(RegionNames.ActionsRegion, typeof(HomeActionsPad));
            _regionManager.RegisterViewWithRegion(RegionNames.ActionsRegion, typeof(ClientsActionsPad));

            // This excludes the navigation region from Journaling.
            _regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(NavigationBar));

            // This excludes the quotes region from Journaling.
            _regionManager.RegisterViewWithRegion(RegionNames.QuotesRegion, typeof(QuotesBar));
        }
    }
}
