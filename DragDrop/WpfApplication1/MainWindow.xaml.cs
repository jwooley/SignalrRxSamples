
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
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
        HubConnection cn;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            cn = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/myDrag")
                .WithAutomaticReconnect()
                .Build();
            await cn.StartAsync();

            cn.Closed += async (error) =>
            {
                await Task.Delay(1000);
                await cn.StartAsync();
            };

            ConfigureSignalR();
            ConfigureRx();
            ConfigureRxComplex();
            SetupDrag();
        }

        private void ConfigureSignalR()
        {
            cn.On<Coord>("onDrag", val =>
            {
                Canvas.SetLeft(myShape, val.x);
                Canvas.SetTop(myShape, val.y);
            });
        }

        private void ConfigureRx()
        {
            cn.AsObservable<Coord>("onDrag")
                .Delay(TimeSpan.FromMilliseconds(250))
                .ObserveOnDispatcher()
                .Subscribe(val =>
                {
                    Canvas.SetLeft(myShape2, val.x);
                    Canvas.SetTop(myShape2, val.y);
                });
        }

        private void ConfigureRxComplex()
        {
            cn.AsObservable<Coord>("onDrag")
                .Delay(TimeSpan.FromMilliseconds(250))
                .ObserveOnDispatcher()
                .Subscribe(pos =>
                {
                    Canvas.SetLeft(myShape3, pos.x);
                    Canvas.SetTop(myShape3, pos.y);
                    byte colorVal = Convert.ToByte(Math.Abs(Math.Sin(((int)(pos.x) + (int)pos.y)) * 128) + 127);

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
                            x = Convert.ToInt32(endLocation.X - startLocation.X),
                            y = Convert.ToInt32(endLocation.Y - startLocation.Y)
                        };

            query.Subscribe(position =>
                {
                    Canvas.SetLeft(myShape, position.x);
                    Canvas.SetTop(myShape, position.y);
                });

            query.Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(position => cn.InvokeAsync("ItemDragged", position.x, position.y));
        }
    }

    public static class ObservableExtensions
    {
        public static IObservable<T> AsObservable<T>(this HubConnection connection, string methodName)
        {
            var subject = new Subject<T>();
            connection.On<T>(methodName, (val) => subject.OnNext(val));

            return subject;
        }
    }
    public class Coord
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
