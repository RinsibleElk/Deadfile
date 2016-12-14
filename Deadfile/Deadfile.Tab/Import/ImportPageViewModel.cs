using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using Screen = Caliburn.Micro.Screen;

namespace Deadfile.Tab.Import
{
    class ImportPageViewModel : Screen, IImportPageViewModel, INavigationAware
    {
        private readonly IDeadfileRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogCoordinator _dialogCoordinator;

        public ImportPageViewModel(IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            _dialogCoordinator = dialogCoordinator;
            BrowseJobs = new DelegateCommand(BrowseJobsAction);
            BrowseQuotations = new DelegateCommand(BrowseQuotationsAction);
            BrowseLocalAuthorities = new DelegateCommand(BrowseLocalAuthoritiesAction);
        }

        public async void Import()
        {
            var progressAsync = await _dialogCoordinator.ShowProgressAsync(this, "Importing...", "Please wait while we import");
            var importer = new Deadfile.Importer.Importer(_repository, JobsFile, QuotationsFile, LocalAuthoritiesFile);
            await Task.Run(() => importer.PerformImport());
            await progressAsync.CloseAsync();
        }

        public bool CanImport
        {
            get { return (!String.IsNullOrWhiteSpace(JobsFile) && File.Exists(JobsFile) &&
                          !String.IsNullOrWhiteSpace(QuotationsFile) && File.Exists(QuotationsFile) &&
                          !String.IsNullOrWhiteSpace(LocalAuthoritiesFile) && File.Exists(LocalAuthoritiesFile)); }
        }

        private string _jobsFile;
        public string JobsFile
        {
            get { return _jobsFile; }
            set
            {
                if (value == _jobsFile) return;
                _jobsFile = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanImport);
            }
        }

        private void BrowseJobsAction()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = @"Find Jobs File",
                Filter = @"Tab Separated File|*.csv"
            };
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                JobsFile = openFileDialog.FileName;
        }

        private string _quotationsFile;
        public ICommand BrowseJobs { get; }

        public string QuotationsFile
        {
            get { return _quotationsFile; }
            set
            {
                if (value == _quotationsFile) return;
                _quotationsFile = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanImport);
            }
        }

        private void BrowseQuotationsAction()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = @"Find Quotations File",
                Filter = @"Tab Separated File|*.csv"
            };
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                QuotationsFile = openFileDialog.FileName;
        }

        private string _localAuthoritiesFile;
        public ICommand BrowseQuotations { get; }

        public string LocalAuthoritiesFile
        {
            get { return _localAuthoritiesFile; }
            set
            {
                if (value == _localAuthoritiesFile) return;
                _localAuthoritiesFile = value;
                NotifyOfPropertyChange(() => LocalAuthoritiesFile);
                NotifyOfPropertyChange(() => CanImport);
            }
        }

        public ICommand BrowseLocalAuthorities { get; }

        private void BrowseLocalAuthoritiesAction()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = @"Find Local Authorities File",
                Filter = @"Tab Separated File|*.csv"
            };
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                LocalAuthoritiesFile = openFileDialog.FileName;
        }

        public void OnNavigatedTo(object parameters)
        {
            // convert camel case to spaces
            DisplayNameBroadcaster.BroadcastDisplayName(_eventAggregator, Experience);
        }

        public void OnNavigatedFrom()
        {
        }

        public Experience Experience { get; } = Experience.Import;
        public bool ShowActionsPad { get; } = false;
        public void CompleteNavigation()
        {
        }
    }
}
