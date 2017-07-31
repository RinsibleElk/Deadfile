using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    /// <summary>
    /// Service for file handling. Used by the import/export pages and facilitates testing.
    /// </summary>
    public class DeadfileFileStreamService : IDeadfileFileStreamService
    {
        public Stream MakeWriteStream(FileInfo file)
        {
            return new FileStream(file.FullName, FileMode.Create, FileAccess.Write);
        }

        public TextWriter MakeWriter(Stream stream)
        {
            return new StreamWriter(stream);
        }

        public Stream MakeReadStream(FileInfo file)
        {
            return new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
        }

        public TextReader MakeReader(Stream stream)
        {
            return new StreamReader(stream);
        }
    }
}
