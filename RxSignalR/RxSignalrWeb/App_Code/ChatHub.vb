Option Strict Off

Imports Microsoft.AspNet.SignalR
Imports Microsoft.VisualBasic

Public Class Chat
    Inherits Hub

    Public Sub Send(message As String)
        Clients.All.addMessage(message)
    End Sub
End Class
