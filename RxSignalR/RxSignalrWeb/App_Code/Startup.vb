Imports Microsoft.VisualBasic
Imports Owin

Public Class Startup
    Public Sub Configuration(app As IAppBuilder)
        app.MapSignalR()
    End Sub

End Class
