using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockExcelService : IExcelService
    {
        public void Export(IDataGridPresenter dataGrid)
        {
        }
    }
}
