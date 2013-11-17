using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionWaypointReached : WayDecision
    {
        public WayDecisionWaypointReached(double posX, double posY, MapExplored me) : base(posX, posY, me)
        {
        }

        public override Point GetWaypoint()
        {
            // TODO implement Waypoint reached logic

            Console.WriteLine("waypoint reached");

            _me.SetStatus(_posX, _posY, MapElementStatus.WaypointVisited);

            return GetRandomPoint(MapElementStatus.WaypointVisited);
        }
    }
}
