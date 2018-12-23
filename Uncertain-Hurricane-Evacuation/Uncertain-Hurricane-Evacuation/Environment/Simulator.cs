using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Uncertain_Hurricane_Evacuation.GraphComponents;
using Uncertain_Hurricane_Evacuation.Parser;
using Uncertain_Hurricane_Evacuation.Part1;
using Uncertain_Hurricane_Evacuation.Part2;
using Evidence = Uncertain_Hurricane_Evacuation.Part2.Evidence;

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
            graph = new GraphParser().ParseGraph(path);
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
                UserFunction.Of("What is the probability that a certain path is free from blockages?", null),
                UserFunction.Of(
                    "What is the path from a given location to a goal that has the highest probability of being free from blockages?",
                    null),
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

            var evidence = input[0];
            var report = Convert.ToBoolean(boolean);
            switch (evidence)
            {
                case fid:
                    evidences.Add(new Evidence(network.FloodingNode(graph.Vertex(id)), report));
                    break;
                case eid:
                    evidences.Add(new Evidence(network.EvacueeNode(graph.Vertex(id)), report));
                    break;
                case bid:
                    evidences.Add(new Evidence(network.BlockageNode(graph.Edge(id)), report));
                    break;
            }
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

        public List<QueryResult> QueryNodes<T>(List<T> nodes) where T : BayesianNode
        {
            return EnumerationInference.EnumerationAsk(
                network,
                nodes.Select(n => new Query(n, true)).ToList(),
                evidences);
        }

        private void Quit()
        {
            active = false;
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
            public Func<List<QueryResult>> Action { get; }

            private UserFunction(string name, Func<List<QueryResult>> action)
            {
                Name = name;
                Action = action;
            }

            public static UserFunction Of(string actionName, Func<List<QueryResult>> action)
            {
                return new UserFunction(actionName, action);
            }
        }
    }
}
