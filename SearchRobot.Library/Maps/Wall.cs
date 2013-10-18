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
        private Line _uiElement;
	    private Geometry _geometry;

		private double GetAngle(Point start, Point end)
		{
			var y = end.Y - start.Y;
			var x = end.X - start.X;

			return Math.Atan2(y, x) / Math.PI * 180;
		}

		public Geometry AsRectangleGeometry(Point start, Point end)
		{
			var angle = GetAngle(start, end);
			var width = Math.Sqrt(Math.Pow(Math.Abs(start.X - end.X), 2) + Math.Pow(Math.Abs(start.Y - end.Y), 2));

			return new RectangleGeometry(new Rect(start.X, start.Y, width, 2), 0, 0, new RotateTransform(angle));
		}

        public Wall(Map map) : base(map)
        {
            EndPoint = new Point();
        }

        public Point EndPoint { get; set; }

	    protected override Geometry GeometryShape
	    {
		    get
		    {
			    return AsRectangleGeometry(StartPosition, EndPoint);

			    return new LineGeometry(
					new System.Windows.Point(StartPosition.X, StartPosition.Y),
					new System.Windows.Point(EndPoint.X, EndPoint.Y));
		    }
	    }

	    public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = EndPoint = point;

			ApplyTo(canvas);
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            Map.Add(this);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            _uiElement.X2 = point.X;
            _uiElement.Y2 = point.Y;

	        bool isOverlapping = IsOverlapping();

	        _uiElement.Fill = isOverlapping ? Brushes.Red : Brushes.Black;
			_uiElement.Stroke = new SolidColorBrush(IsOverlapping() ? Color.FromRgb(255,0,0) : Color.FromRgb(0,0,0));
        }

	    public override void ApplyTo(Canvas canvas)
	    {
			_uiElement = new Line
				{
					Fill = Brushes.Black,
					StrokeThickness = 2,
					Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
					X1 = StartPosition.X,
					Y1 = StartPosition.Y,
					X2 = EndPoint.X,
					Y2 = EndPoint.Y
				};

		    canvas.Children.Add(_uiElement);
	    }

	    public override void Remove(Canvas canvas)
	    {
			canvas.Children.Remove(_uiElement);
			Map.Remove(this);
	    }
    }
}
