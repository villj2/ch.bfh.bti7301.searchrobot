using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SearchRobot.Library.Maps
{
    public class Robot : UniqueMandatoryMapElement
    {
        Polyline _uiElement = new Polyline();
        private Label _label;

        public double Direction { get; set; }

        public Robot(Map map, Label label) : base(map)
        {
            _label = label;
        }

	    protected override Geometry GeometryShape
	    {
			get { return _uiElement.RenderedGeometry; }
	    }

	    public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = point;
			ApplyTo(canvas);
        }

        private double GetAngle(Point start, Point end)
        {
            var y = end.Y - start.Y;
            var x = end.X - start.X;

            return Math.Atan2(y, x) / Math.PI * 180;
        }

        public override void MouseUp(Canvas canvas, Point point)
		{
			if (IsUnique())
			{
				Map.Add(this);
			}
			else
			{
				canvas.Children.Remove(_uiElement);
			}
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            Direction = GetAngle(StartPosition, point);
            _uiElement.RenderTransform = new RotateTransform(Direction + 90, 15, 15);
        }

	    public override void ApplyTo(Canvas canvas)
		{
			_uiElement = new Polyline();

			_uiElement.Points.Add(new System.Windows.Point(10, 30));
			_uiElement.Points.Add(new System.Windows.Point(10, 20));
			_uiElement.Points.Add(new System.Windows.Point(0, 20));
			_uiElement.Points.Add(new System.Windows.Point(15, 0));
			_uiElement.Points.Add(new System.Windows.Point(30, 20));
			_uiElement.Points.Add(new System.Windows.Point(20, 20));
			_uiElement.Points.Add(new System.Windows.Point(20, 30));

			_uiElement.Width = 30;
			_uiElement.Height = 30;

			_uiElement.Fill = Brushes.DarkGreen;

			_uiElement.RenderTransform = new RotateTransform(Direction, 15, 15);

			Canvas.SetLeft(_uiElement, StartPosition.X - 15);
			Canvas.SetTop(_uiElement, StartPosition.Y - 15);

			canvas.Children.Add(_uiElement);
	    }

	    public override void Remove(Canvas canvas)
	    {
			canvas.Children.Remove(_uiElement);
			Map.Remove(this);
	    }
    }
}
