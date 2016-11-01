using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Tab.Events;
using System.Text.RegularExpressions;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Common
{
    /// <summary>
    /// Base class for management pages. Sets the display name for you.
    /// </summary>
    abstract class ManagementPageViewModel : Screen, IPageViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        

        /// <summary>
        /// Requires an event aggregator to communicate the display name to the tab.
        /// </summary>
        /// <param name="eventAggregator"></param>
        protected ManagementPageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// For management pages, the base class takes care of the display name for you.
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(object parameters)
        {
            // convert camel case to spaces
            _eventAggregator.GetEvent<DisplayNameEvent>().Publish(Regex.Replace(Experience.ToString(), @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1"));
        }

        /// <summary>
        /// Can be overridden to perform event handling.
        /// </summary>
        public virtual void OnNavigatedFrom()
        {
        }

        public abstract Experience Experience { get; }
        public bool ShowActionsPad { get; } = false;
    }
}
