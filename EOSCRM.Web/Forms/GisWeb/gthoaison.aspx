<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gthoaison.aspx.cs" Inherits="EOSCRM.Web.GisWeb.gthoaison" %>

<!DOCTYPE html>

<html>
  <head>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no">
    <meta charset="utf-8">
    <title>KML Layers</title>
    <style>
      html, body, #map-canvas {
        height: 100%;
        margin: 0px;
        padding: 0px
      }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp"></script>
    <script>
        function initialize() {
            var chicago = new google.maps.LatLng(10.398383, 105.422265);
            var mapOptions = {
                zoom: 11,
                center: chicago
            }

            var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

            var ctaLayer = new google.maps.KmlLayer({
                url: 'http://powaco.com.vn/GisWeb/thoaison/thoaison.kmz'
            });
            ctaLayer.setMap(map);
        }

        google.maps.event.addDomListener(window, 'load', initialize);

    </script>
  </head>
  <body>
    <div id="map-canvas"></div>
  </body>
</html>
