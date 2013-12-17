using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;
using Timer = System.Threading.Timer;
using System.Windows;
using System.Windows.Controls;
using SearchRobot.Library.Global;
using SearchRobot.Library.RobotParts;
using SearchRobot.Library.Maps;
using Point = SearchRobot.Library.Maps.Point;
using System.Windows.Threading;
using Microsoft.Win32;
using System.ComponentModel;

namespace SearchRobot.Library.Simulation
{
    public class SimulationEngine
    {
        const int CYCLE_INTERVAL = 2; // milliseconds
        const int CYCLE_MINIMAP_UPDATE = 100; // minimap updates every 'x'th time interval is dispatched 

        private AutoResetEvent _autoEvent;
        private int _ticks;
        private readonly Canvas _mapArea;
        private readonly Canvas _minimapArea;
        private readonly Canvas _minimapAreaVisited;
        private static Label _lblOutput;
        private DispatcherTimer _dispatcherTimer;

        private readonly Sight sight = new Sight() {Angle = 90, Reach = int.MaxValue};

        private Robot _robot;
        private Map _map;
        private Minimap _minimap;

        private string _filename;

        public static SimulationEngine SimulationEngineStatic;

        public SimulationEngine(Canvas mapArea, Canvas minimapArea, Canvas minimapAreaVisited, Label lblOutput)
        {
            _mapArea = mapArea;
            _minimapArea = minimapArea;
            _minimapAreaVisited = minimapAreaVisited;
            _lblOutput = lblOutput;

            initialize();
            LoadMap();

            SimulationEngineStatic = this;
        }

        private void initialize()
        {
            _autoEvent = new AutoResetEvent(false);
            _ticks = 0;
            _state = CycleState.Initiated;

            // create Timer
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimerTick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, CYCLE_INTERVAL);
        }

        private void buildMap()
        {
            // get reference of robot
            if (_map != null)
            {
                _robot = _map.Elements.OfType<Robot>().First();
                _robot.Initialize(_mapArea, new Sensor(_robot, _mapArea, new Sight { Angle = 180, Reach = int.MaxValue }));

                _minimap = new Minimap(_minimapArea, _minimapAreaVisited, _robot.MapExplored);
            }
        }

        public void LoadMap()
        {
            Dispose();

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
                _filename = dialog.FileName;

                if (_filename != null)
                {
                    Reset();
                }
            }
        }

        #region Canvas MouseHandling

        private enum CycleState { Initiated, Running, Paused }

        private CycleState _state { get; set; }

        public String Toggle()
        {
            String text = "";

            switch (_state)
            {
                case CycleState.Initiated:
                    CyclesStart();
                    text = TextContent.Instance["Simulation-Button-Pause"];
                    break;
                case CycleState.Running:
                    CyclesStop();
                    text = TextContent.Instance["Simulation-Button-Resume"];
                    break;
                case CycleState.Paused:
                    CyclesStart();
                    text = TextContent.Instance["Simulation-Button-Pause"];
                    break;
            }

            return text;
        }

        public void Reset()
        {
            Dispose();

            if (_filename != null)
            {
                _map = Resolver.StorageManager.Load(_filename);
                _mapArea.Children.Clear();
                _map.ApplyToCanvas(_mapArea);

                buildMap();
            }
        }

        public void AnalyzeMap()
        {
        }
        #endregion

        #region Cycle Handling

        private void CyclesStart()
        {
            _state = CycleState.Running;
            _dispatcherTimer.Start();
        }

        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            _ticks++;
            _robot.Move();

            if(_ticks % CYCLE_MINIMAP_UPDATE == 1)
            {
				_minimap.Update();
				_minimap.drawWaypoints(_robot.GetWayPoints());
            }
        }

        public void CyclesStop()
        {
            _state = CycleState.Paused;
            _dispatcherTimer.Stop();
        }
        #endregion

        #region Robot Handling
        public Point GetRobotPosition()
        {
            return _robot.StartPosition;
        }
        
        public int GetRobotOrientation()
        {
            return (int)_robot.Direction;
        }
        #endregion

        public void Dispose()
        {
            CyclesStop();
            _ticks = 0;

            _map = null;
            _mapArea.Children.Clear();
            if (_robot != null) _robot.Dispose();
            if (_minimap != null) _minimap.Dispose();
            
            _state = CycleState.Initiated;
        }

        public static void EndSimulation(string info)
        {
            _lblOutput.Content = info;
            SimulationEngineStatic.CyclesStop();
        }
    }
}
