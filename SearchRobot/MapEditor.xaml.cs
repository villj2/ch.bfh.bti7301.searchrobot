using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SearchRobot.Library.Maps;
using Point = System.Windows.Point;

namespace SearchRobot
{
    /// <summary>
    /// Interaction logic for MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {
        private static Library.Maps.Point ToPoint(Point point)
        {
            return new Library.Maps.Point() {X = Convert.ToInt32(point.X), Y = Convert.ToInt32(point.Y)};
        }

        private Map Map { get; set; }

        public MapEditor()
        {
            InitializeComponent();
            ToggleSelection(Tools.Wall, true);
            
            Map = new Map();
        }

        private MapElement _currentElement;

        #region Canvas MouseHandling
        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            _currentElement = GetNewActiveTool();
            if (_currentElement != null)
            {
                _currentElement.MouseDown(MapArea, ToPoint(e.GetPosition(MapArea)));
            }
        }


        private void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentElement != null)
            {
                _currentElement.MouseUp(MapArea, ToPoint(e.GetPosition(MapArea)));
                _currentElement = null;
            }
        }


        private void OnCanvasMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            /*lblOutput.Content = string.Format(
                "X:{0},Y:{1}",
                mouseEventArgs.GetPosition(MapArea).X,
                mouseEventArgs.GetPosition(MapArea).Y);*/

            if (_currentElement != null)
            {
                _currentElement.MouseMove(MapArea, ToPoint(mouseEventArgs.GetPosition(MapArea)));
            }
        }
        #endregion

        #region Tool Selection
        private enum Tools { Wall, Disc, Goal, Robot, Remove}
        private Tools ActiveTool { get; set; }

        private void ToggleSelection(Tools option, bool flag)
        {
            if (flag)
            {
                ToggleSelection(ActiveTool, false);
                ActiveTool = option;
            }

            switch (option)
            {
                case Tools.Robot:
                    btnRobot.IsEnabled = !flag;
                    break;
                case Tools.Disc:
                    btnDisc.IsEnabled = !flag;
                    break;
                case Tools.Wall:
                    btnWall.IsEnabled = !flag;
                    break;
                case Tools.Goal:
                    btnGoal.IsEnabled = !flag;
                    break;
                case Tools.Remove:
                    btnRemove.IsEnabled = !flag;
                    break;
            }
        }

        private MapElement GetNewActiveTool()
        {
            switch (ActiveTool)
            {
                case Tools.Wall:
                    return new Wall(Map);
                case Tools.Disc:
                    return new Disc(Map);
                case Tools.Robot:
                    return new Robot(Map, lblOutput);
                case Tools.Goal:
                    return new Goal(Map);
                default:
                    return null;
            }
        }

        private void OnWallSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Wall, true);
        }

        private void OnDiscSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Disc, true);
        }

        private void OnGoalSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Goal, true);
        }

        private void OnRobotSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Robot, true);
        }

        private void OnRemoveSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Remove, true);
        }

        #endregion

    }
}
