/// <reference path="./reference.ts" />

function initialize() {
    var map: MapView.BiharMap = new MapView.BiharMap(document.getElementById('mapCanvas'));
    map.loadGeoData();
    map.loadStyles();
}

google.maps.event.addDomListener(window, 'load', initialize);

angular.module('ElectionVisualization',['controllers','services','directives']);