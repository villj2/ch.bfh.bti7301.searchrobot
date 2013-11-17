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
        private double _posX;
        private double _posY;
        private MovementObject _mo;
        private MapExplored _me;
        private double _collisionAngle;

        public WayDecisionCollision(double posX, double posY, MovementObject mo, MapExplored me) : base()
        {
            _posX = posX;
            _posY = posY;
            _mo = mo;
            _me = me;
        }

        public override Point GetWaypoint()
        {
            // TODO implement collision logic

            // calculate angle between robot and collided obstacle
            _collisionAngle = GeometryHelper.GetAngleAbsolute(_posX, _posY, _mo.X, _mo.Y);

            // update collided point status
            _me.SetStatus(_mo.X, _mo.Y, MapElementStatus.Collided);

            Point p = new Point();
            p.X = Random.Next(0, 800);
            p.Y = Random.Next(0, 600);
            p.Status = MapElementStatus.Waypoint;

            return p;
        }
    }
}
