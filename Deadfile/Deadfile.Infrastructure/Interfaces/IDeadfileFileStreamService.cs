using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    /// <summary>
    /// Service for file handling. Used by the import/export pages and facilitates testing.
    /// </summary>
    public interface IDeadfileFileStreamService
    {
        Stream MakeWriteStream(FileInfo file);
        TextWriter MakeWriter(Stream stream);
        Stream MakeReadStream(FileInfo file);
        TextReader MakeReader(Stream stream);
    }
}
