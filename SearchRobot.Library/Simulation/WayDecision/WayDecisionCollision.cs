using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Point = SearchRobot.Library.Maps.Point;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionCollision : WayDecision
    {
        private MovementObject _mo;
        protected static DispatcherTimer _dispatcherTimer;

        public WayDecisionCollision(double posX, double posY, MovementObject mo, MapExplored me) : base(posX, posY, me)
        {
            _mo = mo;

            init();
        }

        public override Point GetWaypoint()
        {
            Console.WriteLine("COLLISION");

            // update collided point status
            _me.SetStatus(_mo.X, _mo.Y, MapElementStatus.Collided);

            Vector vector = new Vector(_mo.X - _posX, _mo.Y - _posY);
            
            // opposite direction
            vector.Negate();

            // normalize vector (length = 1)
            vector.Normalize();

            // calculate distances to every border
            double tLeft = (-_posX) / vector.X;
            double tRight = (800 - _posX) / vector.X;
            double tTop = (-_posY) / vector.Y;
            double tBottom = (600 - _posY) / vector.Y;

            // now choose the smallest positive!
            /*
            List<double> distancesToBorder = new List<double>();
            if (tLeft > 0) distancesToBorder.Add(tLeft);
            if (tRight > 0) distancesToBorder.Add(tRight);
            if (tTop > 0) distancesToBorder.Add(tTop);
            if (tBottom > 0) distancesToBorder.Add(tBottom);

            double min = int.MaxValue;
            foreach (double val in distancesToBorder)
            {
                if (val < min) min = val;
            }*/

            //vector *= min / 2;
            vector *= 20;

            _point.X = (int)_posX + (int)vector.X;
            _point.Y = (int)_posY + (int)vector.Y;

            _point.Status = MapElementStatus.Waypoint;

            return _point;
        }

        private void init()
        {
            // Every time Robot collides while driving backwards, ReverseCollisions increments and checked if max. ReverseCollisions are reached
            // If Robot collides while driving normally, after a few seconds the ReverseCollisions are set to 0. But while this timer is running and he collides again backwards, timer is stopped.
            // -> If the Robot collides frequently while driving backwards, he is stuck.
            if (_dispatcherTimer == null)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(dispatcherTimerTick);
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            }

            WayDecision.Collisions++;

            if (WayDecision.Collisions >= 10)
            {
                // TODO block game
                SimulationEngine.ShowInfo("Robot stuck!");
            }

            _dispatcherTimer.Stop();
            _dispatcherTimer.Start();
            
            //if (!_dispatcherTimer.IsEnabled) _dispatcherTimer.Start();


            
            
        }

        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            WayDecision.Collisions = 0;
            _dispatcherTimer.Stop();
        }
    }
}
