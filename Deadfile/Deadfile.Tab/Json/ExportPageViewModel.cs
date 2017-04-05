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
    class ExportPageViewModel : Screen, IExportPageViewModel, INavigationAware
    {
        private readonly JsonImporter _jsonImporter;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeadfileDialogCoordinator _dialogCoordinator;

        public ExportPageViewModel(IDeadfileContextAbstractionFactory contextAbstractionFactory,
            IDeadfileRepository repository,
            IEventAggregator eventAggregator,
            IDeadfileDialogCoordinator dialogCoordinator)
        {
            _jsonImporter = new JsonImporter(contextAbstractionFactory, repository);
            _eventAggregator = eventAggregator;
            _dialogCoordinator = dialogCoordinator;
        }

        public async void Export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = @"Export to JSON File",
                AddExtension = true,
                DefaultExt = @".json",
                Filter = @"JSON files (.json)|*.json"
            };
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var progressAsync = await _dialogCoordinator.ShowProgressAsync(this, "Exporting...", "Please wait while we export to " + saveFileDialog.FileName);
                await Task.Run(() => _jsonImporter.ExportToJsonFile(new FileInfo(saveFileDialog.FileName)));
                await progressAsync.CloseAsync();
            }
        }

        public void OnNavigatedTo(object parameters)
        {
            // convert camel case to spaces
            DisplayNameBroadcaster.BroadcastDisplayName(_eventAggregator, Experience);
        }

        public void OnNavigatedFrom()
        {
        }

        public Experience Experience { get; } = Experience.Export;
        public bool ShowActionsPad { get; } = false;
        public bool ShowBrowserPane { get; } = false;
        public void CompleteNavigation()
        {
        }
    }
}
