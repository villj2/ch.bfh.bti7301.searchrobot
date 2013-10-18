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


namespace SearchRobot.Library.Simulation
{
    public class SimulationEngine
    {
        const int CYCLE_INTERVAL = 500;

        private AutoResetEvent _autoEvent;
        private Timer _timer;
        private int _ticks;

        public SimulationEngine()
        {
            _autoEvent = new AutoResetEvent(false);
            _ticks = 0;
            _state = CycleState.Initiated;
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
            TimerCallback timerDelegate = new TimerCallback(CheckStatus);
            _timer = new Timer(timerDelegate, _autoEvent, CYCLE_INTERVAL, CYCLE_INTERVAL);

            _state = CycleState.Running;
        }

        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            _ticks++;
            Console.WriteLine("{0} ticks", _ticks.ToString());
        }

        private void CyclesStop()
        {
            _timer.Dispose();

            _state = CycleState.Paused;
        }

        private void CyclesReset()
        {
            if (_state == CycleState.Paused || _state == CycleState.Running)
            {
                _timer.Dispose();
                _timer = null;
                _ticks = 0;

                _state = CycleState.Initiated;

                Console.WriteLine("Reset, ticks: {0}", _ticks.ToString());

                // TODO reset Map Explored
                // TODO reset Robot Position
            }
        }
        #endregion
    }
}
