using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Global;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Simulation.EdgeDetection
{
	public class EdgeDetectionAlgorithm
	{
		private bool IsFieldBorderSafe(int x, int y, MapElementStatus[,] map, MapElementStatus status)
		{
			if (x < 0 || y < 0 || x > map.GetUpperBound(Constants.XDimension) || y > map.GetUpperBound(Constants.YDimension))
			{
				return false;
			}

			return map[x, y] == status;
		}

		private bool IsEdgeField(int x, int y, MapElementStatus[,] map)
		{
			const MapElementStatus status = MapElementStatus.Undiscovered;

			return IsFieldBorderSafe(x - 1, y, map, status)
					|| IsFieldBorderSafe(x, y - 1, map, status)
					|| IsFieldBorderSafe(x + 1, y, map, status)
					|| IsFieldBorderSafe(x, y + 1, map, status);
		}

		public List<Point> GetEdgePoints(MapElementStatus[,] map)
		{
			List<Point> edgePoints = new List<Point>();

			for (var x = 0; x <= map.GetUpperBound(Constants.XDimension); x++)
			{
				for (var y = 0; y <= map.GetUpperBound(Constants.YDimension); y++)
				{
					if (map[x, y] == MapElementStatus.Discovered && IsEdgeField(x, y, map))
					{
						edgePoints.Add(new Point(x, y));
					}
				}
			}

			return edgePoints;
		}

		public IEnumerable<Edge> GroupToEdges(List<Point> points)
		{
			List<Edge> edges = new List<Edge>();

			while (points.Any())
			{
				var point = points.First();
				points.Remove(point);

				var edge = new Edge(point);
				edges.Add(GrowEdge(edge, points));
			}

			return edges;
		}

		private Edge GrowEdge(Edge edge, List<Point> points)
		{
			bool grew;

			do
			{
				grew = false;

				var newPoints = points.Where(edge.IsPointTouching).ToList();

				if (newPoints.Any())
				{
					grew = true;

					newPoints.ForEach(edge.AddPointUnsafe);
					newPoints.ForEach(p => points.Remove(p));
				}

			} while (grew);

			return edge;
		}
	}
}
