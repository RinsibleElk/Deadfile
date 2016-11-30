using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.UndoRedo
{
    public sealed class UndoValue
    {
        public PropertyInfo Property { get; set; }
        public object PreviousValue { get; set; }
        public object NewValue { get; set; }
        public int? Context { get; set; }
        public UndoType Type { get; set; }
    }
}
