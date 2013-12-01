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
            /*
            // Every time Robot collides while driving backwards, ReverseCollisions increments and checked if max. ReverseCollisions are reached
            // If Robot collides while driving normally, after a few seconds the ReverseCollisions are set to 0. But while this timer is running and he collides again backwards, timer is stopped.
            // -> If the Robot collides frequently while driving backwards, he is stuck.
            if (_dispatcherTimer == null)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(dispatcherTimerTick);
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            }

            if (WayDecision.IgnoreDirection)
            {
                WayDecision.ReverseCollisions++;

                if (WayDecision.ReverseCollisions >= 10)
                {
                    // TODO handle robot stuck
                    Console.WriteLine("Too many collisions, Robot is stuck");
                }

                _dispatcherTimer.Stop();
            }
            else
            {
                if(!_dispatcherTimer.IsEnabled) _dispatcherTimer.Start();
            }
            */

            _point = new Point();
            _random = new Random();
        }

        /*
        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            WayDecision.ReverseCollisions = 0;
            _dispatcherTimer.Stop();
        }
        */

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
