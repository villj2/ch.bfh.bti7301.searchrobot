using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SearchRobot.Library.Maps
{
    public class TrackingMapEntry
    {
        public MapElementStatus status;
        public UIElement point;

        public TrackingMapEntry()
        {
            status = MapElementStatus.Undiscovered;
        }
    }
}
