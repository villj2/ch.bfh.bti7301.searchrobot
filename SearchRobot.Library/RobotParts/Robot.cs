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
        //private Rectangle _uiElement;
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
            get { return 360 - Direction; }
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
            SetDirection(Direction);

            Console.WriteLine("Robot Map: " + Map);
        }

	    protected override Geometry GeometryShape
	    {
            get
            {
                return new EllipseGeometry(GeometryHelper.Convert(StartPosition), Size / 2, Size / 2);

                //var rect = new Rect(StartPosition.X - 15, StartPosition.Y - 15, Size, Size);
                // return new RectangleGeometry(rect, 0, 0, new RotateTransform(Direction, Size / 2, Size / 2));
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
            
            
            // FIXME just4testing - draw rectangle (same as collision detection)
            /*
            _uiElement = new Rectangle
            {
                Height = Size,
                Fill = Brushes.Black,
                Width = Size
            };

            Canvas.SetLeft(_uiElement, StartPosition.X);
            Canvas.SetTop(_uiElement, StartPosition.Y);
            _uiElement.RenderTransform = new RotateTransform(Direction + 90, 15, 15);
            _uiElement.Fill = Brushes.DarkGreen;
            */

            //_uiElement = new Ellipse { Width = Size, Height = Size, Fill = Brushes.DarkGreen };

            SetPos(StartPosition.X, StartPosition.Y);
			SetDirection(Direction);

			canvas.Children.Add(_uiElement);
		}

		//public CartesianArray<MapElementStatus> GetView()
        public List<Point> GetView()
		{
            //_mapExplored.UpdateSensordata((new PointRotator(Direction)).Rotate(_sensor.GetView()), StartPosition);
            var result = (new PointRotator(CartasianDirection)).Rotate(_sensor.GetView());

            // DebugHelper.StoreAsBitmap(string.Format("C:\\SensorImageR-{0}.png", DateTime.Now.Ticks), result);

            _mapExplored.UpdateSensordata(result.ToArray(), StartPosition);
            //_mapExplored.UpdateSensordata(result, StartPosition);

            //var mapArray = (new PointRotator(Direction)).Rotate(_sensor.GetView()).ToArray();
            //var mapCartesianArray = (new PointRotator(Direction)).Rotate(_sensor.GetView());

			//return _sensor.GetView();




            // FIXME just4testing start dijkstra
            _brain.waypoints = new DijkstraHelper().GetPath(StartPosition, new Point(799, 599), _mapExplored);
            _brain.MapExplored.WaypointActive = _brain.waypoints[0];

            return _brain.waypoints;
		}

        public void Move()
        {
			MovementObject mo = _brain.GetNextMove(_positionX, _positionY, Direction);

            // temporarily deactivate robot to avoid collision with clone
            IsCollidable = false;
            
            // create clone for collision dection of next move
            var collisionDummy = (Robot)this.Clone();

            collisionDummy.ApplyTo(_mapArea);
            Map.Add(collisionDummy);
            collisionDummy.Bind(Map);

            // set new position
            collisionDummy.SetPos(mo.X, mo.Y);

            if (collisionDummy.IsOverlapping())
            {
                collisionDummy.Dispose();
                collisionDummy.Remove(_mapArea);

                _brain.Collision(_positionX, _positionY, mo);
                IsCollidable = true;

                return;
            }

            collisionDummy.Dispose();
            collisionDummy.Remove(_mapArea);
            IsCollidable = true;


            SetPos(mo.X, mo.Y);
            SetDirection(mo.Direction);

            //Console.WriteLine("isValid: " + IsValid());
        }

        public void SetPos(double x, double y)
        {
            _positionX = x;
            _positionY = y;

            StartPosition.X = (int)_positionX;
            StartPosition.Y = (int)_positionY;

            //_position.X = (int)Math.Round(_positionExactlyX);
            //_position.Y = (int)Math.Round(_positionExactlyY);

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
