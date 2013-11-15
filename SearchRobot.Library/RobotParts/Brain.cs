using SearchRobot.Library.Maps;
using SearchRobot.Library.Simulation;
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

        public Brain(MapExplored mapExplored) {

            _mapExplored = mapExplored;

            // set first waypoint
            CreateNextWaypoint();
        }

        public MovementObject GetNextMove(double posX, double posY, double currentDirection)
        {
            // check if waypoint is reached
            // FIXME waypoint kann teilweise nicht perfekt getroffen werden!!
            if (GeometryHelper.ComparePointsWithRange(posX, posY, _mapExplored.WaypointActive.X, _mapExplored.WaypointActive.Y, 5))
            {
                Console.WriteLine("waypoint reached");

                CreateNextWaypoint();
            }

            MovementObject settingNew = new MovementObject();
            settingNew.X = posX;
            settingNew.Y = posY;
            settingNew.Direction = currentDirection;

            double targetDirection = CalculateTargetDirection(posX, posY, _mapExplored.WaypointActive);

            // either change direction or position
            if (currentDirection != targetDirection)
            {
                settingNew.Direction = AdjustDirection(currentDirection, targetDirection);

                //Console.WriteLine("dir: " + settingNew.Direction);
            }
            else
            {
                MovementObject positionNew = GetNextMovementPoint(posX, posY);

                //Console.WriteLine("x: " + positionNew.X + "y: " + positionNew.Y);

                settingNew.X = positionNew.X;
                settingNew.Y = positionNew.Y;
            }
              
            return settingNew;
        }

        public void ForceNewWaypoint()
        {
            CreateNextWaypoint();
        }

        /// <summary>
        /// calculates new waypoint based mapExplored
        /// </summary>
        /// <returns></returns>
        private void CreateNextWaypoint()
        {
            // TODO implement logic by deciding what the next waypoint is based on _mapExplored

            Random rnd = new Random();

            Point waypointNew = new Point();
            waypointNew.X = rnd.Next(0, 800);
            waypointNew.Y = rnd.Next(0, 600);
            waypointNew.Status = MapElementStatus.Waypoint;

            _mapExplored.WaypointActive = waypointNew;
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
            if (currentDirection > targetDirection)
            {
                currentDirection = currentDirection - 1;
            }
            else
            {
                currentDirection = currentDirection + 1;
            }

            return currentDirection;
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
