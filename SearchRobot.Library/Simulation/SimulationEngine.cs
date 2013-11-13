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
        const int CYCLE_INTERVAL = 10; // milliseconds
        const int CYCLE_MINIMAP_UPDATE = 100; // minimap updates every 100th time interval is dispatched 

        private AutoResetEvent _autoEvent;
        private int _ticks;
        private Canvas _mapArea;
        private Canvas _minimapArea;
        private DispatcherTimer _dispatcherTimer;

        private readonly Sight sight = new Sight() {Angle = 90, Reach = int.MaxValue};

        private Robot _robot;
        private Map _map;
        private Minimap _minimap;

        private string _filename;

        public SimulationEngine(Canvas mapArea, Canvas minimapArea)
        {
            _mapArea = mapArea;
            _minimapArea = minimapArea;

            initialize();
            LoadMap();
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
                _robot.initialize();
                //_robot.Bind(_map);

                // bind all Elements to map
                
                foreach (MapElement me in _map.Elements)
                {
                    me.Bind(_map);
                }

                _minimap = new Minimap(_minimapArea, _robot.MapExplored);
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
        #endregion

        #region Cycle Handling
        private void CyclesStart()
        {
            _state = CycleState.Running;
            _dispatcherTimer.Start();

			Sensor mySensor = new Sensor(_robot, _mapArea, sight);
        }

        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            _ticks++;
            _robot.Move();
            

            if(_ticks % CYCLE_MINIMAP_UPDATE == 1) _minimap.Update();
        }

        private void CyclesStop()
        {
            _state = CycleState.Paused;
            _dispatcherTimer.Stop();
        }
        #endregion

        #region Robot Handling
        public Point GetRobotPosition()
        {
            throw new NotImplementedException();
        }
        
        public int GetRobotOrientation()
        {
            throw new NotImplementedException();
        }
            
        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Turn(int deg)
        {

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
    }
}
