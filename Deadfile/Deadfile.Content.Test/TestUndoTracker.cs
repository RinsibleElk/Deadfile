using System;
using System.Linq;
using Deadfile.Content.Undo;
using Deadfile.Model;
using Xunit;

namespace Deadfile.Content.Test
{
    public class TestUndoTracker
    {
        [Theory]
        [InlineData("ClientId", 12347)]
        [InlineData("Title", "Ms")]
        [InlineData("FirstName", "John")]
        [InlineData("MiddleNames", "Eugine")]
        [InlineData("LastName", "Roberts")]
        [InlineData("AddressFirstLine", "15 Yemen Road")]
        [InlineData("AddressSecondLine", "Yementown")]
        [InlineData("AddressThirdLine", "Saudi Arabia")]
        [InlineData("AddressPostCode", "EN1 2BB")]
        [InlineData("EmailAddress", "jack.bauer@yahoo.com")]
        [InlineData("PhoneNumber1", "07719535865")]
        [InlineData("PhoneNumber2", "02084568015")]
        [InlineData("PhoneNumber3", "02084568016")]
        [InlineData("Notes", "Here are some notes.")]
        public void TestUndoClient(string propertyName, object newValue)
        {
            var undoTracker = new UndoTracker<ClientModel>();
            var clientModel = MakeClientModel();
            undoTracker.Activate(clientModel);
            typeof(ClientModel).GetProperty(propertyName).SetMethod.Invoke(clientModel, new object[1] { newValue });
            Assert.True(undoTracker.CanUndo);
            Assert.False(undoTracker.CanRedo);
            undoTracker.Undo();
            undoTracker.Deactivate();
            var expectedClientModel = MakeClientModel();
            AssertClientModelsAreEqual(expectedClientModel, clientModel);
        }

        [Theory]
        [InlineData("ClientId", 12347)]
        [InlineData("Title", "Ms")]
        [InlineData("FirstName", "John")]
        [InlineData("MiddleNames", "Eugine")]
        [InlineData("LastName", "Roberts")]
        [InlineData("AddressFirstLine", "15 Yemen Road")]
        [InlineData("AddressSecondLine", "Yementown")]
        [InlineData("AddressThirdLine", "Saudi Arabia")]
        [InlineData("AddressPostCode", "EN1 2BB")]
        [InlineData("EmailAddress", "jack.bauer@yahoo.com")]
        [InlineData("PhoneNumber1", "07719535865")]
        [InlineData("PhoneNumber2", "02084568015")]
        [InlineData("PhoneNumber3", "02084568016")]
        [InlineData("Notes", "Here are some notes.")]
        public void TestRedoClient(string propertyName, object newValue)
        {
            var undoTracker = new UndoTracker<ClientModel>();
            var clientModel = MakeClientModel();
            undoTracker.Activate(clientModel);
            typeof(ClientModel).GetProperty(propertyName).SetMethod.Invoke(clientModel, new object[1] { newValue });
            Assert.True(undoTracker.CanUndo);
            Assert.False(undoTracker.CanRedo);
            undoTracker.Undo();
            Assert.True(undoTracker.CanRedo);
            Assert.False(undoTracker.CanUndo);
            undoTracker.Redo();
            undoTracker.Deactivate();
            var expectedClientModel = MakeClientModel();
            typeof(ClientModel).GetProperty(propertyName).SetMethod.Invoke(expectedClientModel, new object[1] { newValue });
            AssertClientModelsAreEqual(expectedClientModel, clientModel);
        }

        private static ClientModel MakeClientModel()
        {
            return new ClientModel()
            {
                ClientId = 12345,
                Title = "Mr",
                FirstName = "Jack",
                MiddleNames = "Evelyn",
                LastName = "Bauer",
                AddressFirstLine = "1 Yemen Road",
                AddressSecondLine = "Yementon",
                AddressThirdLine = "Yemen",
                AddressPostCode = "EN1 1AA",
                EmailAddress = "jack.bauer@gmail.com",
                PhoneNumber1 = "07719535864",
                PhoneNumber2 = "",
                PhoneNumber3 = "",
                Notes = ""
            };
        }

        private static void AssertClientModelsAreEqual(ClientModel expected, ClientModel actual)
        {
            Assert.Equal(expected.ClientId, actual.ClientId);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.MiddleNames, actual.MiddleNames);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.AddressFirstLine, actual.AddressFirstLine);
            Assert.Equal(expected.AddressSecondLine, actual.AddressSecondLine);
            Assert.Equal(expected.AddressThirdLine, actual.AddressThirdLine);
            Assert.Equal(expected.AddressPostCode, actual.AddressPostCode);
            Assert.Equal(expected.EmailAddress, actual.EmailAddress);
            Assert.Equal(expected.PhoneNumber1, actual.PhoneNumber1);
            Assert.Equal(expected.PhoneNumber2, actual.PhoneNumber2);
            Assert.Equal(expected.PhoneNumber3, actual.PhoneNumber3);
            Assert.Equal(expected.Notes, actual.Notes);
        }
    }
}
