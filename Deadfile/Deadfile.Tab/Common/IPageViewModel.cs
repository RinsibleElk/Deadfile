using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// The basic elements implemented by a content area view model.
    /// </summary>
    public interface IPageViewModel : IScreen, INavigationAware, IJournaled
    {
        /// <summary>
        /// The <see cref="Experience"/> that this page displays.
        /// </summary>
        Experience Experience { get; }

        /// <summary>
        /// Whether an actions pad should be displayed during this experience.
        /// </summary>
        bool ShowActionsPad { get; }

        /// <summary>
        /// Whether a browser pane should be displayed during this experience.
        /// </summary>
        bool ShowBrowserPane { get; }

        /// <summary>
        /// Called when all related views are up and running to finalise navigation of the content area.
        /// </summary>
        /// <remarks>
        /// Guaranteed to be called once by the tab view model.
        /// </remarks>
        void CompleteNavigation();
    }
}
