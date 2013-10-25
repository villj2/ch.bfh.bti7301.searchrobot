using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchRobot.Library.Maps
{
    public abstract class MapTool : ICanvasListener
    {
        private readonly Map _map;

        protected MapTool(Map map)
        {
            _map = map;
        }

        protected MapElement GetElementAtPoint(Canvas canvas, Point point)
        {
            var result = VisualTreeHelper.HitTest(canvas, GeometryHelper.Convert(point));
            return _map.Elements.FirstOrDefault(ele => Equals(ele.UiElement, result.VisualHit));
        }

        public abstract void MouseDown(Canvas canvas, Point point);

        public virtual void MouseUp(Canvas canvas, Point point)
        { }

        public virtual void MouseMove(Canvas canvas, Point point)
        { }

        public virtual void MouseLeave(Canvas canvas)
        { }
    }
}
