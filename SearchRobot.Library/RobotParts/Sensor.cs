using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
		private Lazy<CartesianArray<MapElementStatus>> BaseArea { get; set; }

		private Robot Robot { get; set; }

		private Sight Sight { get; set; }

		private Canvas Canvas { get; set; }

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
                    if (bitmap.GetPixel(x, y).R < 25)
                    {
                        map[x, y] = MapElementStatus.Blocked;
                    }
                    else if (bitmap.GetPixel(x, y).R < 200)
                    {
                        map[x, y] = MapElementStatus.Target;
                    }
                }
            }

            return map;
        }

        private CartesianArray<MapElementStatus> GetRotatedMapCopy(double angle)
        {
            return (new PointRotator(angle)).Rotate(GetRobotCenteredMapCopy(Robot, BaseArea.Value));
        }

        public CartesianArray<MapElementStatus> GetRobotCenteredMapCopy(Robot robot, CartesianArray<MapElementStatus> src)
        {
            var copy = src.Clone();

            copy.XOffset = - robot.StartPosition.X;
            copy.YOffset = - (src.Height - robot.StartPosition.Y);

            return copy;
        }

	    public CartesianArray<MapElementStatus> GetView()
        {
            Console.WriteLine(Robot.CartasianDirection);

            var currentViewPort = GetRotatedMapCopy(-Robot.CartasianDirection);

            int bottomEdge = currentViewPort.BottomRightCoordinate.Y;
			int topEdge = currentViewPort.TopRightCoordinate.Y;
	        int rightEdge = currentViewPort.TopRightCoordinate.X;

            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(new Point(0, 0));

            while(pointQueue.Count > 0)
            {
                Point curPoint = pointQueue.Dequeue();
                Point topPoint = new Point(curPoint.X, curPoint.Y + 1);
                Point rightPoint = new Point(curPoint.X + 1, curPoint.Y);
                Point bottomPoint = new Point(curPoint.X, curPoint.Y - 1);

                if (bottomPoint.Y > bottomEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, bottomPoint, topEdge, rightEdge, bottomEdge);
                }

                if (topPoint.Y < topEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, topPoint, topEdge, rightEdge, bottomEdge);
                }

                if (rightPoint.X < rightEdge)
                {
                    HandlePoint(pointQueue, currentViewPort, rightPoint, topEdge, rightEdge, bottomEdge);
                }
            }

            return currentViewPort;
		}


        private void HandlePoint(Queue<Point> queue, CartesianArray<MapElementStatus> viewport, Point point, int topEdge, int rightEdge, int bottomEdge)
        {
            if (viewport[point] == MapElementStatus.Undiscovered)
            {
                queue.Enqueue(point);
                viewport[point] = MapElementStatus.Discovered;
            }

            if (viewport[point] == MapElementStatus.Blocked || viewport[point] == MapElementStatus.Target)
            {
                SpawnShadow(viewport, point, topEdge, rightEdge, bottomEdge, viewport[point]);
            }
        }

        private void SpawnShadow(CartesianArray<MapElementStatus> viewport, Point point, int topEdge, int rightEdge, int bottomEdge, MapElementStatus elementType)
        {
            /**
             *            . .
             *        . .   .
             *    . .       .
             *  . . . . . . .
             *  
             *  x : 6 ,  y : 3
             * 
             *  ratio -> 3 / 6 -> 0.5
             *  
             *  y : 1 -> y * ratio -> x : 0.5
             *  y : 2 -> y * ratio -> x : 1
             *  
             * */
            
			bool increaseX = point.X > Math.Abs(point.Y);
			double ratio = 0.0;

            int xModifier = point.X > 0 ? 1 : -1;
            int yModifier = point.Y > 0 ? 1 : -1;

			if (point.X != 0 && point.Y != 0)
			{
                ratio = Math.Abs(increaseX ? (double)point.Y / point.X : (double)point.X / point.Y);
			}

			int xdistance = 0;
			int ydistance = 0;

			bool first = true;

            do
            {
                viewport[point.X + xdistance, point.Y + ydistance] = first ? elementType : MapElementStatus.Shadowed;
                first = false;

                if (increaseX)
                {
                    xdistance += xModifier;
                    ydistance = Convert.ToInt32(Math.Round(ratio * xdistance * xModifier * yModifier));
                }
                else
                {
                    ydistance += yModifier;
                    xdistance = Convert.ToInt32(Math.Round(ratio * ydistance * xModifier * yModifier));
                }

            } while (point.X + xdistance < rightEdge
				  && point.Y + ydistance > bottomEdge 
				  && point.Y + ydistance < topEdge);
        }
	}
}
