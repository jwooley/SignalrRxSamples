<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/jquery-2.1.0.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-2.0.3.js" type="text/javascript"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            // Proxy created on the fly
            var hub = $.connection.observableSensorHub;

            // Declare a function on the chat hub so the server can invoke it
            hub.client.Broadcast = function (value) {
                $('#values').prepend('<li>' + value.Time + ': ' + value.Value + '</li>');
            };

            // Start the connection
            $.connection.hub.start();
        });
    </script>
</head>
<body>
    <ul id="values">
    </ul>
</body>
</html>
