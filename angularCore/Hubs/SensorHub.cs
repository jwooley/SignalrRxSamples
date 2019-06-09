using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AngularCore
{
    public class SensorHub : Hub
    {
        private static IObservable<SensorData> _Sensor = null;

        public ChannelReader<SensorData> Values()
        {
            if (_Sensor == null)
            {
                var rand = new Random(DateTime.Now.Millisecond);
                _Sensor = Observable.Generate(
                    initialState: 0.0,
                    condition: x => true,
                    iterate: inVal => rand.NextDouble(),
                    resultSelector: val => new SensorData
                    {
                        TimeStamp = DateTime.Now,
                        SensorType = (Math.Floor(val * 4) + 1).ToString(),
                        SensorValue = val * 20
                    },
                    timeSelector: val => TimeSpan.FromMilliseconds(val * 1000)); 
            }
            return _Sensor.AsChannelReader();
        }
    }

    public static class ObservableExtensions
    {
        public static ChannelReader<T> AsChannelReader<T>(this IObservable<T> observable, int? maxBufferSize = null)
        {
            // This sample shows adapting an observable to a ChannelReader without 
            // back pressure, if the connection is slower than the producer, memory will
            // start to increase.

            // If the channel is bounded, TryWrite will return false and effectively
            // drop items.

            // The other alternative is to use a bounded channel, and when the limit is reached
            // block on WaitToWriteAsync. This will block a thread pool thread and isn't recommended and isn't shown here.
            var channel = maxBufferSize != null ? Channel.CreateBounded<T>(maxBufferSize.Value) : Channel.CreateUnbounded<T>();

            var disposable = observable.Subscribe(
                                value => channel.Writer.TryWrite(value),
                                error => channel.Writer.TryComplete(error),
                                () => channel.Writer.TryComplete());

            // Complete the subscription on the reader completing
            channel.Reader.Completion.ContinueWith(task => disposable.Dispose());

            return channel.Reader;
        }
    }

    public class SensorData
    {
        public DateTime TimeStamp { get; set; }
        public String SensorType { get; set; }
        public double SensorValue { get; set; }
    }
}
