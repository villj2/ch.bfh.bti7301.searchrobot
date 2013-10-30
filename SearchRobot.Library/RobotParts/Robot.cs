using System.Windows;
using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = SearchRobot.Library.Maps.Point;

namespace SearchRobot.Library.RobotParts
{
    public class Robot : UniqueMandatoryMapElement
    {
        private const int Size = 30;
        Polyline _uiElement = new Polyline();
        MapExplored _mapExplored = new MapExplored();
        public double Direction { get; set; }

        public Robot(Map map)
            : base(map)
        {
            // FIXME just4testing set waypoint
            _mapExplored = new MapExplored();
            Point waypoint = new Point();
            waypoint.X = 333;
            waypoint.Y = 333;
            waypoint.Status = MapElementStatus.Blocked;

            _mapExplored.AddPoint(waypoint);


            Point pointToUpdate = new Point();
            pointToUpdate.X = 333;
            pointToUpdate.Y = 333;
            _mapExplored.SetStatus(pointToUpdate, MapElementStatus.Waypoint);


            // Vorgehen
            // Roboter sagt: Ich will 30° drehen und 15Px nach rechts bewegen.
            // Anschliessend führt die Simulation diese Bewegung des Roboters aus. (moveTo)
            // Aber grundsätzlich berechnet der Roboter wie genau er sich bewegt
        }

        internal Robot() { }

	    protected override Geometry GeometryShape
	    {
            get
            {
                return new RectangleGeometry(new Rect(StartPosition.X, StartPosition.Y, Size, Size), 0, 0,
                                       new RotateTransform(Direction, StartPosition.X, StartPosition.Y));
            }
	    }

        public override UIElement UiElement
        {
            get { return _uiElement; }
        }

        public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = point;
			ApplyTo(canvas);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            Direction = GeometryHelper.GetAngle(StartPosition, point);
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

			_uiElement.RenderTransform = new RotateTransform(Direction + 90, 15, 15);

			//Canvas.SetLeft(_uiElement, StartPosition.X - 15);
			//Canvas.SetTop(_uiElement, StartPosition.Y - 15);
            MoveTo(StartPosition);

			canvas.Children.Add(_uiElement);
		}

        public void ExecuteCycle()
        {

        }

        public void MoveTo(Point point)
        {
            Canvas.SetLeft(_uiElement, point.X - 15);
            Canvas.SetTop(_uiElement, point.Y - 15);
        }

	    public override void Remove(Canvas canvas)
	    {
			canvas.Children.Remove(_uiElement);
			Map.Remove(this);
	    }

		public override void Move(Canvas canvas, int offsetX, int offsetY)
		{
			StartPosition.X += offsetX;
			StartPosition.Y += offsetY;

			Canvas.SetLeft(_uiElement, Canvas.GetLeft(_uiElement) + offsetX);
			Canvas.SetTop(_uiElement, Canvas.GetTop(_uiElement) + offsetY);
		}

	    /// <summary>
	    /// Creates a new object that is a copy of the current instance.
	    /// </summary>
	    /// <returns>
	    /// A new object that is a copy of this instance.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override object Clone()
	    {
		    return new Robot {StartPosition = this.StartPosition.Clone(), Direction = this.Direction};
	    }
    }
}
