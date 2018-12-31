using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uncertain_Hurricane_Evacuation.BayesNetwork;
using Uncertain_Hurricane_Evacuation.GraphComponents;
using Uncertain_Hurricane_Evacuation.Parser;

namespace Uncertain_Hurricane_EvacuationTests.Part1
{
    [TestClass]
    public class BayesianNetworkTests
    {
        private IGraph graph;
        private BayesianNetwork network;

        [TestInitialize]
        public void SetUp()
        {
            graph = new FileParser().ParseFile("../../Resources/exampleGraph.txt");
            network = new BayesianNetwork(graph);
        }

        [TestMethod]
        public void FloodNodeTest()
        {
            var probabilities = new List<double>
            {
                0.2,
                0.4,
                0,
                0
            };
            for (var i = 0; i < 4; i++)
            {
                var v = graph.Vertex(i+1);
                Assert.AreEqual(v, network.FloodingNode(v).V);
                var table = network.FloodingNode(v).Table;
                Assert.AreEqual(2, network.FloodingNode(v).Table.Count);
                Assert.IsTrue(table.ContainsKey(BooleanTuple.Of(true)));
                Assert.IsTrue(table.ContainsKey(BooleanTuple.Of(false)));
                Assert.AreEqual(probabilities[i], table[BooleanTuple.Of(true)]);
                Assert.AreEqual(1 - probabilities[i], table[BooleanTuple.Of(false)]);
            }
        }

        [TestMethod]
        public void BayesianNetworkTest()
        {
            Console.WriteLine(network.ToString());
        }
    }
}