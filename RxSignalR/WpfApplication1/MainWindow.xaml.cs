using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<dynamic> chatItems;
        IHubProxy proxy;
        HubConnection cn;

        public MainWindow()
        {
            InitializeComponent();

            chatItems = new ObservableCollection<dynamic>();
            ChatResults.ItemsSource = chatItems;

            cn = new HubConnection("http://localhost:5687/");
            proxy = cn.CreateHubProxy("Chat");

            proxy.On("AddMessage", message => 
                Dispatcher.Invoke(new Action(() => chatItems.Add(message))));

            cn.Start();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await proxy.Invoke("Send", Input.Text);
        }
    }
}
