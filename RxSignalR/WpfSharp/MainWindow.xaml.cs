using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SignalR.Client.Hubs;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;

namespace WpfSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ObservableCollection<SensorData> items;

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            items = new ObservableCollection<SensorData>();
            ItemList.ItemsSource = items;

            // Connect to the service
            var hubConnection = new HubConnection("http://localhost:5687/");

            // Create a proxy to the chat service
            var chat = hubConnection.CreateProxy("observableSensorHub");

            // Print the message when it comes in
            //chat.On<SensorData>("broadcast", value =>
            //    Dispatcher.BeginInvoke(new Action(() => items.Insert(0, value))));

            chat.Observe("broadcast")
                .Select(item => item[0].ToObject<SensorData>())
                .OfType<SensorData>()
                .Where(item => item.Category == "1")
                .ObserveOnDispatcher()
                .Subscribe(item => items.Insert(0, item));

            // Start the connection
            await hubConnection.Start();

        }
    }
}
