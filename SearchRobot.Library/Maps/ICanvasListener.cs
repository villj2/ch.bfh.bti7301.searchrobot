using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchRobot.Library.Maps
{
    public interface ICanvasListener
    {
        void MouseDown(Canvas canvas, Point point);

        void MouseUp(Canvas canvas, Point point);

        void MouseMove(Canvas canvas, Point point);

        void MouseLeave(Canvas canvas);
    }
}
