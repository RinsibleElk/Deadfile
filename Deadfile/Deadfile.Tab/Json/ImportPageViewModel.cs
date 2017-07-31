using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Deadfile.Importer;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.Common;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using Screen = Caliburn.Micro.Screen;

namespace Deadfile.Tab.Json
{
    class ImportPageViewModel : Screen, IImportPageViewModel, INavigationAware
    {
        private readonly JsonImporter _jsonImporter;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeadfileDialogCoordinator _dialogCoordinator;

        public ImportPageViewModel(IDeadfileContextAbstractionFactory contextAbstractionFactory,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDeadfileDialogCoordinator dialogCoordinator,
            IDeadfileFileStreamService fileStreamService)
        {
            _jsonImporter = new JsonImporter(contextAbstractionFactory, repository, fileStreamService);
            _eventAggregator = eventAggregator;
            _dialogCoordinator = dialogCoordinator;
            BrowseJson = new DelegateCommand(BrowseJsonAction);
        }

        public async void Import()
        {
            var progressAsync = await _dialogCoordinator.ShowProgressAsync(this, "Importing...", "Please wait while we import");
            await Task.Run(() => _jsonImporter.ImportFromJsonFile(new FileInfo(JsonFile)));
            await progressAsync.CloseAsync();
        }

        public bool CanImport => !String.IsNullOrWhiteSpace(JsonFile) && File.Exists(JsonFile);

        public ICommand BrowseJson { get; }

        private string _jsonFile;
        public string JsonFile
        {
            get { return _jsonFile; }
            set
            {
                if (value == _jsonFile) return;
                _jsonFile = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanImport);
            }
        }

        private void BrowseJsonAction()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = @"Find JSON File",
                Filter = @"JSON File|*.json"
            };
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                JsonFile = openFileDialog.FileName;
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
        public bool ShowBrowserPane { get; } = false;
        public void CompleteNavigation()
        {
        }
    }
}
