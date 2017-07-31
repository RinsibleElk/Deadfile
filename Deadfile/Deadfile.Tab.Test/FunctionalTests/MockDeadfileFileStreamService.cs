using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Tab.Test.FunctionalTests
{
    class MockDeadfileFileStreamService : IDeadfileFileStreamService
    {
        public Stream MakeWriteStream(FileInfo file)
        {
            return new MemoryStream();
        }

        public TextWriter MakeWriter(Stream stream)
        {
            return new StringWriter();
        }

        public Stream MakeReadStream(FileInfo file)
        {
            return new MemoryStream();
        }

        public TextReader MakeReader(Stream stream)
        {
            return new StringReader(_jsonToRead);
        }

        private string _jsonToRead;

        public void ExpectRead(string json)
        {
            _jsonToRead = json;
        }
    }
}
