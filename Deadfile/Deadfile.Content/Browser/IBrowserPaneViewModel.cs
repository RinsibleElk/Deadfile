using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Deadfile.Model.Browser;

namespace Deadfile.Content.Browser
{
    public interface IBrowserPaneViewModel
    {
        /// <summary>
        /// The first level of the browser tree.
        /// </summary>
        ObservableCollection<BrowserClient> Clients { get; set; }

        /// <summary>
        /// The filter text from the user.
        /// </summary>
        string FilterText { get; set; }

        /// <summary>
        /// Binding for the selected item. Used to pass information between the Browser and the Content.
        /// </summary>
        BrowserModel SelectedItem { get; set; }

        /// <summary>
        /// User settings for filtering.
        /// </summary>
        FilterSettings FilterSettings { get; set; }

        /// <summary>
        /// If an entity is open for editing then browsing will be disabled.
        /// </summary>
        bool BrowsingEnabled { get; set; }
    }
}
