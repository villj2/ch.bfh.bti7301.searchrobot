using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchRobot.Library.Maps
{
    public abstract class MapElement
    {
        protected Map Map { get; private set; }

        protected MapElement(Map map)
        {
            Map = map;
            StartPosition = new Point();
        }

        public Point StartPosition { get; set; }

        public abstract void MouseDown(Canvas canvas, Point point);

        public abstract void MouseUp(Canvas canvas, Point point);

        public abstract void MouseMove(Canvas canvas, Point point);
    }
}
