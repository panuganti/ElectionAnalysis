﻿/// <reference path="./BiharMap.ts"/>

function initialize() {
    var map: BiharElections.BiharMap = new BiharElections.BiharMap(document.getElementById('mapCanvas'));
    map.setMapStyles();
    map.loadGeoData();
    map.loadStyles();
}

google.maps.event.addDomListener(window, 'load', initialize);
