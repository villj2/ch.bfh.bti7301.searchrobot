using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SearchRobot.Library.Simulation.WayDecision
{
    public abstract class WayDecision
    {
        public static bool IgnoreDirection = false;
        public static int Collisions = 0;

        protected double _posX;
        protected double _posY;
        protected MapExplored _me;
        protected Point _point;
        protected Random _random;

        private static DispatcherTimer _dispatcherTimer;

        protected WayDecision()
        {
            init();
        }

        protected WayDecision(double posX, double posY, MapExplored me)
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
