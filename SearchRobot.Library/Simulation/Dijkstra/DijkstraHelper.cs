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
        // FIXME trotzdem mit string-namen arbeiten? Weil nötig bei Dijkstra d = new Dijkstra(_edges, _nodes);
        //private static Dictionary<string, Node> _dictNodes = new Dictionary<string, Node>();
        private static Node[,] _nodeMatrix = new Node[40, 30];
        private static List<Node> _nodeList = new List<Node>();

        private static List<Edge> _edgeList = new List<Edge>();

        public static List<Point> GetPath(Point pos, Point target, MapExplored mapExplored)
        {
            /* 
             * 1) Simplify MapExplored
             * 2) Create Edges / Vertices
             * 3) Calculate Path
             * 4) return path steps as waypoints
             */

            // Simplify MapExplored (20x20 -> 1x1) and if no blocked Element, create node.
            //bool[,] mapSimplified = Simplify(mapExplored);
            Simplify(mapExplored);
            CreateEdges();

            // create dijkstra instance
            Dijkstra d = new Dijkstra(_edgeList, _nodeList);

            // set start node and calculate distances
            //d.calculateDistance(_nodeMatrix[0,0]);
            Node nodeStart = _nodeMatrix[pos.X / 20, pos.Y / 20];
            d.calculateDistance(nodeStart);

            // get path to node
            // FIXME just4testing endpoint static
            List<Node> path = d.getPathTo(_nodeMatrix[5,15]);

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
                    mapSimplified[i / 20, j / 20] = IsFree(i, j, 20, mapExplored);
                }
            }

            return mapSimplified;
        }

        private static bool IsFree(int startX, int startY, int range, MapExplored mapExplored)
        {
            bool isFree = true;

            for (int i = startX; i < startX + range; i++)
            {
                if (!isFree) break;

                for (int j = startY; j < startY + range; j++)
                {
                    //if (mapExplored.GetStatus(i, j) != MapElementStatus.Undiscovered && mapExplored.GetStatus(i, j) != MapElementStatus.Discovered)
                    if (mapExplored.GetStatus(i, j) == MapElementStatus.Blocked ||
                        mapExplored.GetStatus(i, j) == MapElementStatus.BlockedShadowed ||
                        mapExplored.GetStatus(i, j) == MapElementStatus.Shadowed)
                    {
                        isFree = false;
                        break;
                    }
                }
            }

            // if node is free -> save
            if (isFree)
            {
                Node node = new Node(GenerateName(startX / 20, startY / 20));
                _nodeMatrix[startX / 20, startY / 20] = node;
                _nodeList.Add(node);
            }

            return isFree;
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

                                // check if nodes exist!
                                Node nodeOrigin = _nodeMatrix[i, j];
                                Node nodeTarget = _nodeMatrix[i + k, j + l];

                                if (nodeOrigin != null && nodeTarget != null)
                                {
                                    _edgeList.Add(new Edge(nodeOrigin, nodeTarget, 1));
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
            p.X = Convert.ToInt32(name.Substring(0, 2)) * 20;
            p.Y = Convert.ToInt32(name.Substring(2, 2)) * 20;
            p.Status = MapElementStatus.Waypoint;
            return p;
        }
    }
}
