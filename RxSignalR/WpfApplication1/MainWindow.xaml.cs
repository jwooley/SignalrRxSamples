using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> chatItems;
        HubConnection cn;

        public MainWindow()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            chatItems = new ObservableCollection<string>();
            ChatResults.ItemsSource = chatItems;

            cn = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/Hub/Chat")
                .Build();
            await cn.StartAsync();

            cn.On<string>("addMessage", message => 
                chatItems.Add(message));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await cn.InvokeAsync("Send", Input.Text);
        }
    }
}
