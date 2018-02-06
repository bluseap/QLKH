

// If you're adding a number of markers, you may want to
// drop them on the map consecutively rather than all at once.
// This example shows how to use setTimeout() to space
// your markers' animation.

var berlin = new google.maps.LatLng(52.520816, 13.410186);

/*var neighborhoods = [
  new google.maps.LatLng(52.511467, 13.447179),
  new google.maps.LatLng(52.549061, 13.422975),
  new google.maps.LatLng(52.497622, 13.396110),
  new google.maps.LatLng(52.517683, 13.394393)
];
*/

var markers = [];
var iterator = 0;

var map;

function initialize() {
    var mapOptions = {
        zoom: 12,
        center: berlin
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
            mapOptions);
}

function drop() {
    /*for (var i = 0; i < neighborhoods.length; i++) {
        setTimeout(function () {
            addMarker();
        }, i * 200);
    }*/

    //var neighborhoods = [  new google.maps.LatLng(52.511467, 13.447179),

    //var neighborhoods = [new google.maps.LatLng(52.511467, 13.447179)]
    //var mm = EOSCRM.Web.Common.AjaxCRM.GetLatLong();
    //var mm = EOSCRM.Web.GisWeb.WorlMap.GetCurrentTime();
    //alert(mm);
    /*var neighborhoods = [new google.maps.LatLng(52.511467, 13.447179)]
    for (var i = 0; i < neighborhoods.length; i++) {
        setTimeout(function () {
            addMarker();
        }, i * 200);
    }*/   
    getnn();
}
function getnn() {
    var mm = EOSCRM.Web.Common.AjaxCRM.GetCurrentTime();
    alert(mm.ToString());
    /*$.ajax({
        type: "POST",
        url: "EOSCRM.Web.Common.AjaxCRM.cs/GetCurrentTime",
        //url: "~/GisWeb/WorlMap.aspx/GetCurrentTime",
        data: "{}",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: OnSuccess
    });*/
}
/*function OnSuccess(data) {
    alert(data.d);
}*/
function getlat() {
    for (var i = 0; i < neighborhoods.length; i++) {
        setTimeout(function () {
            addMarker();
        }, i * 200);
    }
}

function addMarker() {
    markers.push(new google.maps.Marker({
        position: neighborhoods[iterator],
        map: map,
        draggable: false,
        animation: google.maps.Animation.DROP
    }));
    iterator++;
}

google.maps.event.addDomListener(window, 'load', initialize);

