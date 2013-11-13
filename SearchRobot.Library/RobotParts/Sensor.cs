using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private CartesianArray<MapElementStatus> BaseArea { get; set; }
        
		private Robot Robot { get; set; }

		private Sight Sight { get; set; }

		public Sensor(Robot robot, Canvas canvas, Sight sight)
		{
			Robot = robot;
			Sight = sight;
            BaseArea = CartesianArray<MapElementStatus>.FromArray(GetBaseFieldMap(GetStructureBitmap(canvas)));
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
                    if (bitmap.GetPixel(x, y).R < 100)
                    {
                        map[x, y] = MapElementStatus.Blocked;
                    }
                }
            }

            return map;
        }

        private CartesianArray<MapElementStatus> GetRotatedMapCopy(double angle)
        {
            return (new PointRotator(angle)).Rotate(GetRobotCenteredMapCopy(Robot, BaseArea));
        }

        public CartesianArray<MapElementStatus> GetRobotCenteredMapCopy(Robot robot, CartesianArray<MapElementStatus> src)
        {
            var copy = src.Clone();

            copy.XOffset = -robot.StartPosition.X;
            copy.YOffset = copy.Height - robot.StartPosition.Y;

            return copy;
        }

	    public CartesianArray<MapElementStatus> GetView()
        {
            var currentViewPort = GetRotatedMapCopy(Robot.Direction);

            int leftEdge = currentViewPort.XOffset;
	        int topEdge = currentViewPort.YOffset + currentViewPort.Height;
	        int rightEdge = currentViewPort.XOffset + currentViewPort.Width;

            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(new Point(0, 0));

            while(pointQueue.Count > 0)
            {
                Point curPoint = pointQueue.Dequeue();
                Point leftPoint = new Point(curPoint.X - 1, curPoint.Y);
                Point topPoint = new Point(curPoint.X, curPoint.Y + 1);
                Point rightPoint = new Point(curPoint.X + 1, curPoint.Y);

                if (leftPoint.X >= leftEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, leftPoint, leftEdge, topEdge, rightEdge);
                }

                if (topPoint.Y < topEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, topPoint, leftEdge, topEdge, rightEdge);
                }

                if (rightPoint.X < rightEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, rightPoint, leftEdge, topEdge, rightEdge);
                }
            }

            return currentViewPort;
		}


        private void HandlePoint(Queue<Point> queue, CartesianArray<MapElementStatus> viewport, Point point, int leftEdge, int topEdge, int rightEdge)
        {
            if (viewport[point] == MapElementStatus.Undiscovered)
            {
                queue.Enqueue(point);
                viewport[point] = MapElementStatus.Discovered;
            }
            if (viewport[point] == MapElementStatus.Blocked)
            {
                SpawnShadow(viewport, point, leftEdge, topEdge, rightEdge);
            }
        }

        private void SpawnShadow(CartesianArray<MapElementStatus> viewport, Point point, int leftEdge, int topEdge, int rightEdge)
        {
            bool increaseX = Math.Abs(point.X) < point.Y;
            double ratio = increaseX ? point.Y / point.X : point.X / point.Y;
            int xdistance = 0;
            int ydistance = 0;

            bool first = true;

            do
            {
                viewport[point.X + xdistance, point.Y + ydistance] = first ? MapElementStatus.BlockedShadowed : MapElementStatus.Shadowed;
                first = false;

                if (increaseX)
                {
                    xdistance += 1;
                    ydistance = Convert.ToInt32(Math.Round(ratio * xdistance));
                }
                else
                {
                    ydistance += 1;
                    xdistance = Convert.ToInt32(Math.Round(ratio * ydistance));
                }

            } while (point.X >= leftEdge && point.X < rightEdge && point.Y < topEdge);
        }
	}
}
