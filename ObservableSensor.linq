<Query Kind="VBProgram">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Runtime</Namespace>
</Query>

Sub Main
	dim sensor = GetSensor()
	
	sensor.DumpLive("All Values")
	
	Dim Q1 = From s in sensor
			Where s.SensorValue < 3
			Select s
	
	Q1.DumpLive("Low Value Sensors")
	
	Dim Q2 = From s in sensor
			Where s.SensorType = "3"
			Select s
			
	Q2.DumpLive("Type 3 Sensors")
	
	Dim Heartbeat = (from s in sensor
		Where s.SensorType = "2").
		Buffer(TimeSpan.FromSeconds(3)).
		Select(Function(b) b.Count).
		DumpLive("Total Last 3 Seconds")
		
	Dim GroupTest = 
		sensor.
		GroupBy(function(s) s.SensorType).
		Select(function(s) s.DumpLive(s.Key)).
		DumpLive
			
End Sub

' Define other methods and classes here
Public Class SensorInfo
    Public Property TimeStamp As DateTime
    Public Property SensorType As String
    Public Property SensorValue As Double
	public overrides Function ToString() as String
	    Return String.Format("Time: {0}  , Type: {1}  Value: {2}", TimeStamp, SensorType, SensorValue)
	End Function
End Class

Public Function GetSensor() as IObservable(Of SensorInfo)
	Dim rnd = new Random(Date.Now.Millisecond)
	Return Observable.Generate(Of Double, SensorInfo)( _
		initialState:=0.0,
		condition:=Function(x) True,
		iterate:= Function(inVal) rnd.NextDouble,
		resultSelector:= Function(val)
				return new SensorInfo with {
					.SensorType = (math.Floor(val * 4) + 1).ToString,
					.SensorValue = rnd.NextDouble * 20,
					.TimeStamp = DateTime.Now}
			end Function,
		timeSelector := Function(val) TimeSpan.FromMilliseconds(val * 250)
	)
End Function