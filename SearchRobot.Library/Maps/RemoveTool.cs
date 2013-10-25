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
    public class RemoveTool : MapTool
    {
        public RemoveTool(Map map) : base(map)
        { }

        public override void MouseDown(Canvas canvas, Point point)
        {
            var clickedElement = GetElementAtPoint(canvas, point);

            if (clickedElement != null)
            {
                clickedElement.Remove(canvas);
            }
        }

        public override void MouseUp(Canvas canvas, Point point)
        {
            throw new NotImplementedException();
        }

        public override void MouseMove(Canvas canvas, Point point)
        { }

        public override void MouseLeave(Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
}
