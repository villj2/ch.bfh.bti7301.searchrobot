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
        { }

        public override Point GetWaypoint()
        {
            Console.WriteLine("waypoint reached");
            _me.SetStatus(_posX, _posY, MapElementStatus.WaypointVisited);

            // TODO check if endpoint is reachable
            // 1) check if Type "Target" is in mapExplored
            // 2) check if direct line is available
            // 3) if available set waypoint at the position as goal
            // 4) if not, set random new waypoint (oder whatever)

            Point goal = new Point();
            for (int i = _me.GetStartIndex(0); i < _me.GetEndIndex(0); i++)
            {
                for (int j = _me.GetStartIndex(1); j < _me.GetEndIndex(1); j++)
                {
                    if (_me.GetStatus(i, j) == MapElementStatus.Target)
                    {
                        goal.X = i;
                        goal.Y = j;
                        goal.Status = MapElementStatus.Target;
                    }
                }
            }

            bool wayClear = _me.PathAvailable(goal, new Point((int)_posX, (int)_posY));

            return GetRandomPoint(MapElementStatus.WaypointVisited);
        }
    }
}
