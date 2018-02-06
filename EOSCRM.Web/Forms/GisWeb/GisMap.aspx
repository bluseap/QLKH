﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GisMap.aspx.cs" Inherits="EOSCRM.Web.GisWeb.GisMap" %>

<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
    <title></title>

    <link rel="stylesheet" href="http://js.arcgis.com/3.9/js/dojo/dijit/themes/tundra/tundra.css">
    <link rel="stylesheet" type="text/css" href="http://js.arcgis.com/3.9/js/esri/css/esri.css">


    <style> 
          html, body { height: 100%; width: 100%; margin: 0; padding: 0; }
          #map { height: 100%; margin: 0; padding: 0; }
          
          #meta h3 {
            color: #666;
            font-size: 1.1em;
            padding: 0px;
            margin: 0px;
            display: inline-block;
          }
          #loading { 
            float: right;
          }

    </style> 
    <script src="http://js.arcgis.com/3.9/"></script>

    <script>
        

        var map;
        require([
          "esri/map", "esri/layers/KMLLayer", "dojo/parser", "dojo/dom-style",
          "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!"],
            function (
              Map, KMLLayer,
              parser, domStyle
            )
            {
                map = new Map("map", {
                    basemap: "topo",
                    center: [105.4221725, 10.3982413],
                    zoom: 13
                });

                parser.parse();

                var kmlUrl = "http://powaco.com.vn/GisWeb/DataGis/mj.kml";
                var kml = new KMLLayer(kmlUrl);
                map.addLayer(kml);
                kml.on("load", function () {
                    domStyle.set("loading", "display", "none");
                });
            });
        
    </script>

  </head>
    <body class="tundra">
        <div data-dojo-type="dijit/layout/BorderContainer" 
             data-dojo-props="design:'headline',gutters:false" 
             style="width: 100%; height: 100%; margin: 0;">
          <div id="map" 
               data-dojo-type="dijit/layout/ContentPane" 
               data-dojo-props="region:'center'"> 
            <div id="meta">
              <span id="loading"></span>

              
            <div>
            </div>
            </div>
    </body>

</html>