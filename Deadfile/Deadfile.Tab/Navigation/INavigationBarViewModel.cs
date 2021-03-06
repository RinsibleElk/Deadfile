﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Model.Browser;
using Deadfile.Tab.Events;

namespace Deadfile.Tab.Navigation
{
    interface INavigationBarViewModel
    {
        void Home();
        bool CanHome { get; }
        void Back();
        bool CanBack { get; }
        void Forward();
        bool CanForward { get; }
        void Undo();
        bool CanUndo { get; }
        void Redo();
        bool CanRedo { get; }
        string SearchText { get; set; }
        bool IsSearchShown { get; }
        bool IncludeInactive { get; set; }
        ObservableCollection<BrowserModel> SearchResults { get; }
        BrowserModel SelectedSearchItem { get; set; }
        ICommand LostFocusCommand { get; }
        ICommand GotFocusCommand { get; }
    }
}
