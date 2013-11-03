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
		private BitmapConverter Converter { get; set; }
	
		private Map Map { get; set; }
		private Robot Robot { get; set; }
		private Sight Sight { get; set; }

		public Sensor(Robot robot, Map map, Canvas canvas, Sight sight)
		{
			Map = map;
			Robot = robot;
			Sight = sight;

			Converter = new BitmapConverter(new Size(canvas.ActualWidth, canvas.ActualHeight));
		}

		public List<Point> GetView()
		{
			return null;
		}
	}
}
