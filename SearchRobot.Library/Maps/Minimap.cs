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

        // Tracking Map - Same as MapExplored one Cycle before. Update drawn infos in this array. First always check if at specific position change was made.
        // Only if there is a difference draw the new point.
        private TrackingMapEntry[,] _trackingMap = new TrackingMapEntry[800, 600];

        private int _ticks = 0;

        public Minimap(Canvas minimapArea, Canvas minimapAreaVisited, MapExplored mapExplored)
        {
            _minimapArea = minimapArea;
            _minimapAreaVisited = minimapAreaVisited;
            _mapExplored = mapExplored;
        }

        internal void Update()
        {
            ++_ticks;

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

                        switch (newStatus)
                        {
                            case MapElementStatus.BlockedShadowed:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Black, 1);
                                break;
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
                            case MapElementStatus.Discovered:
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Azure, 1);
                                break;
                        }
                    }
                }
            }
        }

        private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color)
        {
            return drawPoint(posX, posY, color, 1);
        }

        private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color, int size)
        {
            return drawPoint(posX, posY, color, size, _minimapArea);
        }

        private UIElement drawPoint(int posX, int posY, System.Windows.Media.Brush color, int size, Canvas canvas)
        {
            UIElement ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color
            };

            Canvas.SetLeft(ellipse, (int)((posX / (800 / _minimapArea.ActualWidth)) - size / 2));
            Canvas.SetTop(ellipse, (int)((posY / (600 / _minimapArea.ActualHeight)) - size / 2));

            //if(zIndex != 1) Canvas.SetZIndex(ellipse, zIndex);

            canvas.Children.Add(ellipse);

            return ellipse;
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
        }
    }
}
