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
		public Point StartPoint { get; private set; }
		
		public Point EndPoint { get; private set; }

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

		public Point CenterPoint
		{
			get
			{
				return GetClosestPoint(FindCenter());
			}
		}

		public double Width
		{
			get { return GeometryHelper.GetDistance(StartPoint, EndPoint); }
		}

		private Point GetClosestPoint(Point closestTo)
		{
			Point result = null;
			double distance = double.MaxValue;

			_points.ForEach(p =>
				{
					double newDistance = GeometryHelper.GetDistance(p, closestTo);
					if (result == null || newDistance < distance)
					{
						result = p;
						distance = newDistance;
					}
				});

			return result;
		}

		private Point FindCenter()
		{
			int smallestX, smallestY, biggestY, biggestX;

			smallestX = biggestX = Points.First().X;
			smallestY = biggestY = Points.First().Y;

			foreach (var point in Points)
			{
				smallestX = smallestX > point.X ? point.X : smallestX;
				smallestY = smallestY > point.Y ? point.Y : smallestY;
				biggestX = biggestX < point.X ? point.X : biggestX;
				biggestY = biggestY > point.Y ? point.Y : biggestY;
			}

			return new Point(smallestX + (biggestX - smallestX)/2, smallestY + (biggestY - smallestY)/2);
		}
	}
}
