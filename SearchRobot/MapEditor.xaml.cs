using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Win32;
using SearchRobot.Library;
using SearchRobot.Library.Maps;
using SearchRobot.Library.Robot;
using Point = System.Windows.Point;

namespace SearchRobot
{
    /// <summary>
    /// Interaction logic for MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {
        private Map Map { get; set; }

        public MapEditor()
        {
            InitializeComponent();
            ToggleSelection(Tools.Wall, true);
            
            Map = new Map();
        }

        private ICanvasListener _currentListener;

        #region Canvas MouseHandling
        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            _currentListener = GetNewActiveTool();
            if (_currentListener != null)
            {
                _currentListener.MouseDown(MapArea, GeometryHelper.Convert(e.GetPosition(MapArea)));
            }
        }


        private void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentListener != null)
            {
                _currentListener.MouseUp(MapArea, GeometryHelper.Convert(e.GetPosition(MapArea)));
                _currentListener = null;
            }
        }


        private void OnCanvasMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_currentListener != null)
            {
                _currentListener.MouseMove(MapArea, GeometryHelper.Convert(mouseEventArgs.GetPosition(MapArea)));
            }
        }


        private void OnCanvasMouseLeave(object sender, MouseEventArgs e)
        {
            if (_currentListener != null)
            {
                _currentListener.Remove(MapArea);
            }
        }
        #endregion

        #region Tool Selection
        private enum Tools { Wall, Disc, Goal, Robot, Remove, Move}
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

        private ICanvasListener GetNewActiveTool()
        {
            switch (ActiveTool)
            {
                case Tools.Wall:
                    return new Wall(Map);
                case Tools.Disc:
                    return new Disc(Map);
                case Tools.Robot:
                    return new Robot(Map);
                case Tools.Goal:
                    return new Goal(Map);
                case Tools.Remove:
                    return new RemoveTool(Map);
                case Tools.Move:
                    return new RemoveTool(Map);
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

        private void OnMoveSelectionClick(object sender, RoutedEventArgs e)
        {
            ToggleSelection(Tools.Move, true);
        }
        #endregion

		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			var fileDialog = new SaveFileDialog
			                     {
			                         AddExtension = true,
                                     DefaultExt = ".xml",
                                     Filter = "Map Files|*.xml"
			                     };
		    fileDialog.FileOk += FileDialogOnFileOk;

			fileDialog.ShowDialog();
		}

		private void FileDialogOnFileOk(object sender, CancelEventArgs cancelEventArgs)
		{
			var dialog = sender as SaveFileDialog;
            if (dialog != null)
            {
                var filename = dialog.FileName;
                Resolver.StorageManager.Save(filename, Map);
            }
		}

		private void OnLoadClick(object sender, RoutedEventArgs e)
		{
			var fileDialog = new OpenFileDialog
			                     {
			                         Filter = "Map Files|*.xml",
                                     Multiselect = false
			                     };
		    fileDialog.FileOk += LoadMapFromFile;

			fileDialog.ShowDialog();
		}

		void LoadMapFromFile(object sender, CancelEventArgs e)
		{
			var dialog = sender as OpenFileDialog;
            if (dialog != null)
            {
                var filename = dialog.FileName;

                Map = Resolver.StorageManager.Load(filename);
                MapArea.Children.Clear();
                Map.ApplyToCanvas(MapArea);
            }
		}

        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            Map = new Map();
            MapArea.Children.Clear();
        }
    }
}
