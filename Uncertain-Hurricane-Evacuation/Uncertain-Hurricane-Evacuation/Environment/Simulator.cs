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

            Start();
        }

        private void Start()
        {
            Console.WriteLine(network);
            active = true;
            while (active)
            {
                int cmd;
                do
                {
                    Console.WriteLine("Please select one of the following:");
                    Console.WriteLine(string.Join("\n", commandsMapper.Select((tuple, i) => $"\t{i}. {tuple.Name}")));
                } while (!int.TryParse(Console.ReadLine(), out cmd));

                commandsMapper[cmd].Action();
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
            foreach (var v in graph.Vertices)
            {
                Console.WriteLine(ContainEvacuees(v));
            }
            foreach (var v in graph.Vertices)
            {
                Console.WriteLine(Flooded(v));
            }
        }

        public QueryResult ContainEvacuees(IVertex v)
        {
            var query = new Query(network.EvacueeNode(v), true);
            return EnumerationInference.EnumerationAsk(network, new List<Query>{query}, evidences)[0];
        }

        public QueryResult Flooded(IVertex v)
        {
            var query = new Query(network.FloodingNode(v), true);
            return EnumerationInference.EnumerationAsk(network, new List<Query> { query }, evidences)[0];
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
    }
}
