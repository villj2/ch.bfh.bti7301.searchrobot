using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Maps
{
    public class MapExplored
    {
        public List<MapElementStatus> Map { get; set; }

        public void SetStatus(Point point, MapElementStatus status)
        {

        }
            
        public MapElementStatus GetStatus(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
