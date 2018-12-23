using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uncertain_Hurricane_Evacuation.Environment;

namespace Uncertain_Hurricane_Evacuation.Part1
{
    public abstract class BayesianNode
    {
        public abstract string Name { get; }
        public List<BayesianNode> Parents;
        public int TupleSize => Parents.Count + 1;

        public  Dictionary<BooleanTuple, double> Table;
        protected BayesianNode()
        {
            Parents = new List<BayesianNode>();
            Table = new Dictionary<BooleanTuple, double>();
        }

        protected BooleanTuple NewTuple(int i = 0)
        {
            return BooleanTuple.Of(i, TupleSize);
        }

        protected void BuildTable()
        {
            if (Parents.Count > 0)
            {
                var length = Math.Pow(2, Parents.Count);
                for (var i = 0; i < length; i++)
                {
                    var tuple = NewTuple(i);
                    if (Table.ContainsKey(tuple))
                    {
                        continue;
                    }
                    var prod = 1.0;
                    for (var j = 1; j < TupleSize; j++)
                    {
                        if (!tuple[j])
                        {
                            continue;
                        }
                        var baseTuple = NewTuple().Flip(j);
                        prod = prod * Table[baseTuple];
                    }
                    Table.Add(tuple, prod);
                }
            }

            AddNegations();
        }

        private void AddNegations()
        {
            var negation = Table.ToDictionary(k => k.Key.Flip(0), v => 1 - v.Value);
            foreach (var keyValuePair in negation)
            {
                Table.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var kv in Table)
            {
                var negate = kv.Key[0] ? "" : "not ";
                sb.Append($"P({negate}{Name}");
                if (Parents.Count > 0)
                {
                    var parentValues = kv.Key.Enumerable.Select(b => b ? "" : "not ").ToList();
                    sb.Append(" | ");
                    sb.Append(string.Join(", ", Parents.Select((p, i) => $"{parentValues[i + 1]}{p.Name}")));
                }
                sb.AppendLine($") = {kv.Value}");
            }

            return sb.ToString();
        }
    }
}
