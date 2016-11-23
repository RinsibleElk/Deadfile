using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TreeCacheExperiment.Test
{
    public class QuickTest
    {
        private int numLeafMappings = 0;
        private int numAggregations = 0;

        [Fact]
        public void TestMethod1()
        {
            numLeafMappings = 0;
            numAggregations = 0;
            var cache = new TreeCache(Aggregator, LeafMapping);
            var inputTree_0_0 = new InputNode(7, new InputNode[0]);
            var inputTree_0_1 = new InputNode(5, new InputNode[0]);
            var inputTree_1_0 = new InputNode(8, new InputNode[0]);
            var inputTree_1_1 = new InputNode(17, new InputNode[0]);
            var inputTree_0 = new InputNode(0, new InputNode[] { inputTree_0_0, inputTree_0_1 });
            var inputTree_1 = new InputNode(0, new InputNode[] { inputTree_1_0, inputTree_1_1 });
            var inputTree_A = new InputNode(0, new InputNode[] {inputTree_0, inputTree_1});
            var outputTree_A = cache.Aggregate(inputTree_A);
            Assert.Equal(37, outputTree_A.Number);
            Assert.Equal(4, numLeafMappings);
            Assert.Equal(3, numAggregations);

            var inputTree_B = new InputNode(0, new InputNode[] { inputTree_0 });
            var outputTree_B = cache.Aggregate(inputTree_B);
            Assert.Equal(12, outputTree_B.Number);
            Assert.Equal(4, numLeafMappings);
            Assert.Equal(4, numAggregations);

            var inputTree_C = new InputNode(0, new InputNode[] { inputTree_1, inputTree_0 });
            var outputTree_C = cache.Aggregate(inputTree_C);
            Assert.Equal(37, outputTree_C.Number);
            Assert.Equal(6, numLeafMappings);
            Assert.Equal(6, numAggregations);
        }

        private OutputNode LeafMapping(InputNode inputLeaf)
        {
            ++numLeafMappings;
            return new OutputNode(inputLeaf.Number, new List<OutputNode>());
        }

        private OutputNode Aggregator(IReadOnlyList<OutputNode> children)
        {
            ++numAggregations;
            double total = 0;
            foreach (var outputNode in children)
            {
                total += outputNode.Number;
            }
            return new OutputNode(total, children);
        }
    }
}
