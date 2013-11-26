using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionCollisionBackwards : WayDecisionCollision
    {
        public WayDecisionCollisionBackwards(double posX, double posY, MovementObject mo, MapExplored me) : base(posX, posY, mo, me)
        {

        }
    }
}
