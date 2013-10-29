﻿using System;
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
			return new Point() {X = this.X, Y = this.Y};
		}
	}
}
