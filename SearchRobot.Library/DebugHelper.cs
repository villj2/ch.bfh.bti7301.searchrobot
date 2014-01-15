using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchRobot.Library.Maps;

namespace SearchRobot.Library
{
	public static class DebugHelper
	{
		private readonly static Dictionary<MapElementStatus, Color> ColorCode =
			new Dictionary<MapElementStatus, Color>()
				{
					{ MapElementStatus.Undiscovered, Color.White },
					{ MapElementStatus.Discovered, Color.GreenYellow },
					{ MapElementStatus.BlockedShadowed, Color.DarkSlateGray },
					{ MapElementStatus.TargetShadowed, Color.Red },
					{ MapElementStatus.Shadowed, Color.DarkGray },
					{ MapElementStatus.Target, Color.Green },
					{ MapElementStatus.Blocked, Color.Black },
					{ MapElementStatus.Visited, Color.Pink },
					{ MapElementStatus.Waypoint, Color.Fuchsia },
					{ MapElementStatus.WaypointVisited, Color.Blue },
					{ MapElementStatus.Remove, Color.BlanchedAlmond },
					{ MapElementStatus.Collided, Color.Gold }
				};

		public static void StoreAsBitmap(string path, CartesianArray<MapElementStatus> data)
		{
			StoreAsBitmap(path, data.ToArray());
		}

		public static void StoreAsBitmap(string path, MapElementStatus[,] data)
		{
			var bitmap = new Bitmap(data.GetUpperBound(0), data.GetUpperBound(1));

			for (int x = 0; x < data.GetUpperBound(0); x++)
			{
				for (int y = 0; y < data.GetUpperBound(1); y++)
				{
					bitmap.SetPixel(x, y, ColorCode[data[x, y]]);
				}
			}

			// bitmap.Save(path);
		}
	}
}
