using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dragablz;

namespace Deadfile
{
    /// <summary>
    /// Defines an instance of the <see cref="IInterTabClient"/> for Dragablz to be able to create new <see cref="ShellView"/> instances when
    /// the user tears a tab.
    /// </summary>
    public sealed class InterTabClient : IInterTabClient
    {
        private readonly SimpleContainer _container;
        private readonly IWindowManager _windowManager;
        public InterTabClient(IWindowManager windowManager, SimpleContainer container)
        {
            _container = container;
            _windowManager = windowManager;
        }

        /// <summary>
        /// Create a new shell for a torn tab.
        /// </summary>
        /// <param name="interTabClient"></param>
        /// <param name="partition"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            // Use Caliburn.Micro's WindowManager to show a new ShellView for us.
            var viewModel = (ShellViewModel)_container.GetInstance(typeof(ShellViewModel), nameof(ShellViewModel));
            _windowManager.ShowWindow(viewModel);
            var view = (ShellView)viewModel.GetView();
            return new NewTabHost<Window>(view, view.Items);
        }

        /// <summary>
        /// Define how to handle when the last tab in a window is removed.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            // Kill.
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
