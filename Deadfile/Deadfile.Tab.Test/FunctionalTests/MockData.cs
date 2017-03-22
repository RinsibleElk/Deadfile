using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
using Deadfile.Tab.JobChildren;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    internal static class MockData
    {
        /// <summary>
        /// Starting and ending at Home, set up a bunch of Quotations.
        /// </summary>
        /// <param name="setup"></param>
        public static void SetUpQuotations(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            setup.HomePageViewModel.DefineQuotations();
            setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.SelectedItem.Author = "Homer Simpson";
            setup.DefineQuotationsPageViewModel.SelectedItem.Phrase = "English? Who needs that? I'm never going to England.";
            setup.DefineQuotationsPageViewModel.SaveCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.SelectedItem.Author = "Homer Simpson";
            setup.DefineQuotationsPageViewModel.SelectedItem.Phrase = "You'll have to speak up; I'm wearing a towel.";
            setup.DefineQuotationsPageViewModel.SaveCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.SelectedItem.Author = "Homer Simpson";
            setup.DefineQuotationsPageViewModel.SelectedItem.Phrase = "You tried your best and you failed miserably. The lesson is, never try.";
            setup.DefineQuotationsPageViewModel.SaveCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
            setup.DefineQuotationsPageViewModel.SelectedItem.Author = "Homer Simpson";
            setup.DefineQuotationsPageViewModel.SelectedItem.Phrase = "If at first you don't succeed, give up.";
            setup.DefineQuotationsPageViewModel.SaveCommand.Execute(null);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }

        /// <summary>
        /// Starting and ending at Home, set up a few Local Authorities.
        /// </summary>
        /// <param name="setup"></param>
        public static void SetUpLocalAuthorities(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            setup.HomePageViewModel.LocalAuthorities();
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Enfield Council";
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Url = "https://new.enfield.gov.uk/";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Barnet Council";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Islington Council";
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Url = "https://islington.gov.uk/";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Brent Council";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Lambeth Council";
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Url = "https://lambeth.gov.uk/";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Wandsworth Council";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Camden Council";
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Url = "http://camden.gov.uk/";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
            setup.LocalAuthoritiesPageViewModel.SelectedItem.Name = "Haringey Council";
            setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }

        /// <summary>
        /// Starting and ending at Home, add a few clients.
        /// </summary>
        /// <param name="setup"></param>
        public static void SetUpClients(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            setup.HomePageViewModel.AddClient();
            SetUpRinsibleElk(setup);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
            Assert.True(setup.ClientsActionsPadViewModel.AddItemIsVisible);
            Assert.True(setup.ClientsActionsPadViewModel.CanAddItem);
            setup.ClientsActionsPadViewModel.AddItem();
            SetUpHomerSimpson(setup);
            setup.ClientsActionsPadViewModel.AddItem();
            SetUpEllisParsons(setup);
            setup.ClientsActionsPadViewModel.AddItem();
            SetUpBillyMorton(setup);
            setup.ClientsActionsPadViewModel.AddItem();
            SetUpNicoleBryant(setup);
            setup.ClientsActionsPadViewModel.AddItem();
            SetUpLibbyDean(setup);
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }

        // Expect to be on Clients Page, with it editable.
        private static void SetUpRinsibleElk(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Lord High Admiral";
            c.FirstName = "Rinsible";
            c.LastName = "Elk";
            c.Company = "Rinsible Elk Ltd.";
            c.AddressFirstLine = "1 Fake Lane";
            c.AddressPostCode = "N1 1RR";
            c.EmailAddress = "rinsible.elk@gmail.com";
            c.PhoneNumber1 = "07192384756";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        // Expect to be on Clients Page, with it editable.
        private static void SetUpHomerSimpson(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Mr";
            c.FirstName = "Homer";
            c.MiddleNames = "Jay";
            c.LastName = "Simpson";
            c.Company = "Springfield Nuclear Power Plant";
            c.AddressFirstLine = "742 Evergreen Terrace";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        private static void SetUpEllisParsons(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Mr";
            c.FirstName = "Ellis";
            c.LastName = "Parsons";
            c.AddressFirstLine = "99 Shannon Way";
            c.AddressSecondLine = "Chickering";
            c.AddressPostCode = "IP21 0BS";
            c.PhoneNumber1 = "07707517494";
            c.EmailAddress = "EllisParsons@jourrapide.com";
            c.Company = "Capitalcorp";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        private static void SetUpBillyMorton(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Dr";
            c.FirstName = "Billy";
            c.LastName = "Morton";
            c.AddressFirstLine = "23 Vicar Lane";
            c.AddressSecondLine = "Saunderton";
            c.AddressPostCode = "HP27 6PG";
            c.PhoneNumber1 = "07707517494";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        private static void SetUpNicoleBryant(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Ms";
            c.FirstName = "Nicole";
            c.LastName = "Bryant";
            c.AddressFirstLine = "53 Ballifeary Road";
            c.AddressSecondLine = "Ballimore";
            c.AddressPostCode = "FK19 7FB";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        private static void SetUpLibbyDean(MockSetup setup)
        {
            var c = setup.ClientsPageViewModel.SelectedItem;
            c.Title = "Ms";
            c.FirstName = "Libby";
            c.LastName = "Dean";
            c.AddressFirstLine = "47 Scarcroft Road";
            c.AddressSecondLine = "Pool Quay";
            c.AddressPostCode = "SY21 3NQ";
            Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
            setup.ClientsActionsPadViewModel.SaveItem();
        }

        public static void BrowseToJob(MockSetup setup, string jobNameMatch)
        {
            foreach (var browserModel in setup.BrowserPaneViewModel.Items)
            {
                if (browserModel.ModelType != BrowserModelType.Job)
                    Assert.False(true, "Not in right mode to navigate to Job");
                var job = browserModel as BrowserJob;
                Assert.NotNull(job);
                if (job.FullAddress.Contains(jobNameMatch))
                {
                    setup.BrowserPaneViewModel.SelectedItem = job;
                    break;
                }
            }
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.JobsPageViewModel));
            Assert.True(setup.JobsPageViewModel.SelectedItem.AddressFirstLine.Contains(jobNameMatch));
        }

        public static void BrowseToClient(MockSetup setup, string name)
        {
            foreach (var browserModel in setup.BrowserPaneViewModel.Items)
            {
                if (browserModel.ModelType != BrowserModelType.Client)
                    Assert.False(true, "Not in right mode to navigate to client");
                var client = browserModel as BrowserClient;
                Assert.NotNull(client);
                if (client.FullName.Contains(name))
                {
                    setup.BrowserPaneViewModel.SelectedItem = client;
                    break;
                }
            }
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
            Assert.True(setup.ClientsPageViewModel.SelectedItem.FullName.Contains(name));
        }

        public static void SetUpJobs(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            SetUpJobForExistingClient(setup, "Rinsible Elk", "Cemetery Ridge", "U.S.A.", null, null, "Some works for the Addams Family");
            SetUpJobForExistingClient(setup, "Rinsible Elk", "Wayne Manor", "Gotham City", "U.S.A.", null, "Building some sort of cave");
            SetUpJobForExistingClient(setup, "Libby Dean", "1313 Webfoot Walk", "Duckburg", "Calisota", null, "Some work for Donald");
            SetUpJobForExistingClient(setup, "Libby Dean", "4 Privet Drive", "Berkshire", null, null, "Repairs to cupboard under stairs");
            SetUpJobForExistingClient(setup, "Nicole Bryant", "221B Baker Street", "London", null, null, "Fill bullet holes in walls");
            SetUpJobForExistingClient(setup, "Nicole Bryant", "32 Windsor Gardens", "London", null, null, "Bear Proofing");
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }

        private static void SetUpJobForExistingClient(MockSetup setup, string clientNameMatch,
            string addressFirstLine, string addressSecondLine, string addressThirdLine, string addressPostCode, string description)
        {
            BrowseToClient(setup, clientNameMatch);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
            Assert.True(setup.ClientsPageViewModel.CanAddNewJob);
            setup.ClientsPageViewModel.AddNewJob();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.JobsPageViewModel));
            Assert.True(setup.JobsPageViewModel.UnderEdit);
            var j = setup.JobsPageViewModel.SelectedItem;
            Assert.NotEqual(ModelBase.NewModelId, j.ClientId);
            Assert.Equal(ModelBase.NewModelId, j.JobId);
            j.AddressFirstLine = addressFirstLine;
            j.AddressSecondLine = addressSecondLine;
            if (addressThirdLine != null) j.AddressThirdLine = addressThirdLine;
            if (addressPostCode != null) j.AddressPostCode = addressPostCode;
            j.Description = description;
            // Check it has a decent suggested job number.
            Assert.True(j.JobNumber >= 1);
            Assert.True(setup.JobsPageViewModel.CanSave);
            Assert.True(setup.JobsActionsPadViewModel.CanSaveItem);
            Assert.True(setup.JobsActionsPadViewModel.SaveItemIsVisible);
            setup.JobsActionsPadViewModel.SaveItem();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.JobsPageViewModel));
            Assert.True(setup.BrowserPaneViewModel.BrowsingEnabled);
            Assert.False(setup.JobsActionsPadViewModel.SaveItemIsVisible);
            Assert.True(setup.NavigationBarViewModel.CanBack);
            setup.NavigationBarViewModel.Back();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
        }

        public static void SetupJobChildren(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Job;
            Assert.Equal(6, setup.BrowserPaneViewModel.Items.Count);
            SetUpBillableHoursForExistingJob(setup, "Wayne Manor", "Some work that was done", 5);
            SetUpBillableHoursForExistingJob(setup, "221B Baker Street", "Drawing plans", 2);
            SetUpExpenseForExistingJob(setup, "Wayne Manor", ExpenseType.ApplicationFees, "Planning Application", 500);
            SetUpExpenseForExistingJob(setup, "32 Windsor Gardens", ExpenseType.TitleDeedsOrPlans, "Title Deeds", 250);
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }

        private static void SetUpBillableHoursForExistingJob(MockSetup setup, string jobNameMatch, string description, int numHoursWorked)
        {
            BrowseToJob(setup, jobNameMatch);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.JobsPageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.JobsActionsPadViewModel));
            setup.JobsPageViewModel.SelectedJobChild = JobChildExperience.BillableHours;
            Assert.True(Object.ReferenceEquals(setup.JobsPageViewModel.JobChildViewModel, setup.BillableHoursJobChildViewModel));
            Assert.True(setup.BillableHoursJobChildViewModel.EditCommand.CanExecute(null));
            setup.BillableHoursJobChildViewModel.EditCommand.Execute(null);
            Assert.False(setup.NavigationBarViewModel.CanBack);
            Assert.False(setup.NavigationBarViewModel.CanForward);
            Assert.False(setup.JobsActionsPadViewModel.CanEditItem);
            Assert.False(setup.JobsActionsPadViewModel.SaveItemIsVisible);
            Assert.Equal(ModelBase.NewModelId, setup.BillableHoursJobChildViewModel.SelectedItem.BillableHourId);
            setup.BillableHoursJobChildViewModel.SelectedItem.Description = description;
            setup.BillableHoursJobChildViewModel.SelectedItem.HoursWorked = numHoursWorked;
            Assert.True(setup.BillableHoursJobChildViewModel.SaveCommand.CanExecute(null));
            setup.BillableHoursJobChildViewModel.SaveCommand.Execute(null);
            Assert.True(setup.NavigationBarViewModel.CanBack);
            Assert.False(setup.NavigationBarViewModel.CanForward);
            Assert.True(setup.JobsActionsPadViewModel.CanEditItem);
            Assert.False(setup.JobsActionsPadViewModel.SaveItemIsVisible);
        }

        private static void SetUpExpenseForExistingJob(MockSetup setup, string jobNameMatch, ExpenseType expenseType, string description, double netAmount)
        {
            BrowseToJob(setup, jobNameMatch);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.JobsPageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.JobsActionsPadViewModel));
            setup.JobsPageViewModel.SelectedJobChild = JobChildExperience.Expenses;
            Assert.True(Object.ReferenceEquals(setup.JobsPageViewModel.JobChildViewModel, setup.ExpensesJobChildViewModel));
            Assert.True(setup.ExpensesJobChildViewModel.EditCommand.CanExecute(null));
            setup.ExpensesJobChildViewModel.EditCommand.Execute(null);
            Assert.False(setup.NavigationBarViewModel.CanBack);
            Assert.False(setup.NavigationBarViewModel.CanForward);
            Assert.False(setup.JobsActionsPadViewModel.CanEditItem);
            Assert.False(setup.JobsActionsPadViewModel.SaveItemIsVisible);
            Assert.Equal(ModelBase.NewModelId, setup.ExpensesJobChildViewModel.SelectedItem.ExpenseId);
            setup.ExpensesJobChildViewModel.SelectedItem.Type = expenseType;
            setup.ExpensesJobChildViewModel.SelectedItem.Description = description;
            setup.ExpensesJobChildViewModel.SelectedItem.NetAmount = netAmount;
            Assert.True(setup.ExpensesJobChildViewModel.SaveCommand.CanExecute(null));
            setup.ExpensesJobChildViewModel.SaveCommand.Execute(null);
            Assert.True(setup.NavigationBarViewModel.CanBack);
            Assert.False(setup.NavigationBarViewModel.CanForward);
            Assert.True(setup.JobsActionsPadViewModel.CanEditItem);
            Assert.False(setup.JobsActionsPadViewModel.SaveItemIsVisible);
        }

        public static void SetUpInvoices(MockSetup setup)
        {
            // Just invoice Rinsible Elk.
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Client;
            BrowseToClient(setup, "Rinsible Elk");
            Assert.True(setup.ClientsPageViewModel.CanInvoiceClient);
            setup.ClientsPageViewModel.InvoiceClient();
            Assert.False(setup.NavigationBarViewModel.CanHome);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.InvoicesPageViewModel));
            Assert.True(setup.InvoicesPageViewModel.Editable);
            Assert.False(setup.InvoicesPageViewModel.InvoiceEditable);
            // Select all of Rinsible's billables.
            Assert.Equal(2, setup.InvoicesPageViewModel.Jobs.Count);
            foreach (var job in setup.InvoicesPageViewModel.Jobs)
            {
                job.State = BillableModelState.FullyIncluded;
            }
            Assert.True(setup.InvoicesPageViewModel.CanSetBillableItems);
            setup.InvoicesPageViewModel.SetBillableItems();
            Assert.Equal(500, setup.InvoicesPageViewModel.NetAmount);
            Assert.Equal(5, setup.InvoicesPageViewModel.Hours);
            Assert.True(setup.InvoicesPageViewModel.InvoiceEditable);
            Assert.False(setup.InvoicesActionsPadViewModel.CanSaveItem);
            Assert.False(setup.InvoicesActionsPadViewModel.CanPrintItem);
            Assert.False(setup.InvoicesActionsPadViewModel.CanPaidItem);
            Assert.Equal(1, setup.InvoicesPageViewModel.SelectedItem.ClientId);
            Assert.Equal(1, setup.InvoicesPageViewModel.SelectedItem.ChildrenList.Count);
            Assert.True(String.IsNullOrEmpty(setup.InvoicesPageViewModel.SelectedItem.ChildrenList[0].Description));
            Assert.Equal(500, setup.InvoicesPageViewModel.SelectedItem.ChildrenList[0].NetAmount);
            setup.InvoicesPageViewModel.SelectedItem.ChildrenList[0].Description = "Some work that I did";
            Assert.False(setup.InvoicesActionsPadViewModel.CanSaveItem);
            Assert.False(setup.InvoicesActionsPadViewModel.CanPrintItem);
            setup.InvoicesPageViewModel.SelectedItem.ChildrenList[0].NetAmount = 600;
            Assert.False(setup.InvoicesActionsPadViewModel.CanSaveItem);
            Assert.False(setup.InvoicesActionsPadViewModel.CanPrintItem);
            setup.InvoicesPageViewModel.SelectedItem.Description = "A description for this new invoice";
            Assert.True(setup.InvoicesActionsPadViewModel.CanSaveItem);
            Assert.True(setup.InvoicesActionsPadViewModel.SaveItemIsVisible);
            Assert.True(setup.InvoicesActionsPadViewModel.CanPrintItem);
            setup.InvoicesActionsPadViewModel.PrintItem();
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }
    }
}
