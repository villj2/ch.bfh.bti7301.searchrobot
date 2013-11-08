using SearchRobot.Library.RobotParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SearchRobot.Library.Maps
{
	[XmlInclude(typeof(Disc))]
	[XmlInclude(typeof(Goal))]
	[XmlInclude(typeof(Robot))]
	[XmlInclude(typeof(Wall))]
	public abstract class MapElement : ICanvasListener, ICloneable
	{
		protected Map Map { get; private set; }

		protected abstract Geometry GeometryShape { get; }

		public abstract UIElement UiElement { get; }

		public Point StartPosition { get; set; }

		protected MapElement(Map map)
		{
			Bind(map);
			StartPosition = new Point();
		}

		protected MapElement() { }

		public void Bind(Map map)
		{
			if (Map == null)
			{
				Map = map;
			}
			else
			{
				throw new InvalidOperationException("Map Element already bound to Map.");
			}
		}

		public bool IsOverlappingWith(Geometry geometry)
		{
			var intersectionDetail = GeometryShape.FillContainsWithDetail(geometry);
			return intersectionDetail != IntersectionDetail.Empty;
		}

		public bool IsOverlapping()
		{
			if (GeometryShape == null || Map == null)
			{
				return false;
			}

			var currrentGeometry = GeometryShape;

            var result = Map.Elements.Any(e => e != this && e.IsOverlappingWith(currrentGeometry));

			return result;
		}

		public abstract void MouseDown(Canvas canvas, Point point);

		public virtual void MouseUp(Canvas canvas, Point point)
        {
            Map.Add(this);

            if (!IsValid())
            {
                Map.Remove(this);
                canvas.Children.Remove(UiElement);
            }
		}

		public abstract void MouseMove(Canvas canvas, Point point);

        public virtual void MouseLeave(Canvas canvas)
	    {
            Remove(canvas);
	    }

	    public abstract void ApplyTo(Canvas canvas);

		public abstract void Remove(Canvas canvas);

		public virtual bool IsValid()
		{
			return !IsOverlapping();
		}

        public abstract void Move(Canvas canvas, int offsetX, int offsetY);

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public abstract object Clone();
	}
}
