using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Point = SearchRobot.Library.Maps.Point;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionCollision : WayDecision
    {
        private MovementObject _mo;

        public WayDecisionCollision(double posX, double posY, MovementObject mo, MapExplored me) : base(posX, posY, me)
        {
            _mo = mo;
        }

        public override Point GetWaypoint()
        {
            // TODO implement collision logic (opposite angle)
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
            vector *= 100;

            _point.X = (int)_posX + (int)vector.X;
            _point.Y = (int)_posY + (int)vector.Y;

            _point.Status = MapElementStatus.Waypoint;

            return _point;
            //return GetRandomPoint(MapElementStatus.Waypoint);
        }
    }
}
