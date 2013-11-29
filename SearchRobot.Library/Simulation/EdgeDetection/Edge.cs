using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library.Simulation.EdgeDetection
{
	public class Edge
	{
		private readonly List<Point> _points;

		public IReadOnlyCollection<Point> Points
		{
			get { return _points.AsReadOnly(); }
		}

		public Edge(Point point)
		{
			_points = new List<Point>() { point };
		}

		public void AddPoint(Point point)
		{
			if (IsPointTouching(point))
			{
				AddPointUnsafe(point);
			}
			else
			{
				throw new ArgumentException("Point is part of the Edge!", "point");
			}
		}

		public void AddPointUnsafe(Point point)
		{
			_points.Add(point);
		}

		private bool ArePointTouching(Point pointA, Point pointB)
		{
			return Math.Abs(pointA.X - pointB.X) <= 1 && Math.Abs(pointA.Y - pointB.Y) <= 1;
		}

		public bool IsPointTouching(Point point)
		{
			return _points.Any(edgePoint => ArePointTouching(edgePoint, point));
		}
	}
}
