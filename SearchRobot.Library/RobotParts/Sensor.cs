using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using SearchRobot.Library.Global;
using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SearchRobot.Library.Maps.Point;
using Size = System.Windows.Size;

namespace SearchRobot.Library.RobotParts
{
	public class Sensor
	{
        private struct Modifiers
        {
            private readonly int _x;
            private readonly int _y;

            public Modifiers(int x, int y)
            {
                _x = x;
                _y = y;
            }

            public int X { get { return _x; } }
            public int Y { get { return _y; } }
        }

		private Lazy<CartesianArray<MapElementStatus>> BaseArea { get; set; }

		private Robot Robot { get; set; }

		private Sight Sight { get; set; }

		private Canvas Canvas { get; set; }

        private static bool IsInbound(Point point, Area area)
        {
            return (point.X > area.LeftEdge && point.X < area.RightEdge)
                && (point.Y > area.BottomEdge && point.Y < area.TopEdge);
        }

        public static double GetYRatio(double direction)
        {
            return 1 / Math.Cos(GeometryHelper.ToRadians(direction));
        }

        private static Modifiers GetModifiersForDirection(double direction)
        {
            var fixedDirection = direction%360;

            if (fixedDirection >= 0 && fixedDirection <= 90)
            {
                return new Modifiers(1, 1);
            }
            else if (fixedDirection > 90 && fixedDirection <= 180)
            {
                return new Modifiers(-1, 1);
            }
            else if (fixedDirection > 180 && fixedDirection <= 270)
            {
                return new Modifiers(-1, -1);
            }
            else
            {
                return new Modifiers(1, -1);
            }
        }

		public Sensor(Robot robot, Canvas canvas, Sight sight)
		{
			Robot = robot;
			Sight = sight;
			Canvas = canvas;
			BaseArea = new Lazy<CartesianArray<MapElementStatus>>(
				() => CartesianArray<MapElementStatus>.FromArray(GetBaseFieldMap(GetStructureBitmap(Canvas))));
		}

        private Bitmap GetStructureBitmap(Canvas canvas)
        {
            Robot.Remove(canvas);
            Bitmap result = (new BitmapConverter(new Size(canvas.ActualWidth, canvas.ActualHeight))).ToBitmap(canvas);
            Robot.ApplyTo(canvas);

            return result;
        }

        private MapElementStatus[,] GetBaseFieldMap(Bitmap bitmap)
        {
            MapElementStatus[,] map = new MapElementStatus[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).R < 125)
                    {
                        map[x, y] = MapElementStatus.Blocked;
                    }
                    else if (bitmap.GetPixel(x, y).R <= 255 && bitmap.GetPixel(x, y).B <= 100)
                    {
                        map[x, y] = MapElementStatus.Target;
                    }
                }
            }

            return map;
        }

        public CartesianArray<MapElementStatus> GetRobotCenteredMapCopy(Robot robot, CartesianArray<MapElementStatus> src)
        {
            var copy = src.Clone();

            copy.XOffset = - robot.StartPosition.X;
            copy.YOffset = - (src.Height - robot.StartPosition.Y);

            return copy;
        }

        private void SetSensorAngleBoundaries(CartesianArray<MapElementStatus> map, Area area)
        {
            SpawnShadow(map, new Point(0, 0), area, Robot.CartasianDirection + (Sight.Angle / 2));
            SpawnShadow(map, new Point(0, 0), area, Robot.CartasianDirection - (Sight.Angle / 2));
        }

        private Point GetStartPoint(double direction)
        {
            var modifier = GetModifiersForDirection(direction);
            return new Point(2*modifier.X, (int)Math.Round(GetYRatio(direction) * 2 * modifier.Y));
        }

	    public CartesianArray<MapElementStatus> GetView()
        {
            var currentViewPort = GetRobotCenteredMapCopy(Robot, BaseArea.Value);

            Area area = new Area(
                topEdge: currentViewPort.TopRightCoordinate.Y - 1,
                rightEdge: currentViewPort.TopRightCoordinate.X - 1,
                bottomEdge: currentViewPort.BottomLeftCoordinate.Y,
                leftEdge: currentViewPort.BottomLeftCoordinate.X);

	        SetSensorAngleBoundaries(map: currentViewPort, area: area);

            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(GetStartPoint(Robot.CartasianDirection));

            while(pointQueue.Count > 0)
            {
                Point curPoint = pointQueue.Dequeue();
                Point topPoint = new Point(curPoint.X, curPoint.Y + 1);
                Point rightPoint = new Point(curPoint.X + 1, curPoint.Y);
                Point bottomPoint = new Point(curPoint.X, curPoint.Y - 1);
                Point leftPoint = new Point(curPoint.X - 1, curPoint.Y);

                if (IsInbound(bottomPoint, area))
                {
                    HandlePoint(pointQueue, currentViewPort, bottomPoint, area);
                }

                if (IsInbound(topPoint, area))
                {
                    HandlePoint(pointQueue, currentViewPort, topPoint, area);
                }

                if (IsInbound(rightPoint, area))
                {
                    HandlePoint(pointQueue, currentViewPort, rightPoint, area);
                }

                if (IsInbound(leftPoint, area))
                {
                    HandlePoint(pointQueue, currentViewPort, leftPoint, area);
                }
            }

            return currentViewPort;
		}


        private void HandlePoint(Queue<Point> queue, CartesianArray<MapElementStatus> viewport, Point point, Area area)
        {
            if (viewport[point] == MapElementStatus.Undiscovered)
            {
                queue.Enqueue(point);
                viewport[point] = MapElementStatus.Discovered;
            }

            if (viewport[point] == MapElementStatus.Blocked || viewport[point] == MapElementStatus.Target)
            {
                SpawnShadow(viewport, point, area, GetStatusToSpawnFor(viewport[point]));
            }
        }

        private MapElementStatus GetStatusToSpawnFor(MapElementStatus status)
        {
            return status == MapElementStatus.Blocked
                       ? MapElementStatus.BlockedShadowed
                       : MapElementStatus.TargetShadowed;
        }

		private void SpawnShadow(CartesianArray<MapElementStatus> viewport, Point point, Area area, double direction)
        {
            double ratio = GetYRatio(direction);
            bool increaseX = Math.Abs(ratio) < 1;

		    var modifier = GetModifiersForDirection(direction);
		    ratio = Math.Abs(increaseX ? ratio : 1/ratio);

            SpawnShadow(viewport, point, area, MapElementStatus.Shadowed, ratio, modifier, increaseX);
		}

        private void SpawnShadow(CartesianArray<MapElementStatus> viewport, Point point, Area area, MapElementStatus elementType)
        {         
			bool increaseX = Math.Abs(point.X) > Math.Abs(point.Y);
			double ratio = 0.0;

            var modifier = new Modifiers(point.X > 0 ? 1 : -1, point.Y > 0 ? 1 : -1);

			if (point.X != 0 && point.Y != 0)
			{
                ratio = Math.Abs(increaseX ? (double)point.Y / point.X : (double)point.X / point.Y);
			}

            SpawnShadow(viewport, point, area, elementType, ratio, modifier, increaseX);
        }

        private void SpawnShadow(CartesianArray<MapElementStatus> viewport, Point point, Area area, MapElementStatus elementType, double ratio, Modifiers modifier, bool increaseX)
        {
            int xdistance = 0;
            int ydistance = 0;

            bool first = true;

            var curentPoint = new Point(point.X + xdistance, point.Y + ydistance);
            do
            {
                viewport[curentPoint] = first ? elementType : MapElementStatus.Shadowed;
                first = false;

                if (increaseX)
                {
                    xdistance += modifier.X;
                    ydistance = Convert.ToInt32(Math.Floor(ratio * xdistance * modifier.X * modifier.Y));
                }
                else
                {
                    ydistance += modifier.Y;
                    xdistance = Convert.ToInt32(Math.Floor(ratio * ydistance * modifier.X * modifier.Y));
                }
                curentPoint = new Point(point.X + xdistance, point.Y + ydistance);

            } while (IsInbound(curentPoint, area));
        }
	}
}
