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
        protected double _posX;
        protected double _posY;
        protected MapExplored _me;
        protected Point _point;
        protected Random _random;

        public WayDecision()
        {
            init();
        }

        public WayDecision(double posX, double posY, MapExplored me)
        {
            _posX = posX;
            _posY = posY;
            _me = me;

            init();
        }

        private void init()
        {
            _point = new Point();
            _random = new Random();
        }

        //public abstract Point GetWaypoint();

        public virtual Point GetWaypoint()
        {
            return GetRandomPoint(MapElementStatus.Undiscovered);
        }

        protected Point GetRandomPoint(MapElementStatus status)
        {
            _point.X = _random.Next(0, 800);
            _point.Y = _random.Next(0, 600);
            _point.Status = status;

            return _point;
        }
    }
}
