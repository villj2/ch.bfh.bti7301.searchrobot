using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;
using System.Windows;
using System.Windows.Controls;
using SearchRobot.Library.Global;
using SearchRobot.Library.RobotParts;
using SearchRobot.Library.Maps;
using Point = SearchRobot.Library.Maps.Point;
using System.Windows.Threading;


namespace SearchRobot.Library.Simulation
{
    public class SimulationEngine
    {
        const int CYCLE_INTERVAL = 10; // milliseconds

        private AutoResetEvent _autoEvent;
        private int _ticks;
        private Canvas _mapArea;
        private DispatcherTimer _dispatcherTimer;

        private Robot _robot;
        private Map _map;

        public SimulationEngine(Canvas mapArea)
        {
            _mapArea = mapArea;

            initialize();
            loadMap();
            buildMap();
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

        private void loadMap()
        {
            // TODO implementation
        }

        private void buildMap()
        {
            // TODO load Map
            _map = new Map();

            // just 4 testing without map-loading
            _robot = new Robot(this, _map);
            _robot.ApplyTo(_mapArea);

            Point p = new Point();
            p.X = 200;
            p.Y = 300;

            _robot.SetPos(p);
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
            CyclesReset();
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
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _ticks++;
            Console.WriteLine("timer tick, ticks: " + _robot);

            _robot.Move();
        }

        public void ExecuteCycleTick(Object stateInfo)
        {
            _ticks++;

            _robot.Move();
        }

        private void CyclesStop()
        {
            _state = CycleState.Paused;

            _dispatcherTimer.Stop();
        }

        private void CyclesReset()
        {
            if (_state == CycleState.Paused || _state == CycleState.Running)
            {
                _ticks = 0;
                _state = CycleState.Initiated;

                Console.WriteLine("Reset, ticks: {0}", _ticks.ToString());

                // TODO reset Map Explored
                // TODO reset Robot Position
            }
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
    }
}
