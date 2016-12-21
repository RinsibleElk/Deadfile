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
    class JsonPageViewModel : Screen, IJsonPageViewModel, INavigationAware
    {
        private readonly JsonImporter _jsonImporter;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogCoordinator _dialogCoordinator;

        public JsonPageViewModel(IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
        {
            _jsonImporter = new JsonImporter(repository);
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

        public async void Export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = @"Export to JSON File",
                AddExtension = true,
                DefaultExt = @"json"
            };
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var progressAsync = await _dialogCoordinator.ShowProgressAsync(this, "Exporting...", "Please wait while we export to " + saveFileDialog.FileName);
                await Task.Run(() => _jsonImporter.ExportToJsonFile(new FileInfo(saveFileDialog.FileName)));
                await progressAsync.CloseAsync();
            }
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

        public Experience Experience { get; } = Experience.Json;
        public bool ShowActionsPad { get; } = false;
        public void CompleteNavigation()
        {
        }
    }
}
