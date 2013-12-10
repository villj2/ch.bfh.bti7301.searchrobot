using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Global
{
	public class Area
	{
		public int TopEdge { get; set; }

		public int RightEdge { get; set; }

		public int BottomEdge { get; set; }

		public int LeftEdge { get; set; }

		public Area(int topEdge, int rightEdge, int bottomEdge, int leftEdge)
		{
			TopEdge = topEdge;
			RightEdge = rightEdge;
			BottomEdge = bottomEdge;
			LeftEdge = leftEdge;
		}
	}
}
