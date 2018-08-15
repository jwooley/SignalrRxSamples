using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IHubProxy proxy;
        HubConnection cn;

        public MainWindow()
        {
            InitializeComponent();

            cn = new HubConnection("http://localhost:1931");
            proxy = cn.CreateHubProxy("myDrag");

       
            ConfigureSignalR();
            ConfigureRx();
            ConfigureRxComplex();

            cn.Start();

            SetupDrag();
        }

        private void ConfigureSignalR()
        {
            proxy.On("onDrag", val =>
            {
                double x = val.X;
                double y = val.Y;
                Dispatcher.Invoke(new Action(() =>
                {
                    Canvas.SetLeft(myShape, x);
                    Canvas.SetTop(myShape, y);
                }));
            });

        }

        private void ConfigureRx()
        {
            proxy.Observe("onDrag").Delay(TimeSpan.FromMilliseconds(250))
                .Select(tokens => (dynamic)tokens[0])
                .ObserveOnDispatcher()
                .Subscribe(pos =>
                {
                    Canvas.SetLeft(myShape2, (double)(pos.X));
                    Canvas.SetTop(myShape2, (double)(pos.Y));
                });
        }

        private void ConfigureRxComplex()
        {
            proxy.Observe("onDrag")
                .Select(tokens => (dynamic)tokens[0])
                .Delay(TimeSpan.FromMilliseconds(250))
                .ObserveOnDispatcher()
                .Subscribe(pos =>
                {
                    Canvas.SetLeft(myShape3, (double)(pos.X));
                    Canvas.SetTop(myShape3, (double)(pos.Y));
                    byte colorVal = Convert.ToByte(Math.Abs(Math.Sin(((int)(pos.X) + (int)pos.Y)) * 128) + 127);

                    var brush = new SolidColorBrush(Color.FromRgb(colorVal, 0, 0));
                    var b2 = new RadialGradientBrush(Color.FromRgb(colorVal, 0, 0), Color.FromRgb(255, 255, 255));
                    myShape.Fill = b2;
                });

        }

        private void SetupDrag()
        {
            var mouseDown = from evt in Observable.FromEventPattern<MouseButtonEventArgs>(myShape, "MouseLeftButtonDown")
                            select evt.EventArgs.GetPosition(myShape);
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseLeftButtonUp");
            var mouseMove = from evt in Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
                            select evt.EventArgs.GetPosition(this);

            var query = from startLocation in mouseDown
                        from endLocation in mouseMove
                        .TakeUntil(mouseUp)
                        select new
                        {
                            x = Convert.ToInt32( endLocation.X - startLocation.X),
                            y = Convert.ToInt32(endLocation.Y - startLocation.Y)
                        };

            query.Subscribe(position =>
                {
                    Canvas.SetLeft(myShape, position.x);
                    Canvas.SetTop(myShape, position.y);
                });

            query.Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(position => proxy.Invoke("ItemDragged",position.x, position.y));
        }

        private void myShape_MouseMove(object sender, MouseEventArgs e)
        {
            //proxy.Invoke("ItemDragged", e.GetPosition(this).X, e.GetPosition(this).Y);
        }
    }
}
