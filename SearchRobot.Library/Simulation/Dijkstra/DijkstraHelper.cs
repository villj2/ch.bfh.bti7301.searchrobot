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
        private static Node[,] _nodeMatrix;
        private static List<Node> _nodeList;

        private static List<Edge> _edgeList;

        public static List<Point> GetPath(Point pos, Point target, MapExplored mapExplored)
        {
            /* 
             * 1) Simplify MapExplored
             * 2) Create Edges / Vertices
             * 3) Calculate Path
             * 4) return path steps as waypoints
             */

            // initialize
            _nodeMatrix = new Node[40, 30];
            _nodeList = new List<Node>();
            _edgeList = new List<Edge>();

            // Simplify MapExplored (20x20 -> 1x1) and if no blocked Element, create node.
            Simplify(mapExplored);
            CreateEdges();

            // create dijkstra instance
            Dijkstra d = new Dijkstra(_edgeList, _nodeList);

            // set start node and calculate distances
            Node nodeStart = _nodeMatrix[pos.X / 20, pos.Y / 20];
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

        private static bool[,] Simplify(MapExplored mapExplored)
        {
            bool[,] mapSimplified = new bool[40, 30];

            for (int i = 0; i < 800; i += 20)
            {
                for (int j = 0; j < 600; j += 20)
                {
                    mapSimplified[i / 20, j / 20] = CreateNode(i, j, 20, mapExplored);
                }
            }

            return mapSimplified;
        }

        private static bool CreateNode(int startX, int startY, int range, MapExplored mapExplored)
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
            Node node = new Node(GenerateName(startX / 20, startY / 20));
            _nodeMatrix[startX / 20, startY / 20] = node;
            _nodeList.Add(node);

            return true;
        }

        private static void CreateEdges()
        {
            // FIXME just4testing add three edges (0000 -> 0001 -> 0002) and (0000 -> 00002)
            // vertical / horizontal
            //_edgeList.Add(new Edge(_nodeMatrix[0, 0], _nodeMatrix[0, 1], 2));
            //_edgeList.Add(new Edge(_nodeMatrix[0, 1], _nodeMatrix[1, 1], 2));
            //_edgeList.Add(new Edge(_nodeMatrix[1, 1], _nodeMatrix[1, 2], 2));
            //_edgeList.Add(new Edge(_nodeMatrix[1, 2], _nodeMatrix[2, 2], 2));
            //_edgeList.Add(new Edge(_nodeMatrix[2, 2], _nodeMatrix[2, 3], 2));
            //_edgeList.Add(new Edge(_nodeMatrix[2, 3], _nodeMatrix[3, 3], 2));

            //// diagonal
            //_edgeList.Add(new Edge(_nodeMatrix[0, 0], _nodeMatrix[1, 1], 1));
            //_edgeList.Add(new Edge(_nodeMatrix[1, 1], _nodeMatrix[2, 2], 1));
            //_edgeList.Add(new Edge(_nodeMatrix[2, 2], _nodeMatrix[3, 3], 1));

            // create all edges
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            int pointerX = i + k;
                            int pointerY = j + l;

                            // check bounds
                            if (pointerX >= 0 &&
                                pointerX < 40 &&
                                pointerY >= 0 &&
                                pointerY < 30 &&
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

        private static string GenerateName(int x, int y)
        {
            return x.ToString("00") + y.ToString("00");
        }

        private static Point GeneratePointFromName(string name)
        {
            Point p = new Point();
            p.X = Convert.ToInt32(name.Substring(0, 2)) * 20 + 10;
            p.Y = Convert.ToInt32(name.Substring(2, 2)) * 20 - 10;
            p.Status = MapElementStatus.Waypoint;
            return p;
        }
    }
}
