using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.Dijkstra
{
    class Dijkstra
    {
        private List<Node> _nodes;
        private List<Edge> _edges;
        private List<Node> _basis;
        private Dictionary<string, double> _dist;
        private Dictionary<string, Node> _previous;

	    private Node _startNode;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="edges">Liste aller Kanten</param>
        /// <param name="nodes">Liste aller Knoten</param>
        public Dijkstra(List<Edge> edges, List<Node> nodes)
        {
            Edges = edges;
            Nodes = nodes;
            Basis = new List<Node>();
            Dist = new Dictionary<string, double>();
            Previous = new Dictionary<string, Node>();

            // Knoten aufnehmen
            foreach (Node n in Nodes)
            {
                Previous.Add(n.Name, null);
                Basis.Add(n);
                Dist.Add(n.Name, double.MaxValue);
            }
        }

        /// <summary>
        /// Berechnet die kürzesten Wege vom start
        /// Knoten zu allen anderen Knoten
        /// </summary>
        /// <param name="start">Startknoten</param>
        public void calculateDistance(Node start)
        {
	        _startNode = start;
            Dist[start.Name] = 0;

            while (Basis.Count > 0)
            {
                Node u = getNodeWithSmallestDistance();
                if (u == null)
                {
                    Basis.Clear();
                }
                else
                {
                    foreach (Node v in getNeighbors(u))
                    {
                        double alt = Dist[u.Name] +
                                getDistanceBetween(u, v);
                        if (alt < Dist[v.Name])
                        {
                            Dist[v.Name] = alt;
                            Previous[v.Name] = u;
                        }
                    }
                    Basis.Remove(u);
                }
            }
        }

		/// <summary>
		/// Liefert den Pfad zum Knoten d
		/// </summary>
		/// <param name="d">Zielknote<n/param>
		/// <returns></returns>
		public List<Node> getPathTo(Node d)
		{
			List<Node> path = new List<Node>();

			path.Insert(0, d);

			while (Previous[d.Name] != null)
			{
				d = Previous[d.Name];
				path.Insert(0, d);
			}

			if (d.Name.Equals(_startNode.Name))
			{
				return path;
			}
			else
			{
				return new List<Node>();
			}
		}

        /// <summary>
        /// Liefert den Knoten mit der kürzesten Distanz
        /// </summary>
        /// <returns></returns>
        public Node getNodeWithSmallestDistance()
        {
            double distance = double.MaxValue;
            Node smallest = null;

            foreach (Node n in Basis)
            {
                if (Dist[n.Name] < distance)
                {
                    distance = Dist[n.Name];
                    smallest = n;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Liefert alle Nachbarn von n die noch in der Basis sind
        /// </summary>
        /// <param name="n">Knoten</param>
        /// <returns></returns>
        public List<Node> getNeighbors(Node n)
        {
            List<Node> neighbors = new List<Node>();

            foreach (Edge e in Edges)
            {
                if (e.Origin.Equals(n) && Basis.Contains(n))
                {
                    neighbors.Add(e.Destination);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Liefert die Distanz zwischen zwei Knoten
        /// </summary>
        /// <param name="o">Startknoten</param>
        /// <param name="d">Endknoten</param>
        /// <returns></returns>
        public double getDistanceBetween(Node o, Node d)
        {
            foreach (Edge e in Edges)
            {
                if (e.Origin.Equals(o) && e.Destination.Equals(d))
                {
                    return e.Distance;
                }
            }

            return 0;
        }

        /// <summary>
        /// Liste aller Knoten in der Basis
        /// </summary>
        public List<Node> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// Liste aller Kanten
        /// </summary>
        public List<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        /// <summary>
        /// Knotenmenge die noch betrachtet wird
        /// </summary>
        public List<Node> Basis
        {
            get { return _basis; }
            set { _basis = value; }
        }

        /// <summary>
        /// Distanzen der Kanten
        /// </summary>
        public Dictionary<string, double> Dist
        {
            get { return _dist; }
            set { _dist = value; }
        }

        /// <summary>
        /// Vorgänger Knoten
        /// </summary>
        public Dictionary<string, Node> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }
    }
}