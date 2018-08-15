using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace angularCore.Hubs
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
            return _Sensor.AsChannelReader(Context.ConnectionAborted);
        }
    }

    public class SensorData
    {
        public DateTime TimeStamp { get; set; }
        public String SensorType { get; set; }
        public double SensorValue { get; set; }
    }

}
