using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCacheExperiment
{
    public class OutputNode
    {
        public OutputNode(double number, IReadOnlyList<OutputNode> children)
        {
            Number = number;
            Children = children;
        }
        public double Number { get; }
        public IReadOnlyList<OutputNode> Children { get; }
    }
}
