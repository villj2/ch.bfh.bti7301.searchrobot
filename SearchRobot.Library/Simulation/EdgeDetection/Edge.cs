﻿using System;
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

		public Point CenterPoint { get; private set; }

		public double Width { get; private set; }

		private readonly List<Point> _points;

		public IReadOnlyCollection<Point> Points
		{
			get { return _points.AsReadOnly(); }
		}

		public Edge(Point point)
		{
			_points = new List<Point>() {point};
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

		public void AddPointsUnsafe(IEnumerable<Point> points)
		{
			_points.AddRange(points);
		}

		public static bool ArePointTouching(Point pointA, Point pointB)
		{
			return Math.Abs(pointA.X - pointB.X) <= 1 && Math.Abs(pointA.Y - pointB.Y) <= 1;
		}

		public bool IsPointTouching(Point point)
		{
			return _points.Any(edgePoint => ArePointTouching(edgePoint, point));
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

		public void Init()
		{
			int smallestX, smallestY, biggestY, biggestX;

			smallestX = biggestX = Points.First().X;
			smallestY = biggestY = Points.First().Y;

			foreach (var point in Points)
			{
				smallestX = smallestX > point.X ? point.X : smallestX;
				smallestY = smallestY > point.Y ? point.Y : smallestY;

				biggestX = biggestX < point.X ? point.X : biggestX;
				biggestY = biggestY < point.Y ? point.Y : biggestY;
			}

			StartPoint = new Point(smallestX, smallestY);
			EndPoint = new Point(biggestX, biggestY);
			CenterPoint = GetClosestPoint(new Point(smallestX + (biggestX - smallestX)/2, smallestY + (biggestY - smallestY)/2));
			Width = GeometryHelper.GetWidth(StartPoint, EndPoint);
		}
	}
}
