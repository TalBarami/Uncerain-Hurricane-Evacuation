namespace Uncertain_Hurricane_Evacuation.GraphComponents
{
    class Edge : IEdge
    {
        public int Id { get; }
        public string Name => $"E{Id.ToString()}";
        public IVertex V1 { get; }
        public IVertex V2 { get; }
        public double Weight { get; }
        public bool Blocked { get; }

        public Edge(int id, IVertex v1, IVertex v2, double weight)
        {
            Id = id;
            V1 = v1;
            V2 = v2;
            Weight = weight;
            
            V1.Neighbors.Add(V2);
            V2.Neighbors.Add(V1);
            V1.Connectors.Add(this);
            V2.Connectors.Add(this);
        }

        public override string ToString()
        {
            var b = Blocked ? "B" : "";
            return $"E{Id}({V1},{V2})W{Weight}{b}";
        }

        protected bool Equals(Edge other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
