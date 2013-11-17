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

        //private Rectangle _uiElement;
        private Polyline _uiElement;
        private MapExplored _mapExplored;
        private Brain _brain;
        private Sensor _sensor;
        private Robot _collisionDummy;
        private Canvas _mapArea;
        private double _positionX;
        private double _positionY;

        public double Direction { get; set; }

        public Robot(Map map) : base(map) {}

        internal Robot() { }

        public void Initialize(Sensor sensor)
        {
            Console.WriteLine("Robot initialize");

            _mapArea = mapArea;
            _mapExplored = new MapExplored();
            _brain = new Brain(_mapExplored);
	    _sensor = sensor;

            SetPos(StartPosition.X, StartPosition.Y);
            SetDirection(Direction);

            Console.WriteLine("Robot Map: " + Map);
        }

	    protected override Geometry GeometryShape
	    {
            get
            {
                var rect = new Rect(_positionX - 15, _positionY - 15, Size, Size);

                // FIXME Ausrichtung des Rects wohl noch nicht korrekt.
                //return new RectangleGeometry(rect, 0, 0, new RotateTransform(_direction, 15, 15));
                return new RectangleGeometry(rect, 0, 0, new RotateTransform(_direction, 30, 30));
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
            _uiElement.RenderTransform = new RotateTransform(_direction + 90, 15, 15);
            _uiElement.Fill = Brushes.DarkGreen;
            */




            SetPos(StartPosition.X, StartPosition.Y);
			SetDirection(Direction);

			canvas.Children.Add(_uiElement);
		}

		public CartesianArray<MapElementStatus> GetView()
		{
			return _sensor.GetView();
		}

        public void Move()
        {
			MovementObject mo = _brain.GetNextMove(_positionX, _positionY, Direction);

            // temporarily deactivate robot to avoid collision with clone
            IsCollidable = false;
            
            // create clone for collision dection of next move
            _collisionDummy = this.Clone() as Robot;
            _collisionDummy.ApplyTo(_mapArea);
            Map.Add(_collisionDummy);
            _collisionDummy.Bind(Map);

            // set new position
            _collisionDummy.SetPos(mo.X, mo.Y);

            if (_collisionDummy.IsOverlapping())
            {
                _collisionDummy.Dispose();
                _collisionDummy.Remove(_mapArea);

                _brain.Collision(_positionX, _positionY, mo);
                IsCollidable = true;

                return;
            }

            _collisionDummy.Dispose();
            _collisionDummy.Remove(_mapArea);
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

            Canvas.SetLeft(_uiElement, StartPosition.X - 15);
            Canvas.SetTop(_uiElement, StartPosition.Y - 15);
        }

        public void SetDirection(double direction)
        {
			Direction = direction;
			_uiElement.RenderTransform = new RotateTransform(Direction + 90, 15, 15);
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
