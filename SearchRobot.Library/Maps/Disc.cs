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
	public class Disc : MapElement
	{
		public int Radius { get; set; }

		private Ellipse _uiElement;
		private Geometry _geometry;

		public Disc(Map map) : base(map)
		{ }

		public Disc()
		{ }

		protected override Geometry GeometryShape
		{
			get
			{
			    return _geometry ?? new EllipseGeometry(new System.Windows.Point(StartPosition.X, StartPosition.Y), Radius, Radius);
			}
		}

        public override UIElement UiElement
	    {
            get { return _uiElement;  }
	    }

	    public override void MouseDown(Canvas canvas, Point point)
		{
			StartPosition = point;
			ApplyTo(canvas);
		}

		private int GetRadius(Point start, Point end)
		{
			int x = Math.Abs(start.X - end.X);
			int y = Math.Abs(start.Y - end.Y);

			return Convert.ToInt32(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
		}

		public override void MouseMove(Canvas canvas, Point point)
		{
			Radius = GetRadius(StartPosition, point);
            _uiElement.Width = _uiElement.Height = 2 * Radius;

			Canvas.SetLeft(_uiElement, StartPosition.X - Radius);
			Canvas.SetTop(_uiElement, StartPosition.Y - Radius);

		    _geometry = null;
			_uiElement.Fill = IsValid() ? Brushes.Black : Brushes.Red;
		}

		public override void ApplyTo(Canvas canvas)
		{
			_uiElement = new Ellipse { Width = Radius*2, Height = Radius*2, Fill = Brushes.Black };

			Canvas.SetLeft(_uiElement, StartPosition.X - Radius);
			Canvas.SetTop(_uiElement, StartPosition.Y - Radius);

			canvas.Children.Add(_uiElement);
		}

		public override void Remove(Canvas canvas)
		{
			canvas.Children.Remove(_uiElement);
			Map.Remove(this);
		}
	}
}
