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

        public void SetStatus(Point point, MapElementStatus status)
        {
            // check if point exists at position x/y
            // true: update the status of that point
            // false: write out "Point doesn't exist"

            Point pointMapExplored = _map.Find(p => p.X == point.X && p.Y == point.Y);

            if (pointMapExplored != null)
            {
                pointMapExplored.Status = status;
            }
            else
            {
                Console.WriteLine("Point doesn't exist");
            }
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
