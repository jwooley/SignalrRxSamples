<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication3.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #shape {
            height: 100px; width: 100px; background-color: black; cursor: move;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="shape">
    
    </div>
    </form>

    <script src="Scripts/jquery-2.1.0.js"></script>
    <script src="Scripts/jquery-ui-1.10.4.js"></script>
    <script src="Scripts/jquery.signalR-2.0.2.js"></script>
    <script src="/signalr/hubs"></script>
    <script>
        $(function () {
            var shape = $("#shape"),
                hub = $.connection.myDrag;
            hub.client.onDrag = function (val) {
                shape.css({top: val.Y, left: val.X});
            };

            hub.connection.start()
                .done(function () {
                    shape.draggable({
                        drag: function () {
                            hub.server.itemDragged(this.offsetLeft, this.offsetTop)
                        }
                    });
                });
        });
    </script>
</body>
</html>
