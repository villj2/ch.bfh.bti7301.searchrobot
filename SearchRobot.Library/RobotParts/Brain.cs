using System.Diagnostics;
using SearchRobot.Library.Maps;
using SearchRobot.Library.Simulation;
using SearchRobot.Library.Simulation.Dijkstra;
using SearchRobot.Library.Simulation.EdgeDetection;
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
        private int _iterationCounter = 0;
        private const int ReconsiderCounterThresholdd = 50;

        public readonly Queue<Point> _waypointQueue = new Queue<Point>();

        // FIXME just4testing
        public List<Point> waypoints;
        private int waypointindex = 0;

        public MapExplored MapExplored { get { return _mapExplored; } }
        private readonly MapExplored _mapExplored;
        private Robot _robot;
	    private int _scanningIteration;
		private const int ScanningIterationThreshold = 50;

	    public Brain(MapExplored mapExplored, Robot robot)
		{
            _mapExplored = mapExplored;
            _robot = robot;

            // set first waypoint
            // CreateNextWaypoint();
        }

		private bool IsPointingAtTarget(double currentDirection, double targetDirection)
		{
			return Math.Abs((currentDirection + 360)%360 - (targetDirection + 360)%360) < 0.25;
		}

		private bool HasReachedWayPoint(double posX, double posY)
		{
			return GeometryHelper.ComparePointsWithRange(posX, posY, _mapExplored.WaypointActive.X, _mapExplored.WaypointActive.Y, 5);
		}

		private void CalculateNextTarget()
		{
			_waypointQueue.Clear();
			List<Point> route = null;
			var DijkstraHelper = new DijkstraHelper(_mapExplored);

			var goal = GetGoalPoint();
			if (goal != null)
			{
				route = DijkstraHelper.GetPath(_robot.StartPosition, goal);
			}

			if (route == null || route.Any())
			{
				_waypointQueue.Clear();
				EdgeDetectionAlgorithm edgeDetection = new EdgeDetectionAlgorithm();

				var points = edgeDetection.GetEdgePoints(_mapExplored.Map);
				var edges = edgeDetection.GroupToEdges(points).OrderByDescending(edge => edge.Width).ToList();

				foreach (var edge in edges)
				{
					var path = DijkstraHelper.GetPath(_robot.StartPosition, edge.CenterPoint);
					if (path != null && path.Any())
					{
						route = path;
						break;
					}
				}
			}

			if (route != null && route.Any())
			{
				route.ForEach(wp => _waypointQueue.Enqueue(wp));
			}
			else
			{
				throw new ApplicationException("No further valid edges or target found!");
			}
		}

		private void AllowToRescan()
		{
			if (_scanningIteration++ % ScanningIterationThreshold == 0)
			{
                var scannedMap = _robot.GetView();

				DebugHelper.StoreAsBitmap(string.Format("C:\\debugimage-{0}.png", DateTime.Now.Ticks), scannedMap);
				_mapExplored.UpdateSensordata(scannedMap.ToArray(), _robot.StartPosition);
			}
		}

		private Point GetGoalPoint()
		{
			for (var x = 0; x <= _mapExplored.Map.GetUpperBound(0); x++)
			{
				for (var y = 0; y <= _mapExplored.Map.GetUpperBound(1); y++)
				{
					if (_mapExplored.Map[x, y] == MapElementStatus.Target)
					{
						return new Point(x, y);
					}
				}
			}

			return null;
		}

	    private Point ActiveWayPoint
	    {
		    get { return _mapExplored.WaypointActive; }
			set { _mapExplored.WaypointActive = value; }
	    }

		public MovementObject GetNextMove(double posX, double posY, double currentDirection)
		{
			AllowToRescan();

			// check if waypoint is reached
			if (ActiveWayPoint == null || HasReachedWayPoint(posX, posY))
			{
				WayDecision.IgnoreDirection = false;

				if (!_waypointQueue.Any())
				{
					CalculateNextTarget();
				}

				ActiveWayPoint = _waypointQueue.Dequeue();
			}

			MovementObject settingNew = new MovementObject(posX, posY, currentDirection);

			double targetDirection = CalculateTargetDirection(posX, posY, _mapExplored.WaypointActive);

			// either change direction or position
			if (IsPointingAtTarget(currentDirection, targetDirection)  || WayDecision.IgnoreDirection)
			{
				MovementObject positionNew = GetNextMovementPoint(posX, posY);
				settingNew.X = positionNew.X;
				settingNew.Y = positionNew.Y;
			}
			else
			{
				settingNew.Direction = AdjustDirection(currentDirection, targetDirection);
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
			// Only Handle Collision WayDecision if Robot is not driving backwards (which means that Robot already hat Collision)

			_waypointQueue.Clear();
			WayDecision wd;
			
			if (!WayDecision.IgnoreDirection)
			{
				wd = new WayDecisionCollision(posX, posY, mo, _mapExplored);
				WayDecision.IgnoreDirection = true;
			}
			else
			{
				wd = new WayDecisionCollisionBackwards(posX, posY, mo, _mapExplored);
				WayDecision.IgnoreDirection = false;
			}

			// CalculateNextTarget();
			// _mapExplored.WaypointActive = _waypointQueue.Dequeue();

			CreateNextWaypoint(wd);
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
			_waypointQueue.Clear();
            ActiveWayPoint = wayDecision.GetWaypoint();
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
            if(currentDirection < 0) currentDirection += 360;
            if(targetDirection < 0) targetDirection += 360;

            double discrepancy = Math.Abs(targetDirection - currentDirection);

            bool dir = ((currentDirection) - (targetDirection) + 360) % 360 > 180;
            if (dir)
            {
                currentDirection = currentDirection + (discrepancy > 1 ? 2 : 1);
            }
            else
            {
                currentDirection = currentDirection - (discrepancy > 1 ? 2 : 1);
            }

            return currentDirection % 360;
        }

        private double CalculateTargetDirection(double posX, double posY, Point waypoint)
        {
            return Math.Floor(GeometryHelper.GetAngle(posX, posY, waypoint.X, waypoint.Y));
        }

        public void Dispose()
        {
            WayDecision.Collisions = 0;
            _mapExplored.Dispose();
        }
    }
}
