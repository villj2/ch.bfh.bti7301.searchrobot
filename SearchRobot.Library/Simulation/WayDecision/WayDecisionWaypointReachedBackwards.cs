using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionWaypointReachedBackwards : WayDecision
    {

        public WayDecisionWaypointReachedBackwards(double posX, double posY, MapExplored me)  : base(posX, posY, me)
        { }

        public override Point GetWaypoint()
        {
            // update collided point status
            _me.SetStatus(_posX, _posY, MapElementStatus.Remove);

            return new WayDecisionWaypointReached(_posX, _posY, _me).GetWaypointCustom(true);
        }
    }
}
