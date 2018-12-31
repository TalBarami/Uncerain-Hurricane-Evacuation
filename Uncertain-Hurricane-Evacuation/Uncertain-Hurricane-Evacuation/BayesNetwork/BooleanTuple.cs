using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncertain_Hurricane_Evacuation.BayesNetwork
{
    public class BooleanTuple
    {
        public int Count => tuple.Count;
        private readonly List<bool> tuple;

        public IEnumerable<bool> Enumerable => tuple.AsEnumerable();
        public bool this[int i] => tuple[i];

        private BooleanTuple(params bool[] values)
        {
            tuple = values.ToList();
        }

        public static BooleanTuple Of(params bool[] values)
        {
            return new BooleanTuple(values);
        }

        public static BooleanTuple Of(int value, int size)
        {
            var str = Convert.ToString(value, 2);
            str = str.PadLeft(size, '0');

            var tuple = Of(str.Select(c => c != '0').ToArray());
            return tuple;
        }

        public BooleanTuple Flip(int i)
        {
            var newTuple = tuple.ToArray();
            newTuple[i] = !newTuple[i];
            return new BooleanTuple(newTuple);
        }

        public override string ToString()
        {
            return $"({string.Join(",", tuple.Select(b => b ? "T" : "F"))})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BooleanTuple other) || Count != other.Count)
            {
                return false;
            }

            for (var i = 0; i < Count; i++)
            {
                if (tuple[i] != other.tuple[i])
                {
                    return false;
                }
            }

            return true;

        }

        public override int GetHashCode()
        {
            return tuple.Aggregate(0, (i, b) => i + b.GetHashCode());
        }
    }
}
