using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SearchRobot.Library.Maps.Point;

namespace SearchRobot.Library.Maps
{
    public class MapExplored : IDisposable
    {
        private MapElementStatus[,] _map = new MapElementStatus[800,600];

        private Point _waypointActive;
        public Point WaypointActive
        {
            set
            {
                // disable old waypoint
                if (WaypointExists())
                {
                    // set old waypoint to undiscovered if robot didn't reach it. because it means that robot collided on its way.
                    if (GetStatus(_waypointActive.X, _waypointActive.Y) != MapElementStatus.Visited)
                    {
                        SetStatus(_waypointActive.X, _waypointActive.Y, MapElementStatus.Undiscovered);
                    }
                }

                // set new waypoint
                _waypointActive = value;
                SetStatus(_waypointActive.X, _waypointActive.Y, MapElementStatus.Waypoint);
            } 
            get
            {
                return _waypointActive;
            }
        }

        public void SetStatus(int x, int y, MapElementStatus status)
        {
            _map[x, y] = status;
        }

        public void SetStatus(double x, double y, MapElementStatus status)
        {
            SetStatus((int)x, (int)y, status);
        }
            
        public MapElementStatus GetStatus(int x, int y)
        {
            return _map[x, y];
        }

        public MapElementStatus GetStatus(double x, double y)
        {
            return GetStatus((int)x, (int)y);
        }

        public int GetStartIndex(int dimension)
        {
            return _map.GetLowerBound(dimension);
        }

        public int GetEndIndex(int dimension)
        {
            return _map.GetUpperBound(dimension);
        }

        private bool WaypointExists()
        {
            return _waypointActive != null;
        }

        public void Dispose()
        {
            _map = new MapElementStatus[800, 600];
            _waypointActive = null;
        }
    }
}
