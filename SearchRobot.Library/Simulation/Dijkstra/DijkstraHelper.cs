using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.Dijkstra
{
    public class DijkstraHelper
    {
        public const int GRID_SIZE = 20;

        private Node[,] _nodeMatrix;
        private List<Node> _nodeList;

        private List<Edge> _edgeList;

        public List<Point> GetPath(Point pos, Point target, MapExplored mapExplored)
        {
            /* 
             * 1) Simplify MapExplored
             * 2) Create Edges / Vertices
             * 3) Calculate Path
             * 4) return path steps as waypoints
             */

            // initialize
            _nodeMatrix = new Node[800 / GRID_SIZE, 600 / GRID_SIZE];
            _nodeList = new List<Node>();
            _edgeList = new List<Edge>();

            // Simplify MapExplored (20x20 -> 1x1) and if no blocked Element, create node.
            Simplify(mapExplored);
            CreateEdges();

            // create dijkstra instance
            Dijkstra d = new Dijkstra(_edgeList, _nodeList);

            // set start node and calculate distances
            Node nodeStart = _nodeMatrix[pos.X / GRID_SIZE, pos.Y / GRID_SIZE];
            //Node nodeStart = _nodeList.First();
            d.calculateDistance(nodeStart);

            // get path to node
            // FIXME just4testing endpoint static
            List<Node> path = d.getPathTo(_nodeList.Last());

            List<Point> waypoints = new List<Point>();
            foreach (Node n in path)
            {
                waypoints.Add(GeneratePointFromName(n.Name));
            }

            return waypoints;
        }

        private bool[,] Simplify(MapExplored mapExplored)
        {
            bool[,] mapSimplified = new bool[800 / GRID_SIZE, 600 / GRID_SIZE];

            for (int i = 0; i < 800; i += GRID_SIZE)
            {
                for (int j = 0; j < 600; j += GRID_SIZE)
                {
                    mapSimplified[i / GRID_SIZE, j / GRID_SIZE] = CreateNode(i, j, GRID_SIZE, mapExplored);
                }
            }

            return mapSimplified;
        }

        private bool CreateNode(int startX, int startY, int range, MapExplored mapExplored)
        {
            for (int i = startX; i < startX + range; i++)
            {
                for (int j = startY; j < startY + range; j++)
                {
                    MapElementStatus status = mapExplored.GetStatus(i, j);

                    if (status == MapElementStatus.Blocked)
                    {
                        return false;
                    }
                }
            }

            // if node is free -> save
            Node node = new Node(GenerateName(startX / GRID_SIZE, startY / GRID_SIZE));
            _nodeMatrix[startX / GRID_SIZE, startY / GRID_SIZE] = node;
            _nodeList.Add(node);

            return true;
        }

        private void CreateEdges()
        {
            // create all edges
            for (int i = 0; i < 800 / GRID_SIZE; i++)
            {
                for (int j = 0; j < 600 / GRID_SIZE; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            int pointerX = i + k;
                            int pointerY = j + l;

                            // check bounds
                            if (pointerX >= 0 &&
                                pointerX < 800 / GRID_SIZE &&
                                pointerY >= 0 &&
                                pointerY < 600 / GRID_SIZE &&
                                (pointerX != i ||
                                pointerY != j))
                            {

                                bool isDiagonalEdge = Math.Abs(k) + Math.Abs(l) >= 2;

                                // check if nodes exist!
                                Node nodeOrigin = _nodeMatrix[i, j];
                                Node nodeTarget = _nodeMatrix[pointerX, pointerY];

                                if (nodeOrigin != null && nodeTarget != null)
                                {
                                    if (isDiagonalEdge)
                                    {
                                        // check if top / right / bottom / left is blocked. if true, don't create edge!
                                        if (i - 1 >= _nodeMatrix.GetLowerBound(0) && i + 1 < _nodeMatrix.GetUpperBound(0))
                                        {
                                            if (j - 1 > _nodeMatrix.GetLowerBound(1) && j + 1 < _nodeMatrix.GetUpperBound(1))
                                            {
                                                if (_nodeMatrix[i - 1, j] == null || _nodeMatrix[i, j - 1] == null || _nodeMatrix[i, j + 1] == null || _nodeMatrix[i + 1, j] == null)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }

                                    _edgeList.Add(new Edge(nodeOrigin, nodeTarget, isDiagonalEdge ? Math.Sqrt(2) : 1));
                                }
                            }
                        }
                    }
                }
            }

        }

        private string GenerateName(int x, int y)
        {
            return x.ToString("00") + y.ToString("00");
        }

        private Point GeneratePointFromName(string name)
        {
            Point p = new Point();
            p.X = Convert.ToInt32(name.Substring(0, 2)) * GRID_SIZE + GRID_SIZE / 2;
            p.Y = Convert.ToInt32(name.Substring(2, 2)) * GRID_SIZE + GRID_SIZE / 2;
            p.Status = MapElementStatus.Waypoint;
            return p;
        }
    }
}
