Option Strict Off
Option Infer On

Imports System
Imports System.Reactive.Linq
Imports Microsoft.AspNet.SignalR

Imports Microsoft.VisualBasic

Public Class ObservableSensor

    Public Sub New()
        Dim rand = New Random(Now.Millisecond)

        Dim generator = Observable.Generate(Of Double, SensorData)(
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

        Dim query = From val In generator
                    Select val

        query.Subscribe(Sub(value)
                            GlobalHost.ConnectionManager.
                                    GetHubContext(Of ObservableSensorHub)().
                                    Clients.All.Broadcast(value)
                        End Sub)
    End Sub

End Class

