Option Strict Off
Imports System.Reactive.Linq
Imports Microsoft.AspNet.SignalR

Imports Microsoft.VisualBasic

Public Class ObservableSensor

    Public Sub New()
        Dim rand = New Random(Now.Millisecond)

        Dim Generator = Observable.Generate(Of Double, SensorData)(
            initialState:=0,
            condition:=Function(val) True,
            iterate:=Function(val) rand.NextDouble,
            resultSelector:=Function(val) New SensorData With
                                          {
                                              .Time = Now,
                                              .Value = val * 20,
                                              .Category = (CInt(val * 4)).ToString()
                                          },
            timeSelector:=Function(val) TimeSpan.FromSeconds(val))

        Dim query = From val In Generator
                    Where val.Category = "1"
                    Select val

        Generator.Subscribe(Sub(value)
                                Dim context = GlobalHost.ConnectionManager.
                                    GetHubContext(Of ObservableSensorHub)()
                                context.Clients.All.Broadcast(value)
                            End Sub)
    End Sub

End Class

