using SearchRobot.Library.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation.WayDecision
{
    class WayDecisionCollision : WayDecision
    {
        private MovementObject _mo;
        private double _collisionAngle;

        public WayDecisionCollision(double posX, double posY, MovementObject mo, MapExplored me) : base(posX, posY, me)
        {
            _mo = mo;
        }

        public override Point GetWaypoint()
        {
            // TODO implement collision logic (opposite angle)
            Console.WriteLine("COLLISION");

            // calculate angle between robot and collided obstacle
            _collisionAngle = GeometryHelper.GetAngleAbsolute(_posX, _posY, _mo.X, _mo.Y);

            // update collided point status
            _me.SetStatus(_mo.X, _mo.Y, MapElementStatus.Collided);

            Console.WriteLine("_collisionAngle: " + _collisionAngle);

            //Point B = Math.Tan(

            // calculate opposite angle
            _point.X = _random.Next(0, 800);
            _point.Y = _random.Next(0, 600);
            _point.Status = MapElementStatus.Waypoint;

            return _point;
        }
    }
}
