using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Infrastructure.Services;
using Deadfile.Model;
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
using Deadfile.Tab.Reports.UnpaidInvoices;
using Deadfile.Tab.Tab;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using Prism.Events;

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
        public readonly MockDeadfileDialogCoordinator DeadfileDialogCoordinator = new MockDeadfileDialogCoordinator();
        public readonly MockPrintService PrintService = new MockPrintService();
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
        public readonly UnpaidInvoicesPageViewModel UnpaidInvoicesPageViewModel;
        public readonly TodoReportPageViewModel TodoReportPageViewModel;
        public readonly JsonPageViewModel JsonPageViewModel;
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

        public MockSetup(MockDeadfileContextAbstractionFactory factory)
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
            UnpaidInvoicesPageViewModel = new UnpaidInvoicesPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object);
            TodoReportPageViewModel = new TodoReportPageViewModel(TabIdentity, DeadfileDialogCoordinator, PrintService, Repository, EventAggregator.Object);
            JsonPageViewModel = new JsonPageViewModel(factory, Repository, EventAggregator.Object, DeadfileDialogCoordinator);
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
            ViewModels.Add("UnpaidInvoicesPage", UnpaidInvoicesPageViewModel);
            ViewModels.Add("TodoReportPage", TodoReportPageViewModel);
            ViewModels.Add("JsonPage", JsonPageViewModel);
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
        }

        public MockSetup() : this(new MockDeadfileContextAbstractionFactory())
        {
        }

        public void Dispose()
        {
            TabViewModel.TestOnlyOnDeactivate(true);
        }
    }
}
