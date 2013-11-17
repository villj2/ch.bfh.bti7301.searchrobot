using SearchRobot.Library.Maps;
using SearchRobot.Library.Simulation;
using SearchRobot.Library.Simulation.WayDecision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchRobot.Library.RobotParts
{
    public class Brain : IDisposable
    {
        private MapExplored _mapExplored;

        private Random _rnd;

        public Brain(MapExplored mapExplored) {

            _mapExplored = mapExplored;

            _rnd = new Random();

            // set first waypoint
            CreateNextWaypoint();
        }

        public MovementObject GetNextMove(double posX, double posY, double currentDirection)
        {
            // check if waypoint is reached
            if (GeometryHelper.ComparePointsWithRange(posX, posY, _mapExplored.WaypointActive.X, _mapExplored.WaypointActive.Y, 5))
            {
                Console.WriteLine("waypoint reached");
                _mapExplored.SetStatus(posX, posY, MapElementStatus.WaypointVisited);
                CreateNextWaypoint();
            }

            MovementObject settingNew = new MovementObject();
            settingNew.X = posX;
            settingNew.Y = posY;
            settingNew.Direction = currentDirection;

            double targetDirection = CalculateTargetDirection(posX, posY, _mapExplored.WaypointActive);
            //Console.WriteLine("targetDirection: " + targetDirection);

            // either change direction or position
            if ((currentDirection + 360) % 360 != (targetDirection + 360) % 360)
            {
                settingNew.Direction = AdjustDirection(currentDirection, targetDirection);
            }
            else
            {
                MovementObject positionNew = GetNextMovementPoint(posX, posY);

                settingNew.X = positionNew.X;
                settingNew.Y = positionNew.Y;
            }

            // add new movement point to map explored and mark as VISITED
            if (_mapExplored.GetStatus(settingNew.X, settingNew.Y) == MapElementStatus.Undiscovered)
            {
                _mapExplored.SetStatus(settingNew.X, settingNew.Y, MapElementStatus.Visited);
            }
              
            return settingNew;
        }

        /// <summary>
        /// Robot recognizes collision with obstacle and tells brain.
        /// Brain adds point to mapExplored and calculates new Waypoint.
        /// </summary>
        /// <param name="mo"></param>
        public void Collision(double posX, double posY, MovementObject mo)
        {
            CreateNextWaypoint(new WayDecisionCollision(posX, posY, mo, _mapExplored));
        }

        /// <summary>
        /// calculates new waypoint based on mapExplored and _wayDecisionStatus
        /// </summary>
        /// <returns></returns>
        private void CreateNextWaypoint()
        {
            CreateNextWaypoint(new WayDecisionInit());
        }

        private void CreateNextWaypoint(WayDecision wayDecision)
        {
            _mapExplored.WaypointActive = wayDecision.GetWaypoint();
        }

        

        /* calculates new movementPoint based on next waypoint
        /****************************************************************/
        private MovementObject GetNextMovementPoint(double x, double y)
        {
            var movementObject = new MovementObject();

            double movementX = _mapExplored.WaypointActive.X - x;
            double movementY = _mapExplored.WaypointActive.Y - y;

            double posX;
            double posY;

            int dirX = movementX >= 0 ? 1 : -1;
            int dirY = movementY >= 0 ? 1 : -1;

            movementX = Math.Abs(movementX);
            movementY = Math.Abs(movementY);

            if (movementX >= movementY)
            {
                posX = x + 1 * dirX;
                posY = y + (1 / movementX * movementY) * dirY;
            }
            else
            {
                posY = y + 1 * dirY;
                posX = x + (1 / movementY * movementX) * dirX;
            }

            movementObject.X = posX;
            movementObject.Y = posY;

            return movementObject;
        }

        /* calculates new direction based on next waypoint
        /****************************************************************/
        private double AdjustDirection(double currentDirection, double targetDirection)
        {
            double currentDirectionOri = currentDirection;

            if(currentDirection < 0) currentDirection += 360;
            if(targetDirection < 0) targetDirection += 360;

            bool dir = ((currentDirection) - (targetDirection) + 360) % 360 > 180;
            if (dir)
            {
                currentDirection = currentDirection + 1;
            }
            else
            {
                currentDirection = currentDirection - 1;
            }

            return currentDirection % 360;
        }

        private double CalculateTargetDirection(double posX, double posY, Point waypoint)
        {
            return Math.Floor(GeometryHelper.GetAngle(posX, posY, waypoint.X, waypoint.Y));
        }

        public void Dispose()
        {
            _mapExplored.Dispose();
        }
    }
}
