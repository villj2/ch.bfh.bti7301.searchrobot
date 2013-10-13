using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SearchRobot.Library.Robot
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

        public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = point;

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

            _uiElement.RenderTransform = new RotateTransform(30, 15, 15);
            _uiElement.RenderTransform = new TransformGroup();

            Canvas.SetLeft(_uiElement, point.X);
            Canvas.SetTop(_uiElement, point.Y);
            
            canvas.Children.Add(_uiElement);
        }

        private double GetAngle(Point start, Point end)
        {
            var y = end.Y - start.Y;
            var x = end.X - start.X;

            return Math.Atan2(y, x) / Math.PI * 180;
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            this.Map.Add(this);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            Direction = GetAngle(StartPosition, point);
            _uiElement.RenderTransform = new RotateTransform(Direction + 90, 15, 15);
        }
    }
}
