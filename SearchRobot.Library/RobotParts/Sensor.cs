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
	    private enum FieldState
	    {
	        Undiscovered,
            Blocked,
            Shadow,
            Visited,
            Outside
	    }

		private BitmapConverter Converter { get; set; }

        private FieldState[,] BaseArea { get; set; }
        
        private Map Map { get; set; }

		private Robot Robot { get; set; }

		private Sight Sight { get; set; }

		public Sensor(Robot robot, Map map, Canvas canvas, Sight sight)
		{
			Map = map;
			Robot = robot;
			Sight = sight;

			//Converter = new BitmapConverter(new Size(canvas.ActualWidth, canvas.ActualHeight));
		    BaseArea = GetBaseFieldMap(GetStructureBitmap(canvas));
		}

        private Bitmap GetStructureBitmap(Canvas canvas)
        {
            Robot.Remove(canvas);
            Bitmap result = Converter.ToBitmap(canvas);
            Robot.ApplyTo(canvas);

            return result;
        }

        private FieldState[,] GetBaseFieldMap(Bitmap bitmap)
        {
            FieldState[,] map = new FieldState[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).R < 100)
                    {
                        map[x,y] = FieldState.Blocked;
                    }
                }
            }

            return map;
        }

        private CartesianArray<FieldState> GetVisibleField()
        {
            PointRotator rotator = new PointRotator(Robot.StartPosition, Robot.Direction);

            return null;
        }

		public List<Point> GetView()
		{
			return null;
		}
	}
}
