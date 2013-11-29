using SearchRobot.Library.Maps;
using SearchRobot.Library.RobotParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Simulation
{
    public class MovementObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Direction { get; set; }

        public MovementObject(double x, double y, double direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public MovementObject()
        {}
    }
}
