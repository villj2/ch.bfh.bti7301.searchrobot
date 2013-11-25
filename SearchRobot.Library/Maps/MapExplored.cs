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
                _waypointLast = _waypointActive;
                _waypointActive = value;
                SetStatus(_waypointActive.X, _waypointActive.Y, MapElementStatus.Waypoint);
            } 
            get
            {
                return _waypointActive;
            }
        }
        private Point _waypointLast;

        public void SetStatus(int x, int y, MapElementStatus status)
        {
            if(_map[x, y] != status) _map[x, y] = status;
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

        public void UpdateSensordata(MapElementStatus[,] arrMap, Point posRobot)
        //public void UpdateSensordata(CartesianArray<MapElementStatus> arrCartesian, Point posRobot)
        {
            // Methode 1: Vom CartesianArray ausgehend
            /*
            int offsetLeft = -posRobot.X;
            int offsetRight = 800 - posRobot.X;
            int offsetTop = -posRobot.Y;
            int offsetBottom = 600 - posRobot.Y;

            for (int i = offsetLeft; i < offsetRight; i++)
            {
                for (int j = offsetTop; j < offsetBottom; j++)
                {
                    // nur überschreiben wenn Undiscovered
                    if (_map[posRobot.X + i, posRobot.Y + j] == MapElementStatus.Undiscovered)
                    {
                        _map[posRobot.X + i, posRobot.Y + j] = arrCartesian[i, j];
                    }
                }
            }
            */
            // Methode 2: CartesianArray Croppen!
            // FIXME arrMap teilweise kleiner als 800x600! Wie ist das möglich? -> Tritt auf wenn Roboter Direction = 180. Sprich nach links schauen.
            
            int widthSensorMap = arrMap.GetLength(0);
            int heightSensorMap = arrMap.GetLength(1);

            int offsetLeft = (widthSensorMap - 800) / 2;
            int offsetTop = (heightSensorMap - 600) / 2;

            for (int i = offsetLeft; i < 800 + offsetLeft; i++)
            {
                for (int j = offsetTop; j < 600 + offsetTop; j++)
                {
                    // nur überschreiben wenn Undiscovered
                    if (_map[i - offsetLeft, j - offsetTop] == MapElementStatus.Undiscovered)
                    {
                        MapElementStatus status = offsetLeft < 0 || offsetTop < 0 ? MapElementStatus.Undiscovered : arrMap[i, j];
                        _map[i - offsetLeft, j - offsetTop] = status;
                    }
                }
            }
        }

        private bool WaypointExists()
        {
            return _waypointActive != null;
        }

        public bool PathAvailable(Point target, Point positionRobot)
        {
            double shiftX = target.X - positionRobot.X;
            double shiftY = target.Y - positionRobot.Y;

            int dirX = shiftX > 0 ? 1 : -1;
            int dirY = shiftY > 0 ? 1 : -1;

            shiftX = Math.Abs(shiftX);
            shiftY = Math.Abs(shiftY);

            double stepX;
            double stepY;

            int loopCount = 0;

            if (shiftX >= shiftY)
            {
                loopCount = (int)shiftX;
                stepX = 1 * dirX;
                stepY = (1 / shiftX * shiftY) * dirY;
            }
            else
            {
                loopCount = (int)shiftY;
                stepX = (1 / shiftY * shiftX) * dirX;
                stepY = 1 * dirY;
            }

            bool wayClear = true;

            // step through array to goal (direct line) and check if there is a collision on its way
            /*
             * R x 0 0 0 0 0 0 0
             * 0 0 x x 0 0 0 0 0
             * 0 0 0 0 x x 0 0 0
             * 0 0 0 0 0 0 x x 0
             * 0 0 0 0 0 0 0 0 T
             * 
             * x = path
             */

            double x = positionRobot.X;
            double y = positionRobot.Y;
            for (int i = 0; i < loopCount; i++)
            {
                x += stepX;
                y += stepY;

                MapElementStatus status = GetStatus((int)x, (int)y);
                if (status == MapElementStatus.Blocked || status == MapElementStatus.BlockedShadowed)
                {
                    wayClear = false;
                    break;
                }
            }


            return wayClear;
        }

        public void Dispose()
        {
            _map = new MapElementStatus[800, 600];
            _waypointActive = null;
        }
    }
}