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
    public class Wall : MapElement
    {
        private Rectangle _uiElement;
	    private Geometry _geometry;

	    private const int Height = 4;

		public Point EndPoint { get; set; }

		protected override Geometry GeometryShape
		{
			get
			{
				return _geometry ?? (_geometry = AsRectangleGeometry(StartPosition, EndPoint));
			}
		}

        public override UIElement UiElement
        {
            get { return _uiElement; }
        }

        private static Geometry AsRectangleGeometry(Point start, Point end)
        {
            var angle = GeometryHelper.GetAngle(start, end);
            var width = GeometryHelper.GetWidth(start, end);

            return new RectangleGeometry(new Rect(start.X, start.Y, width, Height), 0, 0, new RotateTransform(angle, start.X, start.Y));
        }

        public Wall()
		{ }

		public Wall(Map map) : base(map)
		{
			EndPoint = new Point();
		}

		public override void MouseDown(Canvas canvas, Point point)
		{
			StartPosition = EndPoint = point;
			ApplyTo(canvas);
		}

		public override void MouseMove(Canvas canvas, Point point)
		{
			EndPoint = point;

			_uiElement.Width = GeometryHelper.GetWidth(StartPosition, EndPoint);
			_uiElement.RenderTransform = new RotateTransform(GeometryHelper.GetAngle(StartPosition, EndPoint));

		    _geometry = null;
			_uiElement.Fill = IsValid() ? Brushes.Black : Brushes.Red;
		}

		public override void ApplyTo(Canvas canvas)
		{
			_uiElement = new Rectangle
				{
					Height = Height,
					Fill = Brushes.Black,
					Width = GeometryHelper.GetWidth(StartPosition, EndPoint)
				};

			Canvas.SetLeft(_uiElement, StartPosition.X);
			Canvas.SetTop(_uiElement, StartPosition.Y);

			_uiElement.RenderTransform = new RotateTransform(GeometryHelper.GetAngle(StartPosition, EndPoint));

			canvas.Children.Add(_uiElement);
		}

		public override void Remove(Canvas canvas)
		{
			canvas.Children.Remove(_uiElement);
			Map.Remove(this);
		}
	}
}
