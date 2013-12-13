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
using SearchRobot.Library.Simulation.WayDecision;
using SearchRobot.Library.Simulation.Dijkstra;

namespace SearchRobot.Library.RobotParts
{
    public class Robot : UniqueMandatoryMapElement, IDisposable
    {
        public const int Size = 15;

        public MapExplored MapExplored { get { return _mapExplored; } }

        //private Ellipse _uiElement;
        private Polyline _uiElement;
        private MapExplored _mapExplored;
        private Brain _brain;
        private Sensor _sensor;
        private double _positionX;
        private double _positionY;
        private Canvas _mapArea;

        public double Direction { get; set; }

        public double CartasianDirection
        {
            get { return Direction < 180 ? Direction : 360 + Direction; }
        }

        public Robot(Map map) : base(map) {}

        internal Robot() { }

        public void Initialize(Canvas mapArea, Sensor sensor)
        {
            Console.WriteLine("Robot initialize");

            _mapArea = mapArea;
            _mapExplored = new MapExplored();
            _brain = new Brain(_mapExplored, this);
	        _sensor = sensor;

            SetPos(StartPosition.X, StartPosition.Y);

            // normalize direction
            SetDirection(Direction > 0 ? 360 - Direction : Direction * -1);
        }

	    protected override Geometry GeometryShape
	    {
            get
            {
                return new EllipseGeometry(GeometryHelper.Convert(StartPosition), Size / 2, Size / 2);
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

            _uiElement.Points.Add(new System.Windows.Point(Size * 1/3, Size));
            _uiElement.Points.Add(new System.Windows.Point(Size * 1/3, Size * 2/3));
            _uiElement.Points.Add(new System.Windows.Point(0, Size * 2/3));
            _uiElement.Points.Add(new System.Windows.Point(Size * 1/2, 0));
            _uiElement.Points.Add(new System.Windows.Point(Size, Size * 2/3));
            _uiElement.Points.Add(new System.Windows.Point(Size * 2/3, Size * 2/3));
            _uiElement.Points.Add(new System.Windows.Point(Size * 2/3, Size));

			_uiElement.Width = Size;
			_uiElement.Height = Size;

			_uiElement.Fill = Brushes.DarkGreen;

            //_uiElement = new Ellipse { Width = Size, Height = Size, Fill = Brushes.DarkGreen };

            SetPos(StartPosition.X, StartPosition.Y);
			SetDirection(Direction);

			canvas.Children.Add(_uiElement);
		}

		public CartesianArray<MapElementStatus> GetView()
		{
		    return _sensor.GetView();
		}

        public List<Point> GetWayPoints()
        {
	        return _brain._waypointQueue.ToList();
        }

        public void Move()
        {
			MovementObject mo = _brain.GetNextMove(_positionX, _positionY, Direction);

            if (IsValidMove(mo))
            {
                SetPos(mo.X, mo.Y);
                SetDirection(mo.Direction);
            }
        }

        private bool IsValidMove(MovementObject mo)
        {
            Robot collisionDummy = null;

            try
            {
                IsCollidable = false;
                collisionDummy = (Robot)this.Clone();

                collisionDummy.ApplyTo(_mapArea);
                Map.Add(collisionDummy);
                collisionDummy.Bind(Map);

                // set new position
                collisionDummy.SetPos(mo.X, mo.Y);

                if (collisionDummy.IsOverlapping())
                {
                    _brain.Collision(_positionX, _positionY, mo);
                    return false;
                }

                return true;
            }
            finally
            {
                IsCollidable = true;
                if (collisionDummy != null)
                {
                    collisionDummy.Dispose();
                    collisionDummy.Remove(_mapArea);
                }
            }
        }

        public void SetPos(double x, double y)
        {
            _positionX = x;
            _positionY = y;

            StartPosition.X = (int)_positionX;
            StartPosition.Y = (int)_positionY;

            Canvas.SetLeft(_uiElement, StartPosition.X - Size / 2);
            Canvas.SetTop(_uiElement, StartPosition.Y - Size / 2);
        }

        public void SetDirection(double direction)
        {
			Direction = direction;
			_uiElement.RenderTransform = new RotateTransform(Direction + 90, Size / 2, Size / 2);
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
            if (_mapExplored != null) _mapExplored.Dispose();
            if (_brain != null) _brain.Dispose();
        }
    }
}
