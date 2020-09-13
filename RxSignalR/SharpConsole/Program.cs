using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace SharpConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cn = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/hub/sensor")
                .WithAutomaticReconnect()
                .Build();

            await cn.StartAsync();

            var items = 
                from sensor in cn.AsObservable<SensorData>("broadcast")
                where sensor.Value < 5
                select sensor;

            items.Subscribe(sensor => Console.WriteLine(sensor.Value));

            Console.WriteLine("Receiving Values. Press any key to exit.");
            Console.ReadLine();
        }
    }
    public static class ObservableExtensions
    {
        public static IObservable<T> AsObservable<T>(this HubConnection connection, string methodName)
        {
            var subject = new Subject<T>();
            Task.Run(async () =>
            {
                var stream = connection.StreamAsync<T>(methodName);
                await foreach (var value in stream)
                {
                    subject.OnNext(value);
                }
            });
            return subject;
        }
    }
}
