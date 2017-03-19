using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Xunit;

namespace Deadfile.Tab.Test.FunctionalTests
{
    public class TestEmptySetup
    {
        [Fact]
        public void TestStartsAtHome()
        {
            var setup = new MockSetup();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.HomeActionsPadViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
            Assert.True(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("Home", setup.TabViewModel.DisplayName);
        }

        [Fact]
        public void TestAddClient_NavigatesToClientsPage()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.AddClient();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.ClientsActionsPadViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
            Assert.True(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("New Client", setup.TabViewModel.DisplayName);
        }

        [Fact]
        public void TestAddClient_CanSave()
        {
            var setup = new MockSetup();
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

        [Fact]
        public void TestAddClient_Discard_GoesHome()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.AddClient();
            Assert.False(setup.ClientsActionsPadViewModel.CanSaveItem);
            Assert.True(setup.ClientsActionsPadViewModel.SaveItemIsVisible);
            Assert.True(setup.ClientsActionsPadViewModel.CanDiscardItem);
            Assert.True(setup.ClientsActionsPadViewModel.DiscardItemIsVisible);
            setup.ClientsActionsPadViewModel.DiscardItem();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            Assert.Equal("Home", setup.TabViewModel.DisplayName);
        }

        [Fact]
        public void TestAddQuotation_CanSave()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.DefineQuotations();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
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

        [Fact]
        public void TestAddQuotation_Discard()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.DefineQuotations();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
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

        [Fact]
        public void TestAddQuotation_Undo()
        {
            var setup = new MockSetup();
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

        [Fact]
        public void TestAddLocalAuthority_CanSave()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.LocalAuthorities();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
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

        [Fact]
        public void TestAddLocalAuthority_Discard()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.LocalAuthorities();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
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

        [Fact]
        public void TestDefineQuotations_Back()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.DefineQuotations();
            Assert.True(setup.NavigationBarViewModel.CanBack);
            Assert.False(setup.NavigationBarViewModel.CanForward);
            setup.NavigationBarViewModel.Back();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.HomeActionsPadViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
            Assert.True(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("Home", setup.TabViewModel.DisplayName);
            Assert.False(setup.NavigationBarViewModel.CanBack);
            Assert.True(setup.NavigationBarViewModel.CanForward);
            setup.NavigationBarViewModel.Forward();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.DefineQuotationsPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("Define Quotations", setup.TabViewModel.DisplayName);
        }

        [Fact]
        public void TestLocalAuthorities_Back()
        {
            var setup = new MockSetup();
            setup.HomePageViewModel.LocalAuthorities();
            Assert.True(setup.NavigationBarViewModel.CanBack);
            setup.NavigationBarViewModel.Back();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ActionsPad, setup.HomeActionsPadViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.BrowserPane, setup.BrowserPaneViewModel));
            Assert.True(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("Home", setup.TabViewModel.DisplayName);
            Assert.False(setup.NavigationBarViewModel.CanBack);
            Assert.True(setup.NavigationBarViewModel.CanForward);
            setup.NavigationBarViewModel.Forward();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.LocalAuthoritiesPageViewModel));
            Assert.Equal(null, setup.TabViewModel.ActionsPad);
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.NavigationBar, setup.NavigationBarViewModel));
            Assert.False(setup.TabViewModel.BrowserAndActionsAreVisible);
            Assert.Equal("Local Authorities", setup.TabViewModel.DisplayName);
        }

        [Fact]
        public void TestPopulateEntireDatabaseFromGui()
        {
            var setup = new MockSetup();
            Assert.Equal("Oliver Samson", setup.QuotesBarViewModel.Quotation.Author);
            Assert.Equal("No Quotations defined. Soz.", setup.QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpQuotations(setup);
            Assert.Equal(4, setup.Repository.GetQuotations(null).Count());
            setup.TimerService.FireCallback();
            Assert.Equal("Homer Simpson", setup.QuotesBarViewModel.Quotation.Author);
            Assert.Equal("You tried your best and you failed miserably. The lesson is, never try.", setup.QuotesBarViewModel.Quotation.Phrase);
            MockData.SetUpLocalAuthorities(setup);
            MockData.SetUpClients(setup);
            MockData.SetUpJobs(setup);
        }
    }
}
