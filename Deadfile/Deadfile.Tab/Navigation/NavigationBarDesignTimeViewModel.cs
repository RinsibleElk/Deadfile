using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Model.Browser;

namespace Deadfile.Tab.Navigation
{
    class NavigationBarDesignTimeViewModel : INavigationBarViewModel
    {
        public void Home()
        {
            throw new NotImplementedException();
        }

        public bool CanHome { get; } = true;

        public void Back()
        {
            throw new NotImplementedException();
        }

        public bool CanBack { get; } = true;

        public void Forward()
        {
            throw new NotImplementedException();
        }

        public bool CanForward { get; } = false;

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool CanUndo { get; } = false;

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public bool CanRedo { get; } = false;
        public string SearchText { get; set; } = "Sear";
        public bool IsSearchShown { get; } = true;
        public bool IncludeInactive { get; set; } = false;
        public ObservableCollection<BrowserModel> SearchResults { get; } = new ObservableCollection<BrowserModel>();
        public BrowserModel SelectedSearchItem { get; set; } = null;
        public ICommand LostFocusCommand { get; } = null;
        public ICommand GotFocusCommand { get; } = null;
    }
}
