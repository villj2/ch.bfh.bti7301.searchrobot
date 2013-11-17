using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.WayDecision
{
    public abstract class WayDecision
    {
        protected Point Point;
        protected Random Random;

        public WayDecision()
        {
            Random = new Random();
            Point = new Point();
        }

        //public abstract Point GetWaypoint();

        public virtual Point GetWaypoint()
        {
            Point p = new Point();
            p.X = Random.Next(0, 800);
            p.Y = Random.Next(0, 600);
            p.Status = MapElementStatus.Waypoint;

            return p;
        }
    }
}
