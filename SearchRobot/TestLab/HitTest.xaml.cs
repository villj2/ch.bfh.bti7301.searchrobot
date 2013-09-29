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

namespace SearchRobot.TestLab
{
    /// <summary>
    /// Interaction logic for HitTest.xaml
    /// </summary>
    public partial class HitTest : Window
    {
        public HitTest()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           /* Rectangle
             */
            TestRect.Width = 200;
            TestRect.Height = 100;
            TestRect.Fill = Brushes.Red;

            Canvas.SetLeft(TestRect, 250);
            Canvas.SetTop(TestRect, 50);

            /* Line
             */
            var line = new Line();
            line.X1 = 10;
            line.X2 = 50;
            line.Y1 = 20;
            line.Y2 = 60;
            line.Fill = Brushes.Blue;
            line.StrokeThickness = 4;
            line.MouseUp += new MouseButtonEventHandler(line_MouseUp);

            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Black;
            line.Stroke = redBrush;

            TestCanvas.Children.Add(line);

            /* Circle
             */
            var ellipse = new Ellipse();
            ellipse.Width = 100;
            ellipse.Height = 100;
            ellipse.Fill = Brushes.Gold;
            Canvas.SetLeft(ellipse, 200);
            Canvas.SetTop(ellipse, 100);
            ellipse.MouseUp += new MouseButtonEventHandler(Ellipse_MouseUp);

            TestCanvas.Children.Add(ellipse);

            
            /* Hit Test
             */
            // FIXME not working
            var p = new Point();
            p.X = 220;
            p.Y = 110;

            var res = VisualTreeHelper.HitTest(ellipse, p);

            if (res != null) MessageBox.Show(res.ToString());
            
        }

        private void line_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("line clicked");
        }

        private void TestRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("click TestRect");
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("click Ellipse");
        }
    }
}
