using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    public class JobTaskWithProperty
    {
        public string FullName { get; set; }
        public string Property { get; set; }
        public JobTask JobTask { get; set; }
    }
}
