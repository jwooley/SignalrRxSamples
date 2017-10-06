<Query Kind="VBProgram">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Runtime</Namespace>
</Query>

Sub Main
	Dim sensor = GetSensor()
	
	sensor.DumpLive("All Values")
	
	Dim Q1 = From s In sensor
			Where s.SensorValue < 3
			Select s
	
	Q1.DumpLive("Low Value Sensors")
	
	Dim Q2 = From s In sensor
			Where s.SensorType = "3"
			Select s
			
	Q2.DumpLive("Type 3 Sensors")
	
	Dim Heartbeat = (From s In sensor
		Where s.SensorType = "2").
		Buffer(TimeSpan.FromSeconds(3)).
		Select(Function(b) b.Count * 20).
		DumpLive("Approximate 2's per minute")
		
	Dim GroupTest = 
		sensor.
		GroupBy(Function(s) s.SensorType).
		Select(Function(s) s.DumpLive($"Latest for key: {s.Key}")).
		DumpLive("Grouped")
			
End Sub

' Define other methods and classes here
Public Class SensorInfo
    Public Property TimeStamp As DateTime
    Public Property SensorType As String
    Public Property SensorValue As Double
	Public Overrides Function ToString() As String
	    Return String.Format("Time: {0}  , Type: {1}  Value: {2}", TimeStamp, SensorType, SensorValue)
	End Function
End Class

Public Function GetSensor() As IObservable(Of SensorInfo)
	Dim rnd = New Random(Date.Now.Millisecond)
	Return Observable.Generate(Of Double, SensorInfo)( 
		initialState:=0.0,
		condition:=Function(x) True,
		iterate:= Function(inVal) rnd.NextDouble,
		resultSelector:= Function(val)
				Return New SensorInfo With {
					.SensorType = (math.Floor(val * 4) + 1).ToString,
					.SensorValue = rnd.NextDouble * 20,
					.TimeStamp = DateTime.Now}
			End Function,
		timeSelector := Function(val) TimeSpan.FromMilliseconds(val * 100)
	)
End Function