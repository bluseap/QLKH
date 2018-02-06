<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorldMap.aspx.cs" Inherits="EOSCRM.Web.GisWeb.WorldMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
    <meta charset="utf-8">
    <title>Marker animations with <code>setTimeout()</code></title>
    <style>
      html, body, #map-canvas {
        height: 100%;
        margin: 0px;
        padding: 0px
      }

      #panel {
        position: absolute;
        top: 5px;
        left: 50%;
        margin-left: -180px;
        z-index: 5;
        background-color: #fff;
        padding: 5px;
        border: 1px solid #999;
      }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp"></script>
</head>
<body>
<form id="form1" runat="server">
    
    
    <script type="text/javascript">
         var markers = [
             <asp:Repeater ID="rptMarkers" runat="server">
             <ItemTemplate>
                         {
                             "title": '<%# Eval("Name") %>',
                            "lat": '<%# Eval("LAT") %>',
                            "lng": '<%# Eval("LONG") %>',
                            "description": '<%# Eval("Description") %>'
                }
            </ItemTemplate>
            <SeparatorTemplate>
            ,
            </SeparatorTemplate>
            </asp:Repeater>
        ];
    </script>

    <script>       

        window.onload = function () {
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var infoWindow = new google.maps.InfoWindow();
            var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        //infoWindow.setContent(data.description);
                        infoWindow.setContent(data.title);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
        }

    </script>
    <div id="map-canvas" style="height: 800px"  ></div>

</form>
</body>
</html>
