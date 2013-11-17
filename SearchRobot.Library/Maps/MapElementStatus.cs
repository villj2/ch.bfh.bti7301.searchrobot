using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Maps
{
    public enum MapElementStatus
    {
        Undiscovered,
        Discovered,
        Blocked,
        Visited,
        Waypoint,
        WaypointVisited,
        Target,
        Collided
    }
}
