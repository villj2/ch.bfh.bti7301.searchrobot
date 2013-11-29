using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.Dijkstra
{
    public class Node
    {
        private string _name;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="name">Name des Knotens</param>
        public Node(string name)
        {
            this._name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
    }
}
