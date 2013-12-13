using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SearchRobot.Library.Global;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Simulation.EdgeDetection
{
	public class EdgeDetectionAlgorithm
	{
		Stopwatch creatingTime = new Stopwatch();
		Stopwatch addingTime = new Stopwatch();
		Stopwatch findingTime = new Stopwatch();

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
				creatingTime.Start();
				var point = points.First();
				points.Remove(point);

				var edge = new Edge(point);
				creatingTime.Stop();

				edges.Add(GrowEdge(edge, points));
			}

			return edges;
		}

		private IEnumerable<Point> GetNewEdgeEdgePoints(IEnumerable<Point> oldEdgePoints, IEnumerable<Point> src)
		{
			return oldEdgePoints.SelectMany(p => src.Where(point => Edge.ArePointTouching(point, p))).Distinct();
		}

		private Edge GrowEdge(Edge edge, List<Point> points)
		{
			List<Point> edgeEdgePoints = edge.Points.ToList();
			bool grew;

			do
			{
				grew = false;
				findingTime.Start();

				edgeEdgePoints = GetNewEdgeEdgePoints(edgeEdgePoints, points).ToList();
				findingTime.Stop();

				addingTime.Start();
				if (edgeEdgePoints.Any())
				{
					grew = true;
					edge.AddPointsUnsafe(edgeEdgePoints);
					edgeEdgePoints.ForEach(p => points.Remove(p));
				}
				addingTime.Stop();

			} while (grew);

			edge.Init();

			return edge;
		}

		public void DebugOutput()
		{
			Console.WriteLine("Creating: {0}", creatingTime.ElapsedMilliseconds);
			Console.WriteLine("Finding: {0}", findingTime.ElapsedMilliseconds);
			Console.WriteLine("Adding: {0}", addingTime.ElapsedMilliseconds);
		}
	}
}
