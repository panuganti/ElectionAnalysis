/// <reference path="./BiharMap.ts"/>

function initialize() {
    var map = new BiharElections.BiharMap(document.getElementById('map-canvas'));
    map.setMapStyles();
}

google.maps.event.addDomListener(window, 'load', initialize);
