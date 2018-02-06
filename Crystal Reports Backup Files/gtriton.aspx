<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gtriton.aspx.cs" Inherits="EOSCRM.Web.GisWeb.gtriton" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>POWACO : CÔNG TY CỔ PHẦN ĐIỆN NƯỚC AN GIANG</title>
<style type="text/css">
     html, body, #map-canvas {
        height: 100%;
        margin: 0px;
        padding: 0px
      }

     #floating-panel {
  position: absolute;
  top: 10px;
  left:30%;
  z-index: 5;
  background-color: #fff;
  padding: 1px;
  border: 1px solid #999;
  text-align: center;
  font-family: 'Roboto','sans-serif';
  
  
}

    #submit {
        height: 36px;
        width: 70px;
        font-size:large;
    }

</style>
<script type="text/javascript"
src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=false">
</script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places">
</script>
<script type="text/javascript">
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(success);

    } else {
        navigator.geolocation.getCurrentPosition(success);
        alert("Geo Location is not supported on your current browser!");
    }

    function success(position) {
        var lat = position.coords.latitude;
        var long = position.coords.longitude;
        var city = position.coords.locality;
        var myLatlng = new google.maps.LatLng(lat, long);
        var myOptions = {
            center: myLatlng,
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        var marker = new google.maps.Marker({
            position: myLatlng,
            title: "lat: " + lat + " long: " + long
        });

        marker.setMap(map);
        var infowindow = new google.maps.InfoWindow({ content: "<b>XN LONG XUYÊN</b><br/> Latitude:" + lat + "<br /> Longitude:" + long + "" });
        infowindow.open(map, marker);

        var ctaLayer = new google.maps.KmlLayer({
            url: 'http://powaco.com.vn/GisWeb/triton/TRITON.kmz'
        });
        ctaLayer.setMap(map);


        //var geocoder = new google.maps.Geocoder();
        document.getElementById('submit').addEventListener('click', function () {
            //alert("Geo Location is not supported on your current browser!");
            var marker = new google.maps.Marker({
                position: myLatlng,
                title: "lat: " + lat + " long: " + long
            });
            marker.setMap(map);
            var infowindow = new google.maps.InfoWindow({ content: "<b>XN LONG XUYÊN</b><br/> Latitude:" + lat + "<br /> Longitude:" + long + "" });
            infowindow.open(map, marker);
            //geocodeAddress(geocoder, myLatlng);
        });
    }

    function geocodeAddress(geocoder, resultsMap) {
        var address = document.getElementById('address').value;
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                //alert("Geo Location is not supported on your current browser!");

                resultsMap.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: resultsMap,
                    position: results[0].geometry.location
                });
            } else {
                alert('Lấy tọa độ không thành công: ' + status);
            }
        });
    }
</script>
</head>
<body >
    <div id="floating-panel">      
      <input id="submit" type="button" value="Vị trí">
    </div>
    <div id="map_canvas" style="width:auto; height: 100%"></div>

</body>
</html>

