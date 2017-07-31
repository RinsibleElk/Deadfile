using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Model;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Pdf;
using Deadfile.Tab.Browser;
using Deadfile.Tab.Clients;
using Deadfile.Tab.Events;
using Deadfile.Tab.Home;
using Deadfile.Tab.Invoices;
using Deadfile.Tab.JobChildren;
using Deadfile.Tab.JobChildren.Applications;
using Deadfile.Tab.JobChildren.BillableHours;
using Deadfile.Tab.JobChildren.Expenses;
using Deadfile.Tab.JobChildren.JobTasks;
using Deadfile.Tab.Jobs;
using Deadfile.Tab.Json;
using Deadfile.Tab.Management.DefineQuotations;
using Deadfile.Tab.Management.LocalAuthorities;
using Deadfile.Tab.Navigation;
using Deadfile.Tab.Quotes;
using Deadfile.Tab.Reports.CurrentApplications;
using Deadfile.Tab.Reports.TodoReport;
using Deadfile.Tab.Reports.UnbilledJobs;
using Deadfile.Tab.Reports.Invoices;
using Deadfile.Tab.Tab;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using Prism.Events;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockSetup : IDisposable
    {
        public readonly Dictionary<string, object> ViewModels = new Dictionary<string, object>();
        public readonly TabIdentity TabIdentity = new TabIdentity(1);
        public readonly MockRandomNumberGenerator RandomNumberGenerator = new MockRandomNumberGenerator(42);
        public readonly MockDeadfileContextAbstractionFactory DbContext;
        public readonly DeadfileRepository Repository;
        public readonly MockInvoiceGenerator InvoiceGenerator = new MockInvoiceGenerator();
        public readonly ModelEntityMapper Mapper = new ModelEntityMapper();
        public readonly MockQuotationsTimerService TimerService = new MockQuotationsTimerService();
        public readonly MockDeadfileDispatcherTimerService DispatcherTimerService = new MockDeadfileDispatcherTimerService();
        public readonly MockUrlNavigationService UrlNavigationService = new MockUrlNavigationService();
        public readonly MockDeadfileFileStreamService FileStreamService = new MockDeadfileFileStreamService();
        public readonly MockDeadfileDialogCoordinator DeadfileDialogCoordinator = new MockDeadfileDialogCoordinator();
        public readonly MockPrintService PrintService = new MockPrintService();
        public readonly MockExcelService ExcelService = new MockExcelService();
        public readonly MockNavigationContainer NavigationContainer;
        public readonly NavigationService NavigationService;
        public readonly Mock<IEventAggregator> EventAggregator = new Mock<IEventAggregator>();
        public readonly TabViewModel TabViewModel;
        public readonly NavigationBarViewModel NavigationBarViewModel;
        public readonly HomePageViewModel HomePageViewModel;
        public readonly ClientsPageViewModel ClientsPageViewModel;
        public readonly JobsPageViewModel JobsPageViewModel;
        public readonly LocalAuthoritiesPageViewModel LocalAuthoritiesPageViewModel;
        public readonly UnbilledJobsPageViewModel UnbilledJobsPageViewModel;
        public readonly CurrentApplicationsPageViewModel CurrentApplicationsPageViewModel;
        public readonly InvoicesReportPageViewModel InvoicesReportPageViewModel;
        public readonly TodoReportPageViewModel TodoReportPageViewModel;
        public readonly ImportPageViewModel ImportPageViewModel;
        public readonly ExportPageViewModel ExportPageViewModel;
        public readonly DefineQuotationsPageViewModel DefineQuotationsPageViewModel;
        public readonly InvoicesPageViewModel InvoicesPageViewModel;
        public readonly JobTasksJobChildViewModel JobTasksJobChildViewModel;
        public readonly ApplicationsJobChildViewModel ApplicationsJobChildViewModel;
        public readonly ExpensesJobChildViewModel ExpensesJobChildViewModel;
        public readonly BillableHoursJobChildViewModel BillableHoursJobChildViewModel;
        public readonly ClientsActionsPadViewModel ClientsActionsPadViewModel;
        public readonly JobsActionsPadViewModel JobsActionsPadViewModel;
        public readonly InvoicesActionsPadViewModel InvoicesActionsPadViewModel;
        public readonly BrowserPaneViewModel BrowserPaneViewModel;
        public readonly QuotesBarViewModel QuotesBarViewModel;

        public MockSetup(MockDeadfileContextAbstractionFactory factory, bool populate)
        {
            DeadfileContextAbstraction.TestOnlyFakeConnectionString();
            DbContext = factory;
            Repository = new DeadfileRepository(Mapper, RandomNumberGenerator, DbContext);
            NavigationContainer = new MockNavigationContainer(ViewModels);
            NavigationService = new NavigationService(NavigationContainer);
            TabViewModel = new TabViewModel(TabIdentity, EventAggregator.Object, NavigationService);
            NavigationBarViewModel = new NavigationBarViewModel(TabIdentity, NavigationService, EventAggregator.Object, Repository);
            HomePageViewModel = new HomePageViewModel(TabIdentity, EventAggregator.Object);
            ClientsPageViewModel = new ClientsPageViewModel(TabIdentity, EventAggregator.Object, Repository, DeadfileDialogCoordinator, UrlNavigationService);
            JobsPageViewModel = new JobsPageViewModel(TabIdentity, NavigationService, Repository, EventAggregator.Object, DeadfileDialogCoordinator);
            LocalAuthoritiesPageViewModel = new LocalAuthoritiesPageViewModel(UrlNavigationService, DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            UnbilledJobsPageViewModel = new UnbilledJobsPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object);
            CurrentApplicationsPageViewModel = new CurrentApplicationsPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object);
            InvoicesReportPageViewModel = new InvoicesReportPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object, ExcelService);
            TodoReportPageViewModel = new TodoReportPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object);
            ImportPageViewModel = new ImportPageViewModel(factory, Repository, EventAggregator.Object, DeadfileDialogCoordinator, FileStreamService);
            ExportPageViewModel = new ExportPageViewModel(factory, Repository, EventAggregator.Object, DeadfileDialogCoordinator, FileStreamService);
            DefineQuotationsPageViewModel = new DefineQuotationsPageViewModel(DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            InvoicesPageViewModel = new InvoicesPageViewModel(TabIdentity, PrintService, Repository, EventAggregator.Object, InvoiceGenerator, DeadfileDialogCoordinator);
            JobTasksJobChildViewModel = new JobTasksJobChildViewModel(DispatcherTimerService, DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            ApplicationsJobChildViewModel = new ApplicationsJobChildViewModel(DispatcherTimerService, DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            ExpensesJobChildViewModel = new ExpensesJobChildViewModel(DispatcherTimerService, DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            BillableHoursJobChildViewModel = new BillableHoursJobChildViewModel(DispatcherTimerService, DeadfileDialogCoordinator, Repository, EventAggregator.Object);
            ClientsActionsPadViewModel = new ClientsActionsPadViewModel(TabIdentity, EventAggregator.Object);
            JobsActionsPadViewModel = new JobsActionsPadViewModel(TabIdentity, EventAggregator.Object);
            InvoicesActionsPadViewModel = new InvoicesActionsPadViewModel(TabIdentity, EventAggregator.Object);
            BrowserPaneViewModel = new BrowserPaneViewModel(EventAggregator.Object, Repository);
            QuotesBarViewModel = new QuotesBarViewModel(Repository, TimerService);
            ViewModels.Add("NavigationBar", NavigationBarViewModel);
            ViewModels.Add("HomePage", HomePageViewModel);
            ViewModels.Add("ClientsPage", ClientsPageViewModel);
            ViewModels.Add("JobsPage", JobsPageViewModel);
            ViewModels.Add("LocalAuthoritiesPage", LocalAuthoritiesPageViewModel);
            ViewModels.Add("UnbilledJobsPage", UnbilledJobsPageViewModel);
            ViewModels.Add("CurrentApplicationsPage", CurrentApplicationsPageViewModel);
            ViewModels.Add("InvoicesReportPage", InvoicesReportPageViewModel);
            ViewModels.Add("TodoReportPage", TodoReportPageViewModel);
            ViewModels.Add("ImportPage", ImportPageViewModel);
            ViewModels.Add("ExportPage", ExportPageViewModel);
            ViewModels.Add("DefineQuotationsPage", DefineQuotationsPageViewModel);
            ViewModels.Add("InvoicesPage", InvoicesPageViewModel);
            ViewModels.Add("JobTasksJobChild", JobTasksJobChildViewModel);
            ViewModels.Add("ApplicationsJobChild", ApplicationsJobChildViewModel);
            ViewModels.Add("ExpensesJobChild", ExpensesJobChildViewModel);
            ViewModels.Add("BillableHoursJobChild", BillableHoursJobChildViewModel);
            ViewModels.Add("ClientsActionsPad", ClientsActionsPadViewModel);
            ViewModels.Add("JobsActionsPad", JobsActionsPadViewModel);
            ViewModels.Add("InvoicesActionsPad", InvoicesActionsPadViewModel);
            ViewModels.Add("BrowserPane", BrowserPaneViewModel);
            ViewModels.Add("QuotesBar", QuotesBarViewModel);
            EventAggregator.Setup((ea) => ea.GetEvent<AddClientEvent>()).Returns(new AddClientEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<AddNewJobEvent>()).Returns(new AddNewJobEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<CanUndoEvent>()).Returns(new CanUndoEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<DeleteEvent>()).Returns(new DeleteEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<DiscardChangesEvent>()).Returns(new DiscardChangesEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<DisplayNameEvent>()).Returns(new DisplayNameEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<EditActionEvent>()).Returns(new EditActionEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<JobChildNavigateEvent>()).Returns(new JobChildNavigateEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<InvoiceClientEvent>()).Returns(new InvoiceClientEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<LockedForEditingEvent>()).Returns(new LockedForEditingEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<NavigateEvent>()).Returns(new NavigateEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<NavigateFallBackEvent>()).Returns(new NavigateFallBackEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<PageStateEvent<ClientsPageState>>()).Returns(new PageStateEvent<ClientsPageState>());
            EventAggregator.Setup((ea) => ea.GetEvent<PageStateEvent<JobsPageState>>()).Returns(new PageStateEvent<JobsPageState>());
            EventAggregator.Setup((ea) => ea.GetEvent<PageStateEvent<InvoicesPageState>>()).Returns(new PageStateEvent<InvoicesPageState>());
            EventAggregator.Setup((ea) => ea.GetEvent<PaidEvent>()).Returns(new PaidEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<PrintEvent>()).Returns(new PrintEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<RefreshBrowserEvent>()).Returns(new RefreshBrowserEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<SaveEvent>()).Returns(new SaveEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<SelectedItemEvent>()).Returns(new SelectedItemEvent());
            EventAggregator.Setup((ea) => ea.GetEvent<UndoEvent>()).Returns(new UndoEvent());
            TabViewModel.TestOnlyOnActivate();

            if (populate) PopulateEntireDatabaseFromGui();
        }

        private void PopulateEntireDatabaseFromGui()
        {
            Assert.Equal("Oliver Samson", QuotesBarViewModel.Quotation.Author);
            Assert.Equal("No Quotations defined. Soz.", QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpQuotations(this);
            Assert.Equal(4, Repository.GetQuotations(null).Count());
            TimerService.FireCallback();
            Assert.Equal("Homer Simpson", QuotesBarViewModel.Quotation.Author);
            Assert.Equal("You tried your best and you failed miserably. The lesson is, never try.", QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpLocalAuthorities(this);
            MockData.SetUpClients(this);
            MockData.SetUpJobs(this);
            MockData.SetupJobChildren(this);
            MockData.SetUpInvoices(this);
        }

        public MockSetup(bool populate) : this(new MockDeadfileContextAbstractionFactory(), populate)
        {
        }

        public MockSetup() : this(false)
        {
        }

        public void BrowseToPrivetDrive()
        {
            BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Job;
            Assert.Equal(6, BrowserPaneViewModel.Items.Count);
            var privetDrive = BrowserPaneViewModel.Items.First((b) => ((BrowserJob)b).FullAddress.Contains("Privet Drive"));
            BrowserPaneViewModel.SelectedItem = privetDrive;
            Assert.Equal(JobsPageViewModel, TabViewModel.ContentArea);
            Assert.Equal(2503, JobsPageViewModel.SelectedItem.JobNumber);
        }

        public void BrowseToWindsorGardens()
        {
            BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Job;
            Assert.Equal(6, BrowserPaneViewModel.Items.Count);
            var windsorGardens = BrowserPaneViewModel.Items.First((b) => ((BrowserJob)b).FullAddress.Contains("Windsor Gardens"));
            BrowserPaneViewModel.SelectedItem = windsorGardens;
            Assert.Equal(JobsPageViewModel, TabViewModel.ContentArea);
            Assert.Equal(2505, JobsPageViewModel.SelectedItem.JobNumber);
        }

        public void BrowseToCemeteryRidge()
        {
            BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Job;
            Assert.Equal(6, BrowserPaneViewModel.Items.Count);
            var cemeteryRidge = BrowserPaneViewModel.Items.First((b) => ((BrowserJob)b).FullAddress.Contains("Cemetery Ridge"));
            BrowserPaneViewModel.SelectedItem = cemeteryRidge;
            Assert.Equal(JobsPageViewModel, TabViewModel.ContentArea);
            Assert.Equal(2500, JobsPageViewModel.SelectedItem.JobNumber);
        }

        public void Dispose()
        {
            TabViewModel.TestOnlyOnDeactivate(true);
        }

        public void AddJobTask(string description, string notes, JobTaskPriority? priority)
        {
            Assert.Equal(JobsPageViewModel, TabViewModel.ContentArea);
            Assert.Equal(JobChildExperience.JobTasks, JobsPageViewModel.SelectedJobChild);
            JobTasksJobChildViewModel.SelectedItem = JobTasksJobChildViewModel.Items[JobTasksJobChildViewModel.Items.Count - 1];
            Assert.True(JobTasksJobChildViewModel.EditCommand.CanExecute(null));
            JobTasksJobChildViewModel.EditCommand.Execute(null);
            Assert.True(JobTasksJobChildViewModel.Editable);
            JobTasksJobChildViewModel.SelectedItem.Description = description;
            if (!string.IsNullOrEmpty(notes)) JobTasksJobChildViewModel.SelectedItem.Notes = notes;
            if (priority != null)
            {
                while (JobTasksJobChildViewModel.SelectedItem.Priority != priority.Value)
                {
                    Assert.True(JobTasksJobChildViewModel.TogglePriorityCommand.CanExecute(null));
                    JobTasksJobChildViewModel.TogglePriorityCommand.Execute(null);
                }
            }
            Assert.True(JobTasksJobChildViewModel.SaveCommand.CanExecute(true));
            JobTasksJobChildViewModel.SaveCommand.Execute(true);
        }
    }
}
