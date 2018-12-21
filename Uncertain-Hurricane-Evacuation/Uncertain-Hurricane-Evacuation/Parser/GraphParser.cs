using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uncertain_Hurricane_Evacuation.GraphComponents;

namespace Uncertain_Hurricane_Evacuation.Parser
{
    public class GraphParser
    {
        private const string Vertex = "#V";
        private const string Edge = "#E";
        private const char Whitespace = ' ';
        private const char Comment = ';';

        public IGraph ParseGraph(string path)
        {
            return CreateGraphFromStringList(File.ReadAllLines(path).ToList());
        }

        public IGraph CreateGraphFromStringList(List<string> data)
        {
            var cleanData = data.Select(line => line.Split(Comment)[0].Trim()).Where(line => !string.IsNullOrEmpty(line)).ToList();

            var vertices = ParseVertices(cleanData.FindAll(line => line.StartsWith(Vertex)));
            var edges = ParseEdges(vertices, cleanData.FindAll(line => line.StartsWith(Edge)));

            return new Graph(vertices, edges);
        }

        private List<IVertex> ParseVertices(List<string> verticesData)
        {
            var sizeLine = verticesData[0];
            verticesData.RemoveAt(0);

            if (!int.TryParse(sizeLine.Split(Whitespace)[1], out var length))
            {
                throw new ParseException($"Unable to parse {sizeLine}");
            }

            var lines = verticesData.Select(line => line.Split(Whitespace)).ToList();
            var vertices = new List<IVertex>();
            for (var i=0; i<length; i++)
            {
                var vid = i + 1;
                var line = lines.FirstOrDefault(l => int.TryParse(l[1], out var id) && id == vid);

                if (line != null && double.TryParse(line[3], out var flood))
                {
                    vertices.Add(new Vertex(vid, flood));
                }
                else
                {
                    vertices.Add(new Vertex(vid));
                }
            }

            return vertices;
        }

        private List<IEdge> ParseEdges(List<IVertex> vertices, List<string> edgesData)
        {
            var split = edgesData.Select(line => line.Split(Whitespace)).ToList();
            var edges = new List<IEdge>();
            for (var i = 0; i < edgesData.Count; i++)
            {
                var s = split[i];
                if (!int.TryParse(s[0].Substring(2), out var id) ||
                    !int.TryParse(s[1], out var v1) ||
                    !int.TryParse(s[2], out var v2) ||
                    !double.TryParse(s[3].Substring(1), out var w))
                {
                    throw new ParseException(edgesData[i]);
                }

                edges.Add(new Edge(id, vertices.First(v => v.Id == v1), vertices.First(v => v.Id == v2), w));
            }

            return edges;
        }

        internal class ParseException : Exception
        {
            public ParseException(string message) : base(message)
            {
            }
        }
    }
}
