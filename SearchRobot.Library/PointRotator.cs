using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library
{
    public class PointRotator
    {
        private static Point Center { get; set; }
        private static double Angle { get; set; }

        private static double AngleCos { get; set; }
        private static double AngleSin { get; set; }

        public PointRotator(Point center, double angleInDegree)
        {
            Center = center;
            CalculateAngles(angleInDegree);
        }

        public PointRotator(double angleInDegree)
        {
            CalculateAngles(angleInDegree);
        }

        private void CalculateAngles(double angleInDegree)
        {
            var angleInRadians = GeometryHelper.ToRadians(angleInDegree);
            AngleCos = Math.Cos(angleInRadians);
            AngleSin = Math.Sin(angleInRadians);
        }

        public Point Rotate(Point point)
        {
            Point offsetedPoint = Center == null ? point : new Point() {X = point.X - Center.X, Y = point.Y - Center.Y};

            return new Point
                {
                    X = Convert.ToInt32(offsetedPoint.X * AngleCos - offsetedPoint.Y * AngleSin),
                    Y = Convert.ToInt32(offsetedPoint.Y * AngleSin + offsetedPoint.Y * AngleCos)
                };
        }
    }
}
