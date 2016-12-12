using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using Screen = Caliburn.Micro.Screen;

namespace Deadfile.Tab.Import
{
    class ImportPageViewModel : Screen, IImportPageViewModel, INavigationAware
    {
        private readonly IDeadfileRepository _repository;
        private readonly IDialogCoordinator _dialogCoordinator;

        public ImportPageViewModel(IDeadfileRepository repository,
            IDialogCoordinator dialogCoordinator)
        {
            _repository = repository;
            _dialogCoordinator = dialogCoordinator;
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

        public void BrowseJobs()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = @"Find Jobs File";
            openFileDialog.Filter = @"Tab Separated File|*.csv";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                JobsFile = openFileDialog.FileName;
        }

        private string _quotationsFile;
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

        public void BrowseQuotations()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = @"Find Quotations File";
            openFileDialog.Filter = @"Tab Separated File|*.csv";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                QuotationsFile = openFileDialog.FileName;
        }

        private string _localAuthoritiesFile;
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

        public void BrowseLocalAuthorities()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = @"Find Local Authorities File";
            openFileDialog.Filter = @"Tab Separated File|*.csv";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                LocalAuthoritiesFile = openFileDialog.FileName;
        }

        public void OnNavigatedTo(object parameters)
        {
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
