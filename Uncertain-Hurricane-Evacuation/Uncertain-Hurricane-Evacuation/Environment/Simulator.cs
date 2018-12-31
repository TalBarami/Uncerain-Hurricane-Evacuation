using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Uncertain_Hurricane_Evacuation.BayesNetwork;
using Uncertain_Hurricane_Evacuation.Enumeration;
using Uncertain_Hurricane_Evacuation.GraphComponents;
using Uncertain_Hurricane_Evacuation.Parser;
using Evidence = Uncertain_Hurricane_Evacuation.Enumeration.Evidence;

namespace Uncertain_Hurricane_Evacuation.Environment
{
    class Simulator
    {
        private readonly IGraph graph;
        private readonly BayesianNetwork network;
        private readonly List<Evidence> evidences;
        private readonly UserAction[] commandsMapper;
        private readonly UserFunction[] probabilisticReasoning;

        private bool active;

        public Simulator(string path)
        {
            graph = new FileParser().ParseFile(path);
            network = new BayesianNetwork(graph);
            evidences = new List<Evidence>();

            commandsMapper = new[]
            {
                UserAction.Of("Reset", Reset),
                UserAction.Of("Add Evidence", AddEvidence),
                UserAction.Of("Probabilistic Reasoning", ProbabilisticReasoning),
                UserAction.Of("Quit", Quit)
            };

            probabilisticReasoning = new[]
            {
                UserFunction.Of("What is the probability that each of the vertices contains evacuees?", () => QueryNodes(network.EvacueeNodes)),
                UserFunction.Of("What is the probability that each of the vertices is flooded?", () => QueryNodes(network.FloodingNodes)),
                UserFunction.Of("What is the probability that each of the edges is blocked?", () => QueryNodes(network.BlockageNodes)),
                UserFunction.Of("What is the probability that a certain path is free from blockages?", IsPathFree),
                UserFunction.Of(
                    "What is the path from a given location to a goal that has the highest probability of being free from blockages?",
                    BestPath),
                UserFunction.Of("All", () =>
                    probabilisticReasoning[0].Action()
                        .Concat(probabilisticReasoning[1].Action()).ToList()
                        .Concat(probabilisticReasoning[2].Action()).ToList())
            };

            Start();
        }

        private void Start()
        {
            Console.WriteLine(graph);
            Console.WriteLine(network);
            active = true;
            while (active)
            {
                int cmd;
                do
                {
                    Console.WriteLine("Please select one of the following:");
                    Console.WriteLine(string.Join("\n", commandsMapper.Select((tuple, i) => $"\t{i+1}. {tuple.Name}")));
                } while (!int.TryParse(Console.ReadLine(), out cmd) || cmd < 1 || cmd > commandsMapper.Length);

                commandsMapper[cmd-1].Action();
            }
        }

        private void Reset()
        {
            evidences.Clear();
            Console.WriteLine("Cleared evidences.");
        }

        private void AddEvidence()
        {
            string[] input;
            int id, boolean;
            do
            {
                Console.WriteLine(requestEvidence);
                input = Console.ReadLine()?.Split(' ');
            } while (input == null ||
                     input.Length < 3 ||
                     !$"{fid} {bid} {eid}".Split(' ').Contains(input[0]) ||
                     !int.TryParse(input[1], out boolean) ||
                     !int.TryParse(input[2], out id));

            var evidenceId = input[0];
            var report = Convert.ToBoolean(boolean);
            Evidence evidence;
            switch (evidenceId)
            {
                case fid:
                    evidence = new Evidence(network.FloodingNode(graph.Vertex(id)), report);
                    break;
                case eid:
                    evidence = new Evidence(network.EvacueeNode(graph.Vertex(id)), report);
                    break;
                case bid:
                    evidence = new Evidence(network.BlockageNode(graph.Edge(id)), report);
                    break;
                default:
                    throw new Exception("Shouldn't happen");
            }

            var ev = evidences.FirstOrDefault(e => e.Node == evidence.Node);
            if (ev != null)
            {
                Console.WriteLine($"Evidences already contains {ev}, replacing with {evidence}");
                evidences.Remove(ev);
            }
            evidences.Add(evidence);
            Console.WriteLine($"Evidences: {string.Join(", ", evidences)}");
        }

        private void ProbabilisticReasoning()
        {
            int cmd;
            do
            {
                Console.WriteLine("Choose your query:");
                Console.WriteLine(string.Join("\n", probabilisticReasoning.Select((tuple, i) => $"\t{i+1}. {tuple.Name}")));
            } while (!int.TryParse(Console.ReadLine(), out cmd) || cmd < 1 || cmd > probabilisticReasoning.Length);

            var result = probabilisticReasoning[cmd-1].Action();
            result.ForEach(Console.WriteLine);
        }

        public List<IQueryResult> QueryNodes<T>(List<T> nodes) where T : BayesianNode
        {
            return nodes.Select(node => new Query(node, true))
                .Select(query => EnumerationInference.EnumerationAsk(network, query, evidences))
                .ToList();
        }

        private void Quit()
        {
            active = false;
        }

        private List<IQueryResult> IsPathFree()
        {
            string[] line;
            do
            {
                Console.WriteLine("Enter path: <e1> <e2> ...");
                line = Console.ReadLine()?.Split(' ');
            } while (line == null || line.Length == 0 || line.Any(e => !int.TryParse(e, out _)));

            return new List<IQueryResult>
            {
                EnumerationInference.EnumerationAsk(network,
                    line.Select(int.Parse).Select(graph.Edge).Select(e => new Query(network.BlockageNode(e), false))
                        .ToList(),
                    evidences)
            };
        }

        private List<IQueryResult> BestPath()
        {
            string[] line;
            int id1, id2;
            do
            {
                Console.WriteLine("Select two nodes: <v1> <v2>");
                line = Console.ReadLine()?.Split(' ');
            } while (line == null || line.Length != 2 || !int.TryParse(line[0], out id1) || !int.TryParse(line[1], out id2)
                     || graph.Vertices.All(v => v.Id != id1) || graph.Vertices.All(v => v.Id != id2));

            var v1 = graph.Vertex(id1);
            var v2 = graph.Vertex(id2);
            var paths = graph.Dfs(v1, v2).Select(path => path.Edges).ToList();
            var results = paths.Select(edges => EnumerationInference.EnumerationAsk(network,
                    edges.Select(e => new Query(network.BlockageNode(e), false)).ToList(),
                    evidences)).ToList();
            var max = results.Max(r => r.Result);
            return results.Where(r => Math.Abs(r.Result - max) < double.Epsilon).ToList();
        }

        private readonly string requestEvidence = $"Add evidence using the format: <{fid}/{eid}/{bid}> <{t}/{f}> <id>";
        private const string fid = "f";
        private const string eid = "e";
        private const string bid = "b";
        private const string t = "1";
        private const string f = "0";

        private class UserAction
        {
            public string Name { get; }
            public Action Action { get; }

            private UserAction(string name, Action action)
            {
                Name = name;
                Action = action;
            }

            public static UserAction Of(string actionName, Action action)
            {
                return new UserAction(actionName, action);
            }
        }

        private class UserFunction
        {
            public string Name { get; }
            public Func<List<IQueryResult>> Action { get; }

            private UserFunction(string name, Func<List<IQueryResult>> action)
            {
                Name = name;
                Action = action;
            }

            public static UserFunction Of(string actionName, Func<List<IQueryResult>> action)
            {
                return new UserFunction(actionName, action);
            }
        }
    }
}
