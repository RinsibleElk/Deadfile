using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Deadfile.Pdf
{
    public interface IDocumentPresenter
    {
        FixedDocument Document { get; }
    }
}
