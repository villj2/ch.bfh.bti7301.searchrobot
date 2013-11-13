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
using SearchRobot.Library.Simulation;

namespace SearchRobot.Library.RobotParts
{
    public class Robot : UniqueMandatoryMapElement, IDisposable
    {
        private const int Size = 30;
        public MapExplored MapExplored { get { return _mapExplored; } }

        private Polyline _uiElement;
        private MapExplored _mapExplored;
        private Brain _brain;
        private Sensor _sensor;
        private double _positionX;
        private double _positionY;
        private double _direction;

        public double Direction { get; set; }

        public Robot(Map map) : base(map) {}

        internal Robot() { }

        public void initialize()
        {
            Console.WriteLine("Robot initialize");

            _mapExplored = new MapExplored();
            _brain = new Brain(_mapExplored);

            SetPos(StartPosition.X, StartPosition.Y);
            SetDirection(_direction);

            Console.WriteLine("Robot Map: " + Map);
        }

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
            SetDirection(GeometryHelper.GetAngle(StartPosition, point));
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

            SetPos(StartPosition.X, StartPosition.Y);
            SetDirection(_direction);

			canvas.Children.Add(_uiElement);
		}

        public void Move()
        {
            MovementObject mo = _brain.GetNextMove(_positionX, _positionY, _direction);

            SetPos(mo.X, mo.Y);
            SetDirection(mo.Direction);

            // Problem: Roboter ist nicht unique!!
            Console.WriteLine("IsUnique: " + IsUnique());
            Console.WriteLine("isValid: " + IsValid());
        }

        public void SetPos(double x, double y)
        {
            _positionX = x;
            _positionY = y;

            StartPosition.X = (int)_positionX;
            StartPosition.Y = (int)_positionY;

            //_position.X = (int)Math.Round(_positionExactlyX);
            //_position.Y = (int)Math.Round(_positionExactlyY);

            Canvas.SetLeft(_uiElement, StartPosition.X - 15);
            Canvas.SetTop(_uiElement, StartPosition.Y - 15);
        }

        public void SetDirection(double direction)
        {
            _direction = direction;
            _uiElement.RenderTransform = new RotateTransform(_direction + 90, 15, 15);
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

        public void Dispose()
        {
            _mapExplored.Dispose();
            _brain.Dispose();
        }
    }
}
