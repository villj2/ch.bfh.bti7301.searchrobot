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
        private MapExplored _mapExplored;

        // Tracking Map - Same as MapExplored one Cycle before. Update drawn infos in this array. First always check if at specific position change was made.
        // Only if there is a difference draw the new point.
        private TrackingMapEntry[,] _trackingMap = new TrackingMapEntry[800, 600];

        private int _ticks = 0;

        public Minimap(Canvas minimapArea, MapExplored mapExplored)
        {
            _minimapArea = minimapArea;
            _mapExplored = mapExplored;

            // FIXME just4testing - fill mapExplored with testdata
            /*
            for (int i = 0; i < _mapExplored.Map.GetLength(0); i++)
            {
                for (int j = 0; j < _mapExplored.Map.GetLength(1); j++)
                {
                    if (j % 20 == 0) _mapExplored.Map[i, j] = MapElementStatus.Blocked;
                }
            }
            */
        }

        internal void Update()
        {
            // TODO display map explored in minimap


            ++_ticks;

            for (int i = _mapExplored.GetStartIndex(0); i < _mapExplored.GetEndIndex(0); i++)
            {
                for (int j = _mapExplored.GetStartIndex(1); j < _mapExplored.GetEndIndex(1); j++)
                {
                    // create instance of Tracking Map Entry
                    if (_trackingMap[i, j] == null) _trackingMap[i, j] = new TrackingMapEntry();

                    // check if trackingMap differs from MapExplored, only then set new status / point
                    if (_mapExplored.GetStatus(i, j) != _trackingMap[i, j].status)
                    {
                        // remove point before adding new status
                        _minimapArea.Children.Remove(_trackingMap[i, j].point);
                        _trackingMap[i, j].point = null;
                        
                        // draw point dependent of MapElementStatus in MapExplored 
                        switch (_mapExplored.GetStatus(i, j))
                        {
                            case MapElementStatus.Waypoint:
                                _trackingMap[i, j].status = MapElementStatus.Waypoint;
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.DarkRed, 2);
                                break;
                            case MapElementStatus.Visited:
                                _trackingMap[i, j].status = MapElementStatus.Visited;
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.DeepPink, 1);
                                break;
                            case MapElementStatus.Blocked:
                                _trackingMap[i, j].status = MapElementStatus.Blocked;
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Black);
                                break;
                            case MapElementStatus.Collided:
                                _trackingMap[i, j].status = MapElementStatus.Collided;
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Red, 5);
                                break;
                            case MapElementStatus.WaypointVisited:
                                _trackingMap[i, j].status = MapElementStatus.WaypointVisited;
                                _trackingMap[i, j].point = drawPoint(i, j, System.Windows.Media.Brushes.Blue, 5);
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
            UIElement ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color
            };

            Canvas.SetLeft(ellipse, (posX / (800 / _minimapArea.ActualWidth)) - size / 2);
            Canvas.SetTop(ellipse, (posY / (600 / _minimapArea.ActualHeight)) - size / 2);

            _minimapArea.Children.Add(ellipse);

            return ellipse;
        }

        public void Dispose()
        {
            if (_minimapArea != null)
            {
                _minimapArea.Children.Clear();
                _minimapArea = null;
            }
            
            _mapExplored = null;
        }
    }
}
