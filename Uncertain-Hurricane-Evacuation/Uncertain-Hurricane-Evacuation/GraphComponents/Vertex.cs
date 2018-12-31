using System.Collections.Generic;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    class Vertex : IVertex
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vertex) obj);
        }

        protected bool Equals(Vertex other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public int Id { get; }
        public string Name => $"V{Id.ToString()}";
        public List<IEdge> Connectors { get; }
        public List<IVertex> Neighbors { get; }
        public double FloodingProbability { get; }

        public Vertex(int id) : this(id, 0)
        {
        }

        public Vertex(int id, double floodProbability)
        {
            Id = id;
            FloodingProbability = floodProbability;

            Connectors = new List<IEdge>();
            Neighbors = new List<IVertex>();
        }

        public static bool operator ==(Vertex v1, Vertex v2)
        {
            return v1 != null && v1.Equals(v2);
        }

        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }


        public override string ToString()
        {
            return $"V{Id}";
        }
    }
}
