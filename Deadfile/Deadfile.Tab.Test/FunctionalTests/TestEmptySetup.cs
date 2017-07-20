using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Deadfile.Entity;
using Deadfile.Model;
using Deadfile.Model.Billable;
using Deadfile.Model.Browser;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestEmptySetup
    {
        [Fact]
        public void TestStartsAtHome()
        {
            using (var setup = new MockSetup())
            {
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, null));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.True(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Home", setup.TabViewModel.DisplayName);
            }
        }

        [Fact]
        public void TestAddClient_NavigatesToClientsPage()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.AddClient();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.ClientsActionsPadViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
                Assert.True(setup.TabViewModel.ActionsPadIsVisible);
                Assert.True(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("New Client", setup.TabViewModel.DisplayName);
            }
        }

        [Fact]
        public void TestAddClient_CanSave()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.AddClient();
                Assert.True(setup.ClientsPageViewModel.UnderEdit);
                Assert.False(setup.ClientsPageViewModel.CanSave);
                setup.ClientsPageViewModel.SelectedItem.Title = "Mr";
                setup.ClientsPageViewModel.SelectedItem.FirstName = "Rinsible";
                setup.ClientsPageViewModel.SelectedItem.LastName = "Elk";
                setup.ClientsPageViewModel.SelectedItem.AddressFirstLine = "4 Privet Drive";
                setup.ClientsPageViewModel.SelectedItem.EmailAddress = "rinsible.elk@gmail.com";
                setup.ClientsPageViewModel.SelectedItem.PhoneNumber1 = "07132465890";
                Assert.True(setup.ClientsPageViewModel.CanSave);
                Assert.True(setup.ClientsActionsPadViewModel.CanSaveItem);
                Assert.True(setup.ClientsActionsPadViewModel.SaveItemIsVisible);
                setup.ClientsActionsPadViewModel.SaveItem();
                Assert.Equal(1, setup.Repository.GetClients().Count());
            }
        }

        [Fact]
        public void TestAddClient_Discard_GoesHome()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.AddClient();
                Assert.False(setup.ClientsActionsPadViewModel.CanSaveItem);
                Assert.True(setup.ClientsActionsPadViewModel.SaveItemIsVisible);
                Assert.True(setup.ClientsActionsPadViewModel.CanDiscardItem);
                Assert.True(setup.ClientsActionsPadViewModel.DiscardItemIsVisible);
                setup.ClientsActionsPadViewModel.DiscardItem();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
                Assert.Equal("Home", setup.TabViewModel.DisplayName);
            }
        }

        [Fact]
        public void TestAddQuotation_CanSave()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.DefineQuotations();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Define Quotations", setup.TabViewModel.DisplayName);
                Assert.Equal(1, setup.DefineQuotationsPageViewModel.Items.Count);
                Assert.Equal(ModelBase.NewModelId, setup.DefineQuotationsPageViewModel.Items[0].QuotationId);
                Assert.False(setup.DefineQuotationsPageViewModel.Editable);
                Assert.True(setup.DefineQuotationsPageViewModel.EditCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
                Assert.True(setup.DefineQuotationsPageViewModel.Editable);
                Assert.False(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.Items[0].Author = "Rinsible Elk";
                Assert.False(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.Items[0].Phrase = "This is a test";
                Assert.True(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.SaveCommand.Execute(null);
                Assert.False(setup.DefineQuotationsPageViewModel.Editable);
                Assert.Equal(1, setup.Repository.GetQuotations(null).Count());
                Assert.Equal(2, setup.DefineQuotationsPageViewModel.Items.Count);
            }
        }

        [Fact]
        public void TestAddQuotation_Discard()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.DefineQuotations();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Define Quotations", setup.TabViewModel.DisplayName);
                Assert.Equal(1, setup.DefineQuotationsPageViewModel.Items.Count);
                Assert.Equal(ModelBase.NewModelId, setup.DefineQuotationsPageViewModel.Items[0].QuotationId);
                Assert.False(setup.DefineQuotationsPageViewModel.Editable);
                Assert.True(setup.DefineQuotationsPageViewModel.EditCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
                Assert.True(setup.DefineQuotationsPageViewModel.Editable);
                Assert.True(setup.DefineQuotationsPageViewModel.DiscardCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.Items[0].Author = "Rinsible Elk";
                Assert.True(setup.DefineQuotationsPageViewModel.DiscardCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.Items[0].Phrase = "This is a test";
                Assert.True(setup.DefineQuotationsPageViewModel.DiscardCommand.CanExecute(null));
                setup.DefineQuotationsPageViewModel.DiscardCommand.Execute(null);
                Assert.False(setup.DefineQuotationsPageViewModel.Editable);
                Assert.Equal(1, setup.DefineQuotationsPageViewModel.Items.Count);
            }
        }

        [Fact]
        public void TestAddQuotation_Undo()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.DefineQuotations();
                setup.DefineQuotationsPageViewModel.EditCommand.Execute(null);
                setup.DefineQuotationsPageViewModel.Items[0].Author = "Rinsible Elk";
                setup.DefineQuotationsPageViewModel.Items[0].Phrase = "This is a test";
                Assert.True(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                Assert.True(setup.NavigationBarViewModel.CanUndo);
                Assert.False(setup.NavigationBarViewModel.CanRedo);
                setup.NavigationBarViewModel.Undo();
                Assert.Equal("Rinsible Elk", setup.DefineQuotationsPageViewModel.Items[0].Author);
                Assert.Equal(null, setup.DefineQuotationsPageViewModel.Items[0].Phrase);
                Assert.False(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                Assert.True(setup.NavigationBarViewModel.CanUndo);
                Assert.True(setup.NavigationBarViewModel.CanRedo);
                setup.NavigationBarViewModel.Undo();
                Assert.Equal(null, setup.DefineQuotationsPageViewModel.Items[0].Author);
                Assert.Equal(null, setup.DefineQuotationsPageViewModel.Items[0].Phrase);
                Assert.False(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                Assert.False(setup.NavigationBarViewModel.CanUndo);
                Assert.True(setup.NavigationBarViewModel.CanRedo);
                setup.NavigationBarViewModel.Redo();
                Assert.Equal("Rinsible Elk", setup.DefineQuotationsPageViewModel.Items[0].Author);
                Assert.Equal(null, setup.DefineQuotationsPageViewModel.Items[0].Phrase);
                Assert.False(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                Assert.True(setup.NavigationBarViewModel.CanUndo);
                Assert.True(setup.NavigationBarViewModel.CanRedo);
                setup.NavigationBarViewModel.Redo();
                Assert.Equal("Rinsible Elk", setup.DefineQuotationsPageViewModel.Items[0].Author);
                Assert.Equal("This is a test", setup.DefineQuotationsPageViewModel.Items[0].Phrase);
                Assert.True(setup.DefineQuotationsPageViewModel.SaveCommand.CanExecute(null));
                Assert.True(setup.NavigationBarViewModel.CanUndo);
                Assert.False(setup.NavigationBarViewModel.CanRedo);
            }
        }

        [Fact]
        public void TestAddLocalAuthority_CanSave()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.LocalAuthorities();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Local Authorities", setup.TabViewModel.DisplayName);
                Assert.Equal(1, setup.LocalAuthoritiesPageViewModel.Items.Count);
                Assert.Equal(ModelBase.NewModelId, setup.LocalAuthoritiesPageViewModel.Items[0].LocalAuthorityId);
                Assert.False(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.True(setup.LocalAuthoritiesPageViewModel.EditCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
                Assert.True(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.False(setup.LocalAuthoritiesPageViewModel.SaveCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.Items[0].Name = "London Borough of Enfield";
                Assert.True(setup.LocalAuthoritiesPageViewModel.SaveCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.Items[0].Url = "http://enfield.gov.uk";
                Assert.True(setup.LocalAuthoritiesPageViewModel.SaveCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.SaveCommand.Execute(null);
                Assert.False(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.Equal(1, setup.Repository.GetLocalAuthorities(null).Count());
                Assert.Equal(2, setup.LocalAuthoritiesPageViewModel.Items.Count);
            }
        }

        [Fact]
        public void TestAddLocalAuthority_Discard()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.LocalAuthorities();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Local Authorities", setup.TabViewModel.DisplayName);
                Assert.Equal(1, setup.LocalAuthoritiesPageViewModel.Items.Count);
                Assert.Equal(ModelBase.NewModelId, setup.LocalAuthoritiesPageViewModel.Items[0].LocalAuthorityId);
                Assert.False(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.True(setup.LocalAuthoritiesPageViewModel.EditCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.EditCommand.Execute(null);
                Assert.True(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.True(setup.LocalAuthoritiesPageViewModel.DiscardCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.Items[0].Name = "London Borough of Enfield";
                Assert.True(setup.LocalAuthoritiesPageViewModel.DiscardCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.Items[0].Url = "http://enfield.gov.uk";
                Assert.True(setup.LocalAuthoritiesPageViewModel.DiscardCommand.CanExecute(null));
                setup.LocalAuthoritiesPageViewModel.DiscardCommand.Execute(null);
                Assert.False(setup.LocalAuthoritiesPageViewModel.Editable);
                Assert.Equal(1, setup.LocalAuthoritiesPageViewModel.Items.Count);
            }
        }

        [Fact]
        public void TestDefineQuotations_Back()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.DefineQuotations();
                Assert.True(setup.NavigationBarViewModel.CanBack);
                Assert.False(setup.NavigationBarViewModel.CanForward);
                setup.NavigationBarViewModel.Back();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, null));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.True(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Home", setup.TabViewModel.DisplayName);
                Assert.False(setup.NavigationBarViewModel.CanBack);
                Assert.True(setup.NavigationBarViewModel.CanForward);
                setup.NavigationBarViewModel.Forward();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Define Quotations", setup.TabViewModel.DisplayName);
            }
        }

        [Fact]
        public void TestLocalAuthorities_Back()
        {
            using (var setup = new MockSetup())
            {
                setup.HomePageViewModel.LocalAuthorities();
                Assert.True(setup.NavigationBarViewModel.CanBack);
                setup.NavigationBarViewModel.Back();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.True(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Home", setup.TabViewModel.DisplayName);
                Assert.False(setup.NavigationBarViewModel.CanBack);
                Assert.True(setup.NavigationBarViewModel.CanForward);
                setup.NavigationBarViewModel.Forward();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
                Assert.Equal(null, setup.TabViewModel.ActionsPad);
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
                Assert.False(setup.TabViewModel.ActionsPadIsVisible);
                Assert.False(setup.TabViewModel.BrowserPaneIsVisible);
                Assert.Equal("Local Authorities", setup.TabViewModel.DisplayName);
            }
        }

        private void PopulateEntireDatabaseFromGui(MockSetup setup)
        {
            Assert.Equal("Oliver Samson", setup.QuotesBarViewModel.Quotation.Author);
            Assert.Equal("No Quotations defined. Soz.", setup.QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpQuotations(setup);
            Assert.Equal(4, setup.Repository.GetQuotations(null).Count());
            setup.TimerService.FireCallback();
            Assert.Equal("Homer Simpson", setup.QuotesBarViewModel.Quotation.Author);
            Assert.Equal("You tried your best and you failed miserably. The lesson is, never try.",
                setup.QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpLocalAuthorities(setup);
            MockData.SetUpClients(setup);
            MockData.SetUpJobs(setup);
            MockData.SetupJobChildren(setup);
            MockData.SetUpInvoices(setup);
        }

        [Fact]
        public void TestPopulateEntireDatabaseFromGui()
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);
            }
        }

        [Fact]
        public void TestPayInvoice()
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);
                // Navigate to the invoice via the browser.
                setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Client;
                Assert.Equal(6, setup.BrowserPaneViewModel.Items.Count);
                var rinsibleElk = setup.BrowserPaneViewModel.Items.First((b) => ((BrowserClient) b).FullName.Contains("Rinsible Elk"));
                rinsibleElk.IsExpanded = true;
                Assert.Equal(2, rinsibleElk.Children.Count);
                var secondJob = rinsibleElk.Children[1];
                secondJob.IsExpanded = true;
                var invoice = secondJob.Children[0];
                setup.BrowserPaneViewModel.SelectedItem = invoice;
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.InvoicesPageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.InvoicesActionsPadViewModel));
                Assert.True(setup.InvoicesActionsPadViewModel.CanPaidItem);
                setup.InvoicesActionsPadViewModel.PaidItem();
                Assert.Equal(InvoiceStatus.Paid, setup.InvoicesPageViewModel.SelectedItem.Status);
                Assert.Equal(InvoiceStatus.Paid,
                    setup.Repository.GetInvoiceById(setup.InvoicesPageViewModel.SelectedItem.InvoiceId).Status);
            }
        }

        [Fact]
        public void TestCancelInvoice()
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);
                // Navigate to the invoice via the browser.
                setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Client;
                Assert.Equal(6, setup.BrowserPaneViewModel.Items.Count);
                var rinsibleElk =
                    setup.BrowserPaneViewModel.Items.First((b) => ((BrowserClient) b).FullName.Contains("Rinsible Elk"));
                rinsibleElk.IsExpanded = true;
                Assert.Equal(2, rinsibleElk.Children.Count);
                var secondJob = rinsibleElk.Children[1];
                secondJob.IsExpanded = true;
                var invoice = secondJob.Children[0];
                setup.BrowserPaneViewModel.SelectedItem = invoice;
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.InvoicesPageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.InvoicesActionsPadViewModel));
                Assert.True(setup.InvoicesActionsPadViewModel.CanDeleteItem);
                Assert.True(setup.InvoicesActionsPadViewModel.DeleteItemIsVisible);
                setup.InvoicesActionsPadViewModel.DeleteItem();
                Assert.Equal(InvoiceStatus.Cancelled, setup.InvoicesPageViewModel.SelectedItem.Status);
                Assert.Equal(InvoiceStatus.Cancelled,
                    setup.Repository.GetInvoiceById(setup.InvoicesPageViewModel.SelectedItem.InvoiceId).Status);
            }
        }

        [Fact]
        public void TestAddNewInvoice_Save_StaysOnPageButCanEdit()
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);
                setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Client;
                MockData.BrowseToClient(setup, "Nicole Bryant");
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
                Assert.True(setup.ClientsPageViewModel.CanInvoiceClient);
                setup.ClientsPageViewModel.InvoiceClient();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.InvoicesPageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.InvoicesActionsPadViewModel));
                Assert.True(setup.InvoicesPageViewModel.UnderEdit);
                Assert.False(setup.InvoicesActionsPadViewModel.CanSaveItem);
                Assert.False(setup.InvoicesActionsPadViewModel.CanPrintItem);
                foreach (var job in setup.InvoicesPageViewModel.Jobs)
                {
                    job.State = BillableModelState.FullyIncluded;
                }
                Assert.True(setup.InvoicesPageViewModel.CanSetBillableItems);
                setup.InvoicesPageViewModel.SetBillableItems();
                Assert.False(setup.InvoicesActionsPadViewModel.CanSaveItem);
                setup.InvoicesPageViewModel.SelectedItem.ChildrenList[0].Description = "Some description";
                setup.InvoicesPageViewModel.SelectedItem.Description = "Some description";
                Assert.True(setup.InvoicesPageViewModel.CanSave);
                Assert.True(setup.InvoicesActionsPadViewModel.CanSaveItem);
                Assert.True(setup.InvoicesActionsPadViewModel.CanPrintItem);
                setup.InvoicesActionsPadViewModel.SaveItem();
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.InvoicesPageViewModel));
                Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.InvoicesActionsPadViewModel));
                Assert.False(setup.InvoicesPageViewModel.UnderEdit);
                Assert.True(setup.InvoicesActionsPadViewModel.CanEditItem);
            }
        }

        [Theory]
        [InlineData("Webfoot")]
        [InlineData("1313 Webfoot Walk")]
        [InlineData("1313")]
        [InlineData("2502")]
        [InlineData("Walk")]
        public void TestSearchForJob(string searchText)
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);
                setup.NavigationBarViewModel.SearchText = searchText;
                Assert.True(setup.NavigationBarViewModel.IsSearchShown);
                Assert.Equal(1, setup.NavigationBarViewModel.SearchResults.Count);
                Assert.True(setup.NavigationBarViewModel.SearchResults[0] is BrowserJob);
                var browserJob = setup.NavigationBarViewModel.SearchResults[0] as BrowserJob;
                Assert.NotNull(browserJob);
                Assert.Equal("1313 Webfoot Walk", browserJob.FullAddress);
                Assert.Equal(2502, browserJob.JobNumber);
            }
        }

        [Fact]
        public void TestCannotAddJobChildWhenCreatingNewJob()
        {
            using (var setup = new MockSetup())
            {
                PopulateEntireDatabaseFromGui(setup);

                // Navigate to Rinsible Elk via the browser.
                setup.BrowserPaneViewModel.BrowserSettings.Mode = BrowserMode.Client;
                Assert.Equal(6, setup.BrowserPaneViewModel.Items.Count);
                var rinsibleElk = setup.BrowserPaneViewModel.Items.First((b) => ((BrowserClient)b).FullName.Contains("Rinsible Elk"));
                setup.BrowserPaneViewModel.SelectedItem = rinsibleElk;
                Assert.Equal(setup.ClientsPageViewModel, setup.TabViewModel.ContentArea);
                Assert.True(setup.ClientsPageViewModel.CanAddNewJob);

                // Add a new job.
                setup.ClientsPageViewModel.AddNewJob();
                Assert.Equal(setup.JobsPageViewModel, setup.TabViewModel.ContentArea);

                // While editing a job, it is not allowed to edit any of the job children.
                Assert.Equal(setup.JobTasksJobChildViewModel, setup.JobsPageViewModel.JobChildViewModel);
                Assert.False(setup.JobTasksJobChildViewModel.EditCommand.CanExecute(null));

                // Now fix up and save the job.
                Assert.Equal(setup.JobsActionsPadViewModel, setup.TabViewModel.ActionsPad);
                setup.JobsPageViewModel.SelectedItem.Description = "Some description";
                setup.JobsPageViewModel.SelectedItem.AddressFirstLine = "123 The Death Star";
                Assert.True(setup.JobsActionsPadViewModel.CanSaveItem);
                Assert.True(setup.JobsActionsPadViewModel.SaveItemIsVisible);
                setup.JobsActionsPadViewModel.SaveItem();

                // Now it is allowed to edit the job children.
                Assert.True(setup.JobTasksJobChildViewModel.EditCommand.CanExecute(null));
            }
        }
    }
}
