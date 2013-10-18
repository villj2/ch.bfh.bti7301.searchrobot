using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SearchRobot.Library.Maps
{
    public abstract class MapElement
    {
        protected Map Map { get; private set; }

		protected abstract Geometry GeometryShape { get; }

        protected MapElement(Map map)
        {
            Map = map;
            StartPosition = new Point();
        }

	    public bool IsOverlappingWith(Geometry geometry)
	    {
			var intersectionDetail = GeometryShape.FillContainsWithDetail(geometry);
			return intersectionDetail != IntersectionDetail.Empty;
	    }

		public bool IsOverlapping()
		{
			if (GeometryShape == null)
			{
				return false;
			}

			var currrentGeometry = GeometryShape;
			var result =  Map.Elements.Any(e => e != this && e.IsOverlappingWith(currrentGeometry));

			if (result)
			{
				
			}

			return result;
		}

		public Point StartPosition { get; set; }

		public abstract void MouseDown(Canvas canvas, Point point);

		public abstract void MouseUp(Canvas canvas, Point point);

		public abstract void MouseMove(Canvas canvas, Point point);

		public abstract void ApplyTo(Canvas canvas);

		public abstract void Remove(Canvas canvas);
    }
}
