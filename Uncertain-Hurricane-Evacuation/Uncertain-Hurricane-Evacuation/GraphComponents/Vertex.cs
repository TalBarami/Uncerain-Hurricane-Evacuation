using System.Collections.Generic;

namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    class Vertex : IVertex
    {
        public int Id { get; }
        public string Name => $"V{Id.ToString()}";
        public List<IEdge> Connectors { get; }
        public List<IVertex> Neighbors { get; }

        public bool Flood { get; }
        public bool Evacuee { get; }

        public double FloodProbability { get; }

        public Vertex(int id) : this(id, 0)
        {
        }

        public Vertex(int id, double floodProbability)
        {
            Id = id;
            FloodProbability = floodProbability;

            Connectors = new List<IEdge>();
            Neighbors = new List<IVertex>();
        }

        public override string ToString()
        {
            return $"V{Id}";
        }
        
        protected bool Equals(Vertex other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
