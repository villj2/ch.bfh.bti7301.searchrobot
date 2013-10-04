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
    public class Disc : MapElement
    {

        public int Radius { get; private set; }
        private Ellipse _uiElement;

        public Disc(Map map) : base(map)
        {
        }

        public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = point;

            _uiElement = new Ellipse();
            _uiElement.Width = 0;
            _uiElement.Height = 0;
            _uiElement.Fill = Brushes.Black;

            Canvas.SetLeft(_uiElement, point.X);
            Canvas.SetTop(_uiElement, point.Y);

            canvas.Children.Add(_uiElement);
        }

        private int GetRadius(Point start, Point end)
        {
            int x = Math.Abs(start.X - end.X);
            int y = Math.Abs(start.Y - end.Y);

            return Convert.ToInt32(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            this.Map.Add(this);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            Radius = GetRadius(StartPosition, point);

            _uiElement.Width = _uiElement.Height = 2 * Radius;
            
            Canvas.SetLeft(_uiElement, StartPosition.X - Radius);
            Canvas.SetTop(_uiElement, StartPosition.Y - Radius);
        }
    }
}
