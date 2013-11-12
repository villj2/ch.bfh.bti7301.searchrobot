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

        private static double AngleCos { get; set; }
        private static double AngleSin { get; set; }

        public PointRotator(Point center, double angleInDegree)
        {
            Center = center;
            CalculateAngles(angleInDegree);
        }

        public PointRotator(double angleInDegree)
        {
            Center = null;
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
                    X = Convert.ToInt32(Math.Round(offsetedPoint.X * AngleCos - offsetedPoint.Y * AngleSin)),
                    Y = Convert.ToInt32(Math.Round(offsetedPoint.X * AngleSin + offsetedPoint.Y * AngleCos))
                };
        }

        public NegativeArray<TDataType> Rotate<TDataType>(NegativeArray<TDataType> srcArray)
        {
            // Get Edges after rotation
            int smallestX, biggestX, smallestY, biggestY;

            Point[] corners = new[]
                                  {
                                      srcArray.TopLeftCoordinate, srcArray.TopRightCoordinate,
                                      srcArray.BottomLeftCoordinate, srcArray.BottomRightCoordinate
                                  };

            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = Rotate(corners[i]);
            }

            smallestX = biggestX = corners[0].X;
            smallestY = biggestY = corners[0].Y;

            for (int i = 1; i < corners.Length; i++)
            {
                smallestX = smallestX > corners[i].X ? corners[i].X : smallestX;
                biggestX = biggestX < corners[i].X ? corners[i].X : biggestX;

                smallestY = smallestY > corners[i].Y ? corners[i].Y : smallestY;
                biggestY = biggestY < corners[i].Y ? corners[i].Y : biggestY;
            }

            // create the resulting array with the correct sizes
            NegativeArray<TDataType> result = new NegativeArray<TDataType>(
                                                            biggestX - smallestX + 1,
                                                            biggestY - smallestY + 1,
                                                            smallestX,
                                                            smallestY);

            // rotate all not default values
            TDataType defaultValue = default(TDataType);
            for (int x = srcArray.XOffset; x < srcArray.XOffset + srcArray.Width; x++)
            {
                for (int y = srcArray.YOffset; y < srcArray.YOffset + srcArray.Height; y++)
                {
                    if (!srcArray[x, y].Equals(defaultValue))
                    {
                        Point p = Rotate(new Point(x, y));
                        result[p.X, p.Y] = srcArray[x, y];
                    }
                }
            }

            return result;
        }
    }
}
