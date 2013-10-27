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
    public class Brain
    {
        private MapExplored _mapExplored;

        public Brain(MapExplored mapExplored) {

            _mapExplored = mapExplored;
        }

        public MovementObject GetNextMove(Point currentPosition, double currentDirection)
        {
            // if waypoint doesn't exist, create one and add to mapExplored
            Point waypoint;
            if (!_mapExplored.WaypointExists())
            {
                waypoint = CalculateNextWaypoint();
                _mapExplored.AddPoint(waypoint);
            }
            else
            {
                waypoint = _mapExplored.GetWaypoint();
            }

            // check if waypoint is reached
            if (GeometryHelper.ComparePoints(currentPosition, waypoint))
            {
                Console.WriteLine("WAYPOINT HIT");
                // change status of old waypoint
                _mapExplored.SetStatus(waypoint, MapElementStatus.Visited);

                waypoint = CalculateNextWaypoint();
                _mapExplored.AddPoint(waypoint);
            }

            MovementObject mo = new MovementObject();
            mo.Position = currentPosition;
            mo.Direction = currentDirection;
            // either change direction or position
            double targetDirection = CalculateTargetDirection(currentPosition, waypoint);
            if (currentDirection != targetDirection)
            {
                mo.Direction = AdjustDirection(currentDirection, targetDirection);
            }
            else
            {
                mo.Position = GetNextMovementPoint(currentPosition);
            }

            return mo;
        }

        /* calculates new waypoint based mapExplored
        /****************************************************************/
        private Point CalculateNextWaypoint()
        {
            // TODO implement logic by deciding what the next waypoint is based on _mapExplored

            Random rnd = new Random();

            Point waypointNew = new Point();
            waypointNew.X = rnd.Next(0, 800);
            waypointNew.Y = rnd.Next(0, 600);
            waypointNew.Status = MapElementStatus.Waypoint;

            Console.WriteLine("waypointNew.X: " + waypointNew.X);
            Console.WriteLine("waypointNew.Y: " + waypointNew.Y);

            return waypointNew;
        }

        /* calculates new movementPoint based on next waypoint
        /****************************************************************/
        private Point GetNextMovementPoint(Point currentPosition)
        {
            Point waypoint = _mapExplored.GetWaypoint();

            double movementX = waypoint.X - currentPosition.X;
            double movementY = waypoint.Y - currentPosition.Y;

            Point movementPoint = new Point();

            // moving to the top of the map not possible atm
            int dirX = movementX >= 0 ? 1 : -1;
            int dirY = movementY >= 0 ? 1 : -1;

            movementX = Math.Abs(movementX);
            movementY = Math.Abs(movementY);

            if (movementX >= movementY)
            {
                movementPoint.X = currentPosition.X + 1 * dirX;
                movementPoint.Y = currentPosition.Y + (1 / movementX * movementY) * dirY;
            }
            else
            {
                movementPoint.Y = currentPosition.Y + 1 * dirY;
                movementPoint.X = currentPosition.X + (1 / movementY * movementX) * dirX;
            }

            return movementPoint;
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

        private double CalculateTargetDirection(Point currentPosition, Point waypoint)
        {
            return Math.Floor(GeometryHelper.GetAngle(currentPosition, waypoint));
        } 
    }
}
