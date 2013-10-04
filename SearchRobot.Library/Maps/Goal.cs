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
    public class Goal : UniqueMandatoryMapElement
    {
        private Ellipse _uiElement;

        public Goal(Map map) : base(map)
        {
        }

        public override void MouseDown(Canvas canvas, Point point)
        {
            _uiElement = new Ellipse();

            StartPosition = point;

            _uiElement = new Ellipse();
            _uiElement.Width = 10;
            _uiElement.Height = 10;
            _uiElement.Fill = Brushes.DarkRed;

            Canvas.SetLeft(_uiElement, point.X);
            Canvas.SetTop(_uiElement, point.Y);

            canvas.Children.Add(_uiElement);
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            Map.Add(this);
        }

        public override void MouseMove(Canvas canvas, Point point)
        {
        }
    }
}
