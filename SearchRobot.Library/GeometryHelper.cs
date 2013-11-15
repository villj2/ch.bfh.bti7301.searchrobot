using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library
{
	public static class GeometryHelper
	{
		private const double EPSILON = 0.001;

		public static double GetAngle(Point centerPoint, Point edgePoint)
		{
			return Math.Atan2(edgePoint.Y - centerPoint.Y, edgePoint.X - centerPoint.X) / Math.PI * 180;
		}

        public static double GetAngle(double x1, double y1, double x2, double y2)
        {
            return Math.Atan2(y2 - y1, x2 - x1) / Math.PI * 180;
        }

		public static double GetWidth(Point centerPoint, Point edgePoint)
		{
			return Math.Sqrt(
					Math.Pow(Math.Abs(centerPoint.X - edgePoint.X), 2) +
					Math.Pow(Math.Abs(centerPoint.Y - edgePoint.Y), 2));
		}

		public static System.Windows.Point Convert(Point point)
		{
			return new System.Windows.Point(point.X, point.Y);
		}

        public static Point Convert(System.Windows.Point point)
        {
            return new Point { X = System.Convert.ToInt32(point.X), Y = System.Convert.ToInt32(point.Y) };
        }

        public static bool ComparePoints(Point point1, Point point2)
        {
            return (Math.Abs(point1.X - point2.X) < EPSILON && Math.Abs(point1.Y - point2.Y) < EPSILON);
        }

        public static double ToDegree(double radians)
        {
            return radians*180/Math.PI;
        }

        public static double ToRadians(double degree)
        {
            return degree/180*Math.PI;
        }

        public static bool ComparePoints(int x1, int y1, int x2, int y2)
        {
            return (x1 == x2 && y1 == y2);
        }

        public static bool ComparePointsWithRange(double x1, double y1, double x2, double y2, int range)
        {
            double x2Min = x2 - range / 2;
            double x2Max = x2 + range / 2;

            double y2Min = y2 - range / 2;
            double y2Max = y2 + range / 2;

            return ((x1 >= x2Min && x1 <= x2Max) && (y1 >= y2Min && y1 <= y2Max));
        }
	}
}
