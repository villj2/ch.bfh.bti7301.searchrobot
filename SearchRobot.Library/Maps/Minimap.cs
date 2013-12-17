using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchRobot.Library.Maps
{
    class Minimap : IDisposable
    {
        private Canvas _minimapArea;
        private Canvas _minimapAreaVisited;
        private MapExplored _mapExplored;

        private Bitmap _bm;

        //public static Minimap MAGIC_MINIMAP;

        // Tracking Map - Same as MapExplored one Cycle before. Update drawn infos in this array. First always check if at specific position change was made.
        // Only if there is a difference draw the new point.
        //private TrackingMapEntry[,] _trackingMap = new TrackingMapEntry[800, 600];
        private MapElementStatus[,] _trackingMap;

        public Minimap(Canvas minimapArea, Canvas minimapAreaVisited, MapExplored mapExplored)
        {
            _minimapArea = minimapArea;
            _minimapAreaVisited = minimapAreaVisited;
            _mapExplored = mapExplored;

            _bm = new Bitmap(800, 600);
            _trackingMap = new MapElementStatus[800, 600];

            //MAGIC_MINIMAP = this;
        }

        internal void Update()
        {
            for (int i = _mapExplored.GetStartIndex(0); i < _mapExplored.GetEndIndex(0); i++)
            {
                for (int j = _mapExplored.GetStartIndex(1); j < _mapExplored.GetEndIndex(1); j++)
                {
                    drawPixel(i, j,_mapExplored.GetStatus(i, j));
                }
            }

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = BitmapConverter.AsImage(_bm).Source;
            _minimapArea.Background = brush;
        }

        private void drawPixel(int x, int y, MapElementStatus status)
        {
            if (_trackingMap[x, y] != status)
            {
                _trackingMap[x, y] = status;

                switch (status)
                {
                    case MapElementStatus.Waypoint:
                        _bm.SetPixel(x, y, System.Drawing.Color.DarkRed);
                        break;
                    case MapElementStatus.Visited:
                        _bm.SetPixel(x, y, System.Drawing.Color.DeepPink);
                        break;
                    case MapElementStatus.Blocked:
                        _bm.SetPixel(x, y, System.Drawing.Color.Black);
                        break;
                    case MapElementStatus.Collided:
                        _bm.SetPixel(x, y, System.Drawing.Color.Red);
                        break;
                    case MapElementStatus.WaypointVisited:
                        _bm.SetPixel(x, y, System.Drawing.Color.Blue);
                        break;
                    case MapElementStatus.Discovered:
                        _bm.SetPixel(x, y, System.Drawing.Color.SkyBlue);
                        break;
                    //case MapElementStatus.Target:
                    //    _bm.SetPixel(i, j, System.Drawing.Color.Blue);
                    //    break;
                }
            }
        }

        internal void drawWaypoints(List<Point> waypoints)
        {
            foreach (Point p in waypoints)
            {
                //drawPoint(p.X, p.Y, System.Windows.Media.Brushes.DarkRed, 5);
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Point omegaP = new Point(p.X + i, p.Y + j);

                        if (omegaP.InBound())
                        {
                            _bm.SetPixel(omegaP.X, omegaP.Y, System.Drawing.Color.DarkRed);
                        }
                    }
                }
                
            }

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = BitmapConverter.AsImage(_bm).Source;
            _minimapArea.Background = brush;
        }

        public void Dispose()
        {
            if (_minimapArea != null)
            {
                _minimapArea.Children.Clear();
                _minimapArea = null;
            }

            if (_minimapAreaVisited != null)
            {
                _minimapAreaVisited.Children.Clear();
                _minimapAreaVisited = null;
            }

            _mapExplored = null;
            _trackingMap = null;
        }
    }
}
