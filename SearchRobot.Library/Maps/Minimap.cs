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

        // Tracking Map - Same as MapExplored one Cycle before. Update drawn infos in this array. First always check if at specific position change was made.
        // Only if there is a difference draw the new point.
        //private TrackingMapEntry[,] _trackingMap = new TrackingMapEntry[800, 600];
        private MapElementStatus[,] _trackingMap;

        private int _ticks = 0;

        public Minimap(Canvas minimapArea, Canvas minimapAreaVisited, MapExplored mapExplored)
        {
            _minimapArea = minimapArea;
            _minimapAreaVisited = minimapAreaVisited;
            _mapExplored = mapExplored;

            _bm = new Bitmap(800, 600);
            _trackingMap = new MapElementStatus[800, 600];
        }

        internal void Update()
        {
            ++_ticks;
            
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


            /*
            for (int i = _mapExplored.GetStartIndex(0); i < _mapExplored.GetEndIndex(0); i++)
            //for (int i = 0; i < 800; i++)
            {
                for (int j = _mapExplored.GetStartIndex(1); j < _mapExplored.GetEndIndex(1); j++)
                //for (int j = 0; j < 600; j++)
                {
                    // create instance of Tracking Map Entry
                    if (_trackingMap[i, j] == null) _trackingMap[i, j] = new TrackingMapEntry();

                    // check if trackingMap differs from MapExplored, only then set new status / point
                    if (_mapExplored.GetStatus(i, j) != _trackingMap[i, j].status)
                    {
                        // remove point before adding new status
                        //_minimapArea.Children.Remove(_trackingMap[i, j].point);
                        //_trackingMap[i, j].point = null;

                        // draw point dependent of MapElementStatus in MapExplored and update status in trackingMap
                        MapElementStatus newStatus = _mapExplored.GetStatus(i, j);
                        _trackingMap[i, j].status = newStatus;

                        if (newStatus == MapElementStatus.Remove)
                        {
                            _minimapArea.Children.Remove(_trackingMap[i, j].point);
                            _trackingMap[i, j].point = null;
                            _trackingMap[i, j].status = MapElementStatus.Undiscovered;
                        }

                        switch (newStatus)
                        {
                            //case MapElementStatus.BlockedShadowed:
                            //    _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Black, 1);
                            //    break;
                            case MapElementStatus.Waypoint:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.DarkRed, 5);
                                break;
                            case MapElementStatus.Visited:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.DeepPink, 1, _minimapAreaVisited);
                                break;
                            case MapElementStatus.Blocked:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Black);
                                break;
                            case MapElementStatus.Collided:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Red, 5);
                                break;
                            case MapElementStatus.WaypointVisited:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Blue, 5);
                                break;
                            //case MapElementStatus.Discovered:
                            //    _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Azure, 1);
                            //    break;
                        }
                    }
                }
            }
            */
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
                        _bm.SetPixel(x, y, System.Drawing.Color.Blue);
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
                _bm.SetPixel(p.X, p.Y, System.Drawing.Color.DarkRed);
            }

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = BitmapConverter.AsImage(_bm).Source;
            _minimapArea.Background = brush;
        }

        //private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color)
        //{
        //    return drawPoint(posX, posY, color, 1);
        //}

        //private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color, int size)
        //{
        //    return drawPoint(posX, posY, color, size, _minimapArea);
        //}

        //private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color, int size, Canvas canvas)
        //{
        //    UIElement ellipse = new Ellipse
        //    {
        //        Width = size,
        //        Height = size,
        //        Fill = color
        //    };

        //    Canvas.SetLeft(ellipse, (int)((posX / (800 / _minimapArea.ActualWidth)) - size / 2));
        //    Canvas.SetTop(ellipse, (int)((posY / (600 / _minimapArea.ActualHeight)) - size / 2));

        //    //if(zIndex != 1) Canvas.SetZIndex(ellipse, zIndex);

        //    canvas.Children.Add(ellipse);

        //    return ellipse;
        //}

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
