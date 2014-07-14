<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dragger.aspx.cs" Inherits="WebApplication5.Dragger" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #shape {
            height: 100px;
            width: 100px;
            background-color:#ff0000;
            cursor:move;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
        <h1>Drag Drop with SignalR</h1>
        <div>Drag this item below.</div>
        <div id="shape">
    
    
    </div>
    </form>
    <script src="Scripts/jquery-2.1.0.js"></script>
    <script src="Scripts/jquery-ui-1.10.4.js"></script>
    <script src="Scripts/jquery.signalR-2.0.2.js"></script>
    <script src="/signalr/hubs"></script>
    <script>
        $(function () {
            var $shape = $("#shape"),
                hub = $.connection.myDrag;

            hub.connection.start()
            .done(function () {
                $shape.draggable({
                    drag: function () {
                        hub.server.itemDragged(this.offsetLeft, this.offsetTop);
                    }
                });
            });

            hub.client.onDrag = function (val) {
                $shape.css({ top: val.Y, left: val.X });
            };
        });
    </script>

</body>
</html>
