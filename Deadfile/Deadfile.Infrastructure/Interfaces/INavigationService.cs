using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// A quick and dirty navigation service that uses Caliburn.Micro conventions navigate.
    /// </summary>
    public interface INavigationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Navigate to the specified view model.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="hostKey"></param>
        /// <param name="key"></param>
        /// <param name="parameters"></param>
        void RequestNavigate(object host, string hostKey, string key, object parameters);

        /// <summary>
        /// Teardown. Called when a host is going down, so that resources can be freed and individual views deactivated.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="hostKey"></param>
        void RequestDeactivate(object host, string hostKey);

        /// <summary>
        /// Go back one in the Journal.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Whether we can go back one in the Journal.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Go forward one in the Journal.
        /// </summary>
        void GoForward();
       
        /// <summary>
        /// Whether we can go forward one in the Journal.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Called to end the life of the navigation service.
        /// </summary>
        void Teardown();
    }
}
