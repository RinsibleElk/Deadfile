using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCacheExperiment
{
    public class TreeCache
    {
        private readonly Func<IReadOnlyList<OutputNode>, OutputNode> _aggregator;
        private readonly Func<InputNode, OutputNode> _leafMapping;
        private Dictionary<InputNode, OutputNode> _cache = new Dictionary<InputNode, OutputNode>();
        private Dictionary<InputNode, OutputNode> _transientCache = new Dictionary<InputNode, OutputNode>();

        public TreeCache(Func<IReadOnlyList<OutputNode>, OutputNode> aggregator, Func<InputNode, OutputNode> leafMapping)
        {
            _aggregator = aggregator;
            _leafMapping = leafMapping;
        }

        public OutputNode Aggregate(InputNode inputTree)
        {
            if (_cache.ContainsKey(inputTree)) return _cache[inputTree];
            AggregateInner(inputTree);
            _cache = _transientCache;
            _transientCache = new Dictionary<InputNode, OutputNode>();
            return _cache[inputTree];
        }

        private void AggregateInner(InputNode inputTree)
        {
            OutputNode outputTree;
            if (inputTree.Children.Count == 0)
            {
                outputTree = _cache.ContainsKey(inputTree) ? _cache[inputTree] : _leafMapping(inputTree);
            }
            else
            {
                foreach (var inputNode in inputTree.Children)
                    AggregateInner(inputNode);
                outputTree = _cache.ContainsKey(inputTree) ? _cache[inputTree] : _aggregator(inputTree.Children.Select((inputNode) => _transientCache[inputNode]).ToList());
            }
            _transientCache.Add(inputTree, outputTree);
        }
    }
}
