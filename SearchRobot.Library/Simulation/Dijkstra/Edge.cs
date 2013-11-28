using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.Dijkstra
{
    class Edge
    {
        private Node _origin;
        private Node _destination;
        private double _distance;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="origin">Startknoten</param>
        /// <param name="destination">Zielknoten</param>
        /// <param name="distance">Distanz</param>
        public Edge(Node origin, Node destination, double distance)
        {
            this._origin = origin;
            this._destination = destination;
            this._distance = distance;
        }

        /// <summary>
        /// Startknoten
        /// </summary>
        public Node Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Zielknoten
        /// </summary>
        public Node Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// Distanz
        /// </summary>
        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
}