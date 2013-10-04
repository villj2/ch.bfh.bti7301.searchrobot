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
    public class Wall : MapElement
    {
        private Line _uiElement;

        public Wall(Map map) : base(map)
        {
            EndPoint = new Point();
        }

        public Point EndPoint { get; set; }

        public override void MouseDown(Canvas canvas, Point point)
        {
            StartPosition = EndPoint = point;

            _uiElement = new Line() { Fill = Brushes.Black, StrokeThickness = 2, Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0))};
            _uiElement.X1 = _uiElement.X2 = point.X;
            _uiElement.Y1 = _uiElement.Y2 = point.Y;

            canvas.Children.Add(_uiElement);
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            this.Map.Add(this);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
            _uiElement.X2 = point.X;
            _uiElement.Y2 = point.Y;
        }
    }
}
