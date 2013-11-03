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
        //private List<Point> _map = new List<Point>();
        public MapElementStatus[,] Map { get { return _map; } }
        private MapElementStatus[,] _map = new MapElementStatus[800,600];

        private Point _waypointActive;
        public Point WaypointActive
        {
            set
            {
                // disable old waypoint
                if(WaypointExists()) SetStatus(_waypointActive.X, _waypointActive.Y, MapElementStatus.Visited);

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
            
        public MapElementStatus GetStatus(Point point)
        {
            throw new NotImplementedException();
        }

        public bool WaypointExists()
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
