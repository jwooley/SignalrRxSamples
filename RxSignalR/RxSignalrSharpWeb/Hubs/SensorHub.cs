using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RxSignalrSharpWeb.Hubs
{
    public class SensorHub : Hub
    {
        private static IObservable<SensorData> _Sensor = null;

        public ChannelReader<SensorData> Values(CancellationToken cancellationToken)
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
            return _Sensor.AsChannelReader(cancellationToken);
        }

        public ChannelReader<int> Counter(
            int count,
            int delay,
            CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<int>();

            // We don't want to await WriteItemsAsync, otherwise we'd end up waiting 
            // for all the items to be written before returning the channel back to
            // the client.
            _ = WriteItemsAsync(channel.Writer, count, delay, cancellationToken);

            return channel.Reader;
        }

        private async Task WriteItemsAsync(
            ChannelWriter<int> writer,
            int count,
            int delay,
            CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                for (var i = 0; i < count; i++)
                {
                    await writer.WriteAsync(i, cancellationToken);

                    // Use the cancellationToken in other APIs that accept cancellation
                    // tokens so the cancellation can flow down to them.
                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                localException = ex;
            }

            writer.Complete(localException);
        }
    }

    public static class ObservableExtensions
    {
        public static ChannelReader<T> AsChannelReader<T>(this IObservable<T> observable, CancellationToken cancellationToken)
        {
            // This sample shows adapting an observable to a ChannelReader without 
            // back pressure, if the connection is slower than the producer, memory will
            // start to increase.

            // If the channel is bounded, TryWrite will return false and effectively
            // drop items.

            // The other alternative is to use a bounded channel, and when the limit is reached
            // block on WaitToWriteAsync. This will block a thread pool thread and isn't recommended and isn't shown here.
            var channel = Channel.CreateUnbounded<T>();

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
