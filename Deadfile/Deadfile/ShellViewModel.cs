using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Tab;
using Dragablz;
using Prism.Commands;
using LogManager = NLog.LogManager;

namespace Deadfile
{
    /// <summary>
    /// The view model for Deadfile's shell.
    /// </summary>
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel
    {
        private readonly SimpleContainer _container;
        private static bool _isFirst = true;
        private string _server;
        private string _database;
        private string _username;
        private string _password;
        private readonly DelegateCommand _acceptCommand;
        private readonly DelegateCommand _cancelCommand;

        /// <summary>
        /// Invoked by SimpleContainer.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="interTabClient"></param>
        public ShellViewModel(SimpleContainer container, IInterTabClient interTabClient)
        {
            _server = Properties.Settings.Default.Server;
            _database = Properties.Settings.Default.Database;
            _username = Properties.Settings.Default.Username;
            _password = Properties.Settings.Default.Password;
            _acceptCommand = new DelegateCommand(Accept, () => CanAccept);
            _cancelCommand = new DelegateCommand(Cancel);
            ClosingItemActionCallback = new ItemActionCallback((dataContext) =>
            {
                CloseItem((Screen)dataContext.DragablzItem.DataContext);
            });
            _container = container;
            InterTabClient = interTabClient;
            OpenNewTab = new DelegateCommand(OpenTab);
            OpenNewTabToBrowserModelCommand = new DelegateCommand<BrowserModel>(OpenTabToBrowserItem);
            OpenNewTabToNewClientCommand = new DelegateCommand(OpenNewTabToNewClient);
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

        public void OpenTabToBrowserItem(BrowserModel browserModel)
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            tabModule.NavigateToBrowserModel(browserModel);
        }

        public void OpenNewTabToNewClient()
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            tabModule.NavigateToNewClient();
        }

        public ItemActionCallback ClosingItemActionCallback { get; }

        public void CloseItem(Screen dataContext)
        {
            dataContext.TryClose();
        }

        public IInterTabClient InterTabClient { get; }

        public ICommand OpenNewTab { get; }

        public ICommand OpenNewTabToBrowserModelCommand { get; }

        public ICommand OpenNewTabToNewClientCommand { get; }
        public string Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Server)));
                _acceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string Database
        {
            get { return _database; }
            set
            {
                _database = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Database)));
                _acceptCommand.RaiseCanExecuteChanged();
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Username)));
                _acceptCommand.RaiseCanExecuteChanged();
            }
        }

        // Ridiculously naughty...
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Password)));
                _acceptCommand.RaiseCanExecuteChanged();
            }
        }

        private void Cancel()
        {
            SettingsIsOpen = false;
        }

        private void Accept()
        {
            Properties.Settings.Default.Database = _database;
            Properties.Settings.Default.Username = _username;
            Properties.Settings.Default.Server = _server;
            Properties.Settings.Default.Password = _password;
            DeadfileRepository.DatabaseName = _database;
            DeadfileRepository.UserId = _username;
            DeadfileRepository.ServerName = _server;
            DeadfileRepository.Password = _password;
            DeadfileRepository.RebuildConnectionString();
            Properties.Settings.Default.Save();
            SettingsIsOpen = false;
        }

        private bool CanAccept
        {
            get
            {
                DeadfileRepository.DatabaseName = _database;
                DeadfileRepository.UserId = _username;
                DeadfileRepository.ServerName = _server;
                DeadfileRepository.Password = _password;
                var result = DeadfileRepository.RebuildConnectionString();
                DeadfileRepository.DatabaseName = Properties.Settings.Default.Database;
                DeadfileRepository.UserId = Properties.Settings.Default.Username;
                DeadfileRepository.ServerName = Properties.Settings.Default.Server;
                DeadfileRepository.Password = Properties.Settings.Default.Password;
                DeadfileRepository.RebuildConnectionString();
                return result;
            }
        }

        public ICommand AcceptCommand => _acceptCommand;
        public ICommand CancelCommand => _cancelCommand;
        private bool _settingsIsOpen = false;

        public bool SettingsIsOpen
        {
            get { return _settingsIsOpen; }
            set
            {
                if (value == _settingsIsOpen) return;
                _settingsIsOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SettingsIsOpen)));
            }
        }
    }
}
