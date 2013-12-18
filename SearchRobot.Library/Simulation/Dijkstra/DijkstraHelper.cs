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
	    private readonly MapExplored _mapExplored;

	    private readonly Dijkstra _dijkstra;

	    public DijkstraHelper(MapExplored mapExplored)
		{
			/* 
			 * 1) Simplify MapExplored
			 * 2) Create Edges / Vertices
			 * 3) Calculate Path
			 * 4) return path steps as waypoints
			 */

			_nodeMatrix = new Node[800 / GRID_SIZE, 600 / GRID_SIZE];
			_nodeList = new List<Node>();
			_edgeList = new List<Edge>();

			Simplify(mapExplored);
			CreateEdges();

			// create dijkstra instance
			_dijkstra = new Dijkstra(_edgeList, _nodeList);
		}

		public List<Point> GetPath(Point pos, Point target)
		{
			// set start node and calculate distances
			_dijkstra.calculateDistance(GetNodeFromPoint(pos));

			return _dijkstra.getPathTo(GetNodeFromPoint(target)).Select(n => GeneratePointFromName(n.Name)).ToList();
		}

		private Node GetNodeFromPoint(Point point)
		{
			return _nodeMatrix[point.X / GRID_SIZE, point.Y / GRID_SIZE];
		}

        private bool[,] Simplify(MapExplored mapExplored)
        {
            bool[,] mapSimplified = new bool[800 / GRID_SIZE, 600 / GRID_SIZE];

            for (int i = 0; i < 800; i += GRID_SIZE)
            {
                for (int j = 0; j < 600; j += GRID_SIZE)
                {
                    CreateNode(i, j, GRID_SIZE, mapExplored);
                }
            }

            return mapSimplified;
        }

        private void CreateNode(int startX, int startY, int range, MapExplored mapExplored)
        {
            for (int i = startX; i < startX + range; i++)
            {
                for (int j = startY; j < startY + range; j++)
                {
                    MapElementStatus status = mapExplored.GetStatus(i, j);

                    // don't create node if any pixel in range x range is blocked
                    if (status == MapElementStatus.Blocked)
                    {
                        return;
                    }
                }
            }

            // if node is free -> save
            Node node = new Node(GenerateName(startX / GRID_SIZE, startY / GRID_SIZE));
            _nodeMatrix[startX / GRID_SIZE, startY / GRID_SIZE] = node;
            _nodeList.Add(node);
        }

        private void CreateEdges()
        {
            // create all edges
            for (int x = 0; x < 800 / GRID_SIZE; x++)
            {
                for (int y = 0; y < 600 / GRID_SIZE; y++)
                {
                    connectNeighbours(x, y);
                }
            }
        }

        private void connectNeighbours(int x, int y)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int pointerX = x + i;
                    int pointerY = y + j;

                    // check bounds
                    if (pointerX >= 0 &&
                        pointerX < 800 / GRID_SIZE &&
                        pointerY >= 0 &&
                        pointerY < 600 / GRID_SIZE &&
                        (pointerX != x ||
                        pointerY != y))
                    {

                        bool isDiagonalEdge = Math.Abs(i) + Math.Abs(j) >= 2;

                        // check if nodes exist!
                        Node nodeOrigin = _nodeMatrix[x, y];
                        Node nodeTarget = _nodeMatrix[pointerX, pointerY];

                        if (nodeOrigin != null && nodeTarget != null)
                        {
                            // don't create edges node has neighbours and is diagonal
                            if(isDiagonalEdge && isDiagonalHasNeighbours(x, y))
                            {
                                continue;
                            }

                            _edgeList.Add(new Edge(nodeOrigin, nodeTarget, isDiagonalEdge ? Math.Sqrt(2) : 1));
                        }
                    }
                }
            }
        }

        private bool isDiagonalHasNeighbours(int x, int y)
        {
            // check if top / right / bottom / left is blocked. if true, don't create edge!
            if (x - 1 >= _nodeMatrix.GetLowerBound(0) && x + 1 < _nodeMatrix.GetUpperBound(0))
            {
                if (y - 1 > _nodeMatrix.GetLowerBound(1) && y + 1 < _nodeMatrix.GetUpperBound(1))
                {
                    if (_nodeMatrix[x - 1, y] == null || _nodeMatrix[x, y - 1] == null || _nodeMatrix[x, y + 1] == null || _nodeMatrix[x + 1, y] == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsPointValid(Point point)
        {
            return _nodeList.Any(x => x.Name.Equals(GenerateName((point.X - GRID_SIZE / 2) / GRID_SIZE, (point.Y - GRID_SIZE / 2) / GRID_SIZE)));
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
