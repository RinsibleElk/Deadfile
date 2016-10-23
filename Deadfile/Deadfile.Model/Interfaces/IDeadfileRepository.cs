using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Interfaces
{
    /// <summary>
    /// Repository for accessing data from the DeadfileContext.
    /// </summary>
    public interface IDeadfileRepository
    {
        /// <summary>
        /// Synchronous for now.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClientModel> GetClients();

        /// <summary>
        /// Synchronous server side filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<ClientModel> GetFilteredClients(string filter);

        /// <summary>
        /// Naughty method to seed the repository with fake data. Run at initialization time and only changes the database if it's totally empty.
        /// </summary>
        /// <remarks>
        /// This is for use in testing. It means that users can drop the whole database, run the application, and it will have put some data there.
        /// </remarks>
        void SetUpFakeData();

        /// <summary>
        /// Run on a one minute timer, used to retrieve a random quotation and display it to the user for a bit of fun.
        /// </summary>
        /// <returns></returns>
        QuotationModel GetRandomQuotation();
    }
}
