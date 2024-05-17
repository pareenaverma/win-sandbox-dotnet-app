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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TSP.WPF.Model;

namespace TSP.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double _radius = 5.0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TspParams(this);

        }

        internal void DrawPath(List<System.Drawing.Point> path)
        {
            PathCanvas.Children.Clear();
            SolidColorBrush ellipseFillBrush = new SolidColorBrush(Colors.DarkOrange);
            // Draw all the points
            foreach (System.Drawing.Point p in path)
            {
                Ellipse e = new Ellipse
                {
                    Height = _radius * 2,
                    Width = _radius * 2,
                    Fill = ellipseFillBrush
                };
                Canvas.SetLeft(e, p.X);
                Canvas.SetTop(e, p.Y);
                PathCanvas.Children.Add(e);
            }

            // Draw the lines
            SolidColorBrush lineBrush = new SolidColorBrush(Colors.Green);
            for (int i = 0; i < path.Count - 1; i++)
            {
                Line l = new Line
                {
                    Stroke = lineBrush,
                    StrokeThickness = 2,
                    X1 = path[i].X + _radius,
                    Y1 = path[i].Y + _radius,
                    X2 = path[i + 1].X + _radius,
                    Y2 = path[i + 1].Y + _radius,
                    SnapsToDevicePixels = true,

                };
                PathCanvas.Children.Add(l);

            }

        }

        internal void DrawPath(object best)
        {
            throw new NotImplementedException();
        }
    }
}
