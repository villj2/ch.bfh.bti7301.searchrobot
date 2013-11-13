using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Maps
{
	public class Point
	{
        public int X { get; set; }
        public int Y { get; set; }
        
                public MapElementStatus Status { get; set; }
		
		public Point Clone()
		{
			return new Point {X = X, Y = Y};
		}

        public Point() {}

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Point;
            return p != null && p.X == X && p.Y == Y; 
        }
	}
}
