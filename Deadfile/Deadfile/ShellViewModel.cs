using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure;
using Deadfile.Model.Browser;
using Deadfile.Tab;
using Dragablz;
using Prism.Commands;
using LogManager = NLog.LogManager;

namespace Deadfile
{
    /// <summary>
    /// The view model for Deadfile's shell.
    /// </summary>
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel
    {
        private readonly SimpleContainer _container;
        private static bool _isFirst = true;

        /// <summary>
        /// Invoked by SimpleContainer.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="interTabClient"></param>
        public ShellViewModel(SimpleContainer container, IInterTabClient interTabClient)
        {
            ClosingItemActionCallback = new ItemActionCallback((dataContext) =>
            {
                CloseItem((Screen)dataContext.DragablzItem.DataContext);
            });
            _container = container;
            InterTabClient = interTabClient;
            OpenNewTab = new DelegateCommand(OpenTab);
            OpenNewTabToBrowserModelCommand = new DelegateCommand<BrowserModel>(OpenTabToBrowserItem);
            OpenNewTabToNewClientCommand=new DelegateCommand(OpenNewTabToNewClient);
            if (_isFirst)
            {
                OpenTab();
                _isFirst = false;
            }
            DisplayName = "Deadfile";
        }

        public void OpenTab()
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
        }

        public void OpenTabToBrowserItem(BrowserModel browserModel)
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            tabModule.NavigateToBrowserModel(browserModel);
        }

        public void OpenNewTabToNewClient()
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            tabModule.NavigateToNewClient();
        }

        public ItemActionCallback ClosingItemActionCallback { get; }

        public void CloseItem(Screen dataContext)
        {
            dataContext.TryClose();
        }

        public IInterTabClient InterTabClient { get; }

        public ICommand OpenNewTab { get; }

        public ICommand OpenNewTabToBrowserModelCommand { get; }

        public ICommand OpenNewTabToNewClientCommand { get; }

        private TabablzControl _tabablz = null;
        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            _tabablz = ((ShellView) view).Items;
        }
    }
}
