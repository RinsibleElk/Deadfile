﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model.Browser;
using Deadfile.Model.Interfaces;
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
        }

        private static void BrowseToClient(MockSetup setup, string name)
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
                    Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.ClientsPageViewModel));
                    return;
                }
            }
        }

        public static void SetUpJobs(MockSetup setup)
        {
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
            BrowseToClient(setup, "Rinsible Elk");
            Assert.True(setup.NavigationBarViewModel.CanHome);
            setup.NavigationBarViewModel.Home();
            Assert.True(Object.ReferenceEquals(setup.TabViewModel.ContentArea, setup.HomePageViewModel));
        }
    }
}