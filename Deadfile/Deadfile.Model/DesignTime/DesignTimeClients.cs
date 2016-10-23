using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.DesignTime
{
    public static class DesignTimeClients
    {
        public static IEnumerable<ClientModel> Clients
        {
            get
            {
                var entityClients = Client.GenerateFakeData();
                return entityClients.Select(MakeClientModel);
            }
        }

        private static ClientModel MakeClientModel(Client entityClient)
        {
            return new ClientModel()
            {
                ClientId = entityClient.ClientId,
                AddressFirstLine = entityClient.AddressFirstLine,
                AddressSecondLine = entityClient.AddressSecondLine,
                AddressThirdLine = entityClient.AddressThirdLine,
                AddressPostCode = entityClient.AddressPostCode,
                Title = entityClient.Title,
                FirstName = entityClient.FirstName,
                MiddleNames = entityClient.MiddleNames,
                LastName = entityClient.LastName,
                PhoneNumber1 = entityClient.PhoneNumber1,
                PhoneNumber2 = entityClient.PhoneNumber2,
                PhoneNumber3 = entityClient.PhoneNumber3,
                EmailAddress = entityClient.EmailAddress,
                Status = entityClient.Status,
                Notes = entityClient.Notes
            };
        }
    }
}
