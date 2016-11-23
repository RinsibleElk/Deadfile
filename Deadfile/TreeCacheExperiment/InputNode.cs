using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCacheExperiment
{
    public class InputNode
    {
        public InputNode(int number, IReadOnlyList<InputNode> children)
        {
            Number = number;
            Children = children;
        }
        public int Number { get; }
        public IReadOnlyList<InputNode> Children { get; }
    }
}
