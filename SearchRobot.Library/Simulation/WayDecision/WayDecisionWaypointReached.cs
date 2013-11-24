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
                    }
                }
            }
            // FIXME just4testing -> set goal manually because sensor is not delivering that info at the moment
            goal.X = 799;
            goal.Y = 1;

            double shiftX = goal.X - _posX;
            double shiftY = goal.Y - _posY;

            int dirX = shiftX > 0 ? 1 : -1;
            int dirY = shiftY > 0 ? 1 : -1;

            shiftX = Math.Abs(shiftX);
            shiftY = Math.Abs(shiftY);

            double stepX;
            double stepY;

            int loopCount = 0;

            if (shiftX >= shiftY)
            {
                loopCount = (int)shiftX;
                stepX = 1 * dirX;
                stepY = (1 / shiftX * shiftY) * dirY;
            }
            else
            {
                loopCount = (int)shiftY;
                stepX = (1 / shiftY * shiftX) * dirX;
                stepY = 1 * dirY;
            }

            bool wayClear = true;

            // step through array to goal (direct line) and check if there is a collision on its way
            /*
             * R x 0 0 0 0 0 0 0
             * 0 0 x x 0 0 0 0 0
             * 0 0 0 0 x x 0 0 0
             * 0 0 0 0 0 0 x x 0
             * 0 0 0 0 0 0 0 0 T
             * 
             * x = path
             */

            double x = _posX;
            double y = _posY;
            for (int i = 0; i < loopCount; i++)
            {
                x += stepX;
                y += stepY;

                MapElementStatus status = _me.GetStatus((int)x, (int)y);
                if (status == MapElementStatus.Blocked
                    || status == MapElementStatus.BlockedShadowed)
                {
                    wayClear = false;
                    break;
                }
            }

            return GetRandomPoint(MapElementStatus.WaypointVisited);
        }
    }
}
