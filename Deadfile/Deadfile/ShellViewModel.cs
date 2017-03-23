using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure;
using Deadfile.Infrastructure.Styles;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Tab;
using Dragablz;
using MahApps.Metro;
using Prism.Commands;
using Accent = Deadfile.Infrastructure.Styles.Accent;
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
            if (!Enum.TryParse(Properties.Settings.Default.Theme, out _themeToUse))
            {
                _themeToUse = Theme.BaseDark;
                Properties.Settings.Default.Theme = _themeToUse.ToString();
                Properties.Settings.Default.Save();
            }
            if (!Enum.TryParse(Properties.Settings.Default.Accent, out _accentToUse))
            {
                _accentToUse = Accent.Red;
                Properties.Settings.Default.Accent = _accentToUse.ToString();
                Properties.Settings.Default.Save();
            }
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
            OpenNewTabToExperienceCommand = new DelegateCommand<Experience?>(OpenNewTabToExperience);
            OpenNewTabToInvoiceClientCommand=new DelegateCommand<int?>(OpenNewTabToInvoiceClient);
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

        private void OpenNewTabToInvoiceClient(int? clientId)
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            if (clientId == null)
                throw new ApplicationException("Expected a valid client id to invoice.");
            else
                tabModule.NavigateToNewInvoice(clientId.Value);
        }

        public void OpenNewTabToExperience(Experience? experience)
        {
            var tabModule = _container.GetInstance<TabModule>();
            var tabViewModel = tabModule.GetFirstViewModel();
            ActivateItem(tabViewModel);
            if (experience == null)
                throw new ApplicationException("Expected a valid experience to navigate to.");
            else
                tabModule.NavigateToExperience(experience.Value);
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
        public ICommand OpenNewTabToExperienceCommand { get; }
        public ICommand OpenNewTabToInvoiceClientCommand { get; }

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
            DeadfileContextAbstraction.DatabaseName = _database;
            DeadfileContextAbstraction.UserId = _username;
            DeadfileContextAbstraction.ServerName = _server;
            DeadfileContextAbstraction.Password = _password;
            DeadfileContextAbstraction.RebuildConnectionString();
            Properties.Settings.Default.Save();
            SettingsIsOpen = false;
        }

        private static bool DebugAcceptAnything = false;

        private bool CanAccept
        {
            get
            {
                if (DebugAcceptAnything) return true;
                DeadfileContextAbstraction.DatabaseName = _database;
                DeadfileContextAbstraction.UserId = _username;
                DeadfileContextAbstraction.ServerName = _server;
                DeadfileContextAbstraction.Password = _password;
                var result = DeadfileContextAbstraction.RebuildConnectionString();
                DeadfileContextAbstraction.DatabaseName = Properties.Settings.Default.Database;
                DeadfileContextAbstraction.UserId = Properties.Settings.Default.Username;
                DeadfileContextAbstraction.ServerName = Properties.Settings.Default.Server;
                DeadfileContextAbstraction.Password = Properties.Settings.Default.Password;
                DeadfileContextAbstraction.RebuildConnectionString();
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

        private Theme _themeToUse;
        public Theme ThemeToUse
        {
            get { return _themeToUse; }
            set
            {
                if (value == _themeToUse) return;
                _themeToUse = value;
                Properties.Settings.Default.Theme = _themeToUse.ToString();
                Properties.Settings.Default.Save();
                ThemeManager.ChangeAppStyle(Application.Current,
                                            ThemeManager.GetAccent(_accentToUse.ToString()),
                                            ThemeManager.GetAppTheme(_themeToUse.ToString()));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ThemeToUse)));
            }
        }

        private Accent _accentToUse;
        public Accent AccentToUse
        {
            get { return _accentToUse; }
            set
            {
                if (value == _accentToUse) return;
                _accentToUse = value;
                Properties.Settings.Default.Accent = _accentToUse.ToString();
                Properties.Settings.Default.Save();
                ThemeManager.ChangeAppStyle(Application.Current,
                                            ThemeManager.GetAccent(_accentToUse.ToString()),
                                            ThemeManager.GetAppTheme(_themeToUse.ToString()));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AccentToUse)));
            }
        }
    }
}
