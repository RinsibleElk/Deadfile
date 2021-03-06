﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Browser;

namespace Deadfile.Tab.Browser
{
    public interface IBrowserPaneViewModel
    {
        /// <summary>
        /// The first level of the browser tree.
        /// </summary>
        ObservableCollection<BrowserModel> Items { get; set; }

        /// <summary>
        /// Binding for the selected item. Used to pass information between the Browser and the Content.
        /// </summary>
        BrowserModel SelectedItem { get; set; }

        /// <summary>
        /// User settings for filtering.
        /// </summary>
        BrowserSettings BrowserSettings { get; set; }

        /// <summary>
        /// If an entity is open for editing then browsing will be disabled.
        /// </summary>
        bool BrowsingEnabled { get; set; }
    }
}
