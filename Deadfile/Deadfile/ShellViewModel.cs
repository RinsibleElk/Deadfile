using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure;
using Deadfile.Tab;
using Dragablz;

namespace Deadfile
{
    /// <summary>
    /// The view model for Deadfile's shell.
    /// </summary>
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive
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
            _container = container;
            InterTabClient = interTabClient;
            OpenNewTab = new SimpleCommand(OpenTab);
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

        public void CloseItem(Screen dataContext)
        {
            dataContext.TryClose();
        }

        public IInterTabClient InterTabClient { get; private set; }

        public ICommand OpenNewTab { get; }
    }
}
