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
    public class MoveTool : MapTool
    {
        private MapElement _mapElement;

        public MoveTool(Map map) : base(map)
        { }

        public override void MouseDown(Canvas canvas, Point point)
        {
            _mapElement = GetElementAtPoint(canvas, point);
        }
    }
}
