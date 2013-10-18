using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SearchRobot.Library.Maps.Point;

namespace SearchRobot.Library.Maps
{
    public class MapExplored
    {
        private List<Point> _map = new List<Point>();

        public void AddPoint(Point point)
        {
            _map.Add(point);
        }

        public void SetStatus(String point, MapElementStatus status)
        {
            // check if point exists (with lambda expression)
            // if exists (point.x = ... & point.y = ...) then update the status of that point
            // otherwise throw error "point doesn't exist"
        }
            
        public MapElementStatus GetStatus(Point point)
        {
            throw new NotImplementedException();
        }

        private Point GetWaypoint()
        {
            return _map.Where(x => x.Status == MapElementStatus.Waypoint).FirstOrDefault();
        }
    }
}
