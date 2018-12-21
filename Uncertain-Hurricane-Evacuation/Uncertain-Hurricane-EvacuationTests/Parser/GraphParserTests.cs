using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uncertain_Hurricane_Evacuation.Parser;

namespace Uncertain_Hurricane_EvacuationTests.Parser
{
    [TestClass]
    public class GraphParserTests
    {
        [TestMethod]
        public void ParseGraphTest()
        {
            var graph = new GraphParser().ParseGraph("../../Resources/exampleGraph.txt");

            Assert.AreEqual(4, graph.Vertices.Count);
            Assert.AreEqual(4, graph.Edges.Count);

            Assert.AreEqual(0.2, graph.Vertex(1).FloodProbability);
            Assert.AreEqual(0.4, graph.Vertex(2).FloodProbability);
            Assert.AreEqual(0, graph.Vertex(3).FloodProbability);
            Assert.AreEqual(0, graph.Vertex(4).FloodProbability);

            Assert.AreEqual(graph.Vertex(1), graph.Edge(1).V1);
            Assert.AreEqual(graph.Vertex(2), graph.Edge(1).V2);
            Assert.AreEqual(1, graph.Edge(1).Weight);

            Assert.AreEqual(graph.Vertex(2), graph.Edge(2).V1);
            Assert.AreEqual(graph.Vertex(3), graph.Edge(2).V2);
            Assert.AreEqual(3, graph.Edge(2).Weight);

            Assert.AreEqual(graph.Vertex(3), graph.Edge(3).V1);
            Assert.AreEqual(graph.Vertex(4), graph.Edge(3).V2);
            Assert.AreEqual(3, graph.Edge(3).Weight);

            Assert.AreEqual(graph.Vertex(2), graph.Edge(4).V1);
            Assert.AreEqual(graph.Vertex(4), graph.Edge(4).V2);
            Assert.AreEqual(4, graph.Edge(4).Weight);

            Assert.IsTrue(graph.Vertex(1).Neighbors.Contains(graph.Vertex(2)));
            Assert.IsTrue(graph.Vertex(2).Neighbors.Contains(graph.Vertex(1)));
            Assert.IsTrue(graph.Vertex(1).Connectors.Contains(graph.Edge(1)));
            Assert.IsTrue(graph.Vertex(2).Connectors.Contains(graph.Edge(1)));

            Assert.IsTrue(graph.Vertex(2).Neighbors.Contains(graph.Vertex(3)));
            Assert.IsTrue(graph.Vertex(3).Neighbors.Contains(graph.Vertex(2)));
            Assert.IsTrue(graph.Vertex(2).Connectors.Contains(graph.Edge(2)));
            Assert.IsTrue(graph.Vertex(3).Connectors.Contains(graph.Edge(2)));

            Assert.IsTrue(graph.Vertex(3).Neighbors.Contains(graph.Vertex(4)));
            Assert.IsTrue(graph.Vertex(4).Neighbors.Contains(graph.Vertex(3)));
            Assert.IsTrue(graph.Vertex(3).Connectors.Contains(graph.Edge(3)));
            Assert.IsTrue(graph.Vertex(4).Connectors.Contains(graph.Edge(3)));

            Assert.IsTrue(graph.Vertex(2).Neighbors.Contains(graph.Vertex(4)));
            Assert.IsTrue(graph.Vertex(4).Neighbors.Contains(graph.Vertex(2)));
            Assert.IsTrue(graph.Vertex(2).Connectors.Contains(graph.Edge(4)));
            Assert.IsTrue(graph.Vertex(4).Connectors.Contains(graph.Edge(4)));
        }
    }
}