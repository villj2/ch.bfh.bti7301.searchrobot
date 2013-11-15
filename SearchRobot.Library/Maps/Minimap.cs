﻿using System;
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
        private MapElementStatus[,] _trackingMap = new MapElementStatus[800, 600];

        private int _ticks = 0;

        public Minimap(Canvas minimapArea, MapExplored mapExplored)
        {
            _minimapArea = minimapArea;
            _mapExplored = mapExplored;

            // FIXME just4testing - fill mapExplored with testdata
            for (int i = 0; i < _mapExplored.Map.GetLength(0); i++)
            {
                for (int j = 0; j < _mapExplored.Map.GetLength(1); j++)
                {
                    if (j % 20 == 0) _mapExplored.Map[i, j] = MapElementStatus.Blocked;
                }
            }
        }

        internal void Update()
        {
            // TODO display map explored in minimap

            //Console.WriteLine("Minimap Update");

            //_minimapArea.Width = 800;
            //_minimapArea.Height = 600;

            /*
            var bm = new Bitmap(800, 600);
            bm.SetPixel(33, 100, System.Drawing.Color.Black);
            bm.SetPixel(34, 100, System.Drawing.Color.Black);
            bm.SetPixel(35, 100, System.Drawing.Color.Black);
            bm.SetPixel(36, 100, System.Drawing.Color.Black);
            bm.SetPixel(37, 100, System.Drawing.Color.Black);

            bm.SetPixel(795, 599, System.Drawing.Color.Red);
            bm.SetPixel(796, 599, System.Drawing.Color.Red);
            bm.SetPixel(797, 599, System.Drawing.Color.Red);
            bm.SetPixel(798, 599, System.Drawing.Color.Red);
            bm.SetPixel(799, 599, System.Drawing.Color.Red);
            
              
            var bmi = new BitmapImage();

            //_minimapArea.Children.Add(bmi);

            using (MemoryStream memory = new MemoryStream())
            {
                bm.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bmi.BeginInit();
                bmi.StreamSource = memory;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();
            }

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = bmi;

            _minimapArea.Children.Add(img);
             * 
            */


            // FIXME Performance des Todes :(
            //_minimapArea.Children.Clear();

            ++_ticks;

            for (int i = _mapExplored.Map.GetLowerBound(0); i < _mapExplored.Map.GetUpperBound(0); i++)
            {
                for (int j = _mapExplored.Map.GetLowerBound(1); j < _mapExplored.Map.GetUpperBound(1); j++)
                {
                    if (_mapExplored.Map[i, j] == MapElementStatus.Waypoint)
                    {
                        if (_trackingMap[i, j] != MapElementStatus.Waypoint)
                        {
                            _trackingMap[i, j] = MapElementStatus.Waypoint;
                            drawPoint(i, j, System.Windows.Media.Brushes.DarkRed, 5);
                        }
                    }
                    else if (_mapExplored.Map[i, j] == MapElementStatus.Visited)
                    {
                        if (_trackingMap[i, j] != MapElementStatus.Visited)
                        {
                            _trackingMap[i, j] = MapElementStatus.Visited;
                            drawPoint(i, j, System.Windows.Media.Brushes.DeepPink, 5);
                        }
                    }
                    else if (_mapExplored.Map[i, j] == MapElementStatus.Blocked)
                    {
                        if (_trackingMap[i, j] != MapElementStatus.Blocked)
                        {
                            _trackingMap[i, j] = MapElementStatus.Blocked;
                            drawPoint(i, j, System.Windows.Media.Brushes.Black);
                        }
                    }
                }
            }
        }

        private void drawPoint(int posX, int posY, System.Windows.Media.Brush color)
        {
            drawPoint(posX, posY, color, 1);
        }

        private void drawPoint(int posX, int posY, System.Windows.Media.Brush color, int size)
        {
            UIElement ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = color
            };

            Canvas.SetLeft(ellipse, posX / (800 / _minimapArea.ActualWidth));
            Canvas.SetTop(ellipse, posY / (600 / _minimapArea.ActualHeight));

            _minimapArea.Children.Add(ellipse);
        }

        public void Dispose()
        {
            if(_minimapArea != null) _minimapArea.Children.Clear();
            _minimapArea = null;
            _mapExplored = null;
        }
    }
}
