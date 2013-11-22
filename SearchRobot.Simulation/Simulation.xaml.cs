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
using SearchRobot.Library.Simulation;
using SearchRobot.Library.Global;

namespace SearchRobot.Simulation
{
    /// <summary>
    /// Interaction logic for Simulation.xaml
    /// </summary>
    public partial class Simulation : Window
    {
        private SimulationEngine _simulationEngine { get; set; }

        public Simulation()
        {
            InitializeComponent();
            InitializeText();

            _simulationEngine = new SimulationEngine(MapArea, MinimapArea);
        }

        private void InitializeText()
        {
            btnLoad.Content = TextContent.Instance["Simulation-Button-Load"];
            btnStart.Content = TextContent.Instance["Simulation-Button-Start"];
            btnReset.Content = TextContent.Instance["Simulation-Button-Reset"];
            btnAnalyze.Content = TextContent.Instance["Simulation-Button-Analyze"];
        }

        private void OnBtnLoadClick(object sender, RoutedEventArgs e)
        {
            _simulationEngine.LoadMap();
            InitializeText();
        }

        private void OnBtnStartClick(object sender, RoutedEventArgs e)
        {
            btnStart.Content = _simulationEngine.Toggle();
        }

        private void OnBtnResetClick(object sender, RoutedEventArgs e)
        {
            _simulationEngine.Reset();
            InitializeText();
        }

        private void OnBtnAnalyzeClick(object sender, RoutedEventArgs e)
        {
            _simulationEngine.AnalyzeMap();
        }
    }
}
