using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Content.Events;
using Deadfile.Content.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace Deadfile.Content.Navigation
{
    public class NavigationBarViewModel : INavigationBarViewModel
    {
        private IRegionNavigationJournal navigationJournal = null;
        private readonly DelegateCommand backCommand;
        private readonly DelegateCommand homeCommand;
        private readonly DelegateCommand forwardCommand;
        private readonly DelegateCommand undoCommand;
        private readonly DelegateCommand redoCommand;
        public NavigationBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(Navigated);
            backCommand = new DelegateCommand(NavigateBack, CanNavigateBack);
            homeCommand = new DelegateCommand(NavigateHome, CanNavigateBack);
            forwardCommand=new DelegateCommand(NavigateForward, CanNavigateForward);
            undoCommand = new DelegateCommand(PerformUndo, CanUndo);
            redoCommand = new DelegateCommand(PerformRedo, CanRedo);
        }
        private void Navigated(IRegionNavigationJournal navigationJournal)
        {
            this.navigationJournal = navigationJournal;
            backCommand.RaiseCanExecuteChanged();
            homeCommand.RaiseCanExecuteChanged();
            forwardCommand.RaiseCanExecuteChanged();
        }
        public ICommand BackCommand { get { return backCommand; } }

        public ICommand HomeCommand { get { return homeCommand; } }

        public ICommand ForwardCommand { get { return forwardCommand; } }

        public ICommand UndoCommand { get { return undoCommand; } }

        public ICommand RedoCommand { get { return redoCommand; } }

        private void NavigateBack()
        {
            if (navigationJournal != null)
            {
                navigationJournal.GoBack();
                forwardCommand.RaiseCanExecuteChanged();
            }
        }

        private void NavigateForward()
        {
            if (navigationJournal != null)
            {
                navigationJournal.GoForward();
                backCommand.RaiseCanExecuteChanged();
                forwardCommand.RaiseCanExecuteChanged();
                homeCommand.RaiseCanExecuteChanged();
            }
        }

        private void NavigateHome()
        {
            if (navigationJournal != null)
            {
                while (navigationJournal.CanGoBack)
                    navigationJournal.GoBack();
                forwardCommand.RaiseCanExecuteChanged();
            }
        }
        private bool CanNavigateBack()
        {
            return (navigationJournal != null) && (navigationJournal.CanGoBack);
        }

        private bool CanNavigateForward()
        {
            return (navigationJournal != null) && (navigationJournal.CanGoForward);
        }

        private void PerformUndo()
        {
            
        }

        private bool CanUndo()
        {
            return false;
        }

        private void PerformRedo()
        {

        }

        private bool CanRedo()
        {
            return false;
        }
    }
}
