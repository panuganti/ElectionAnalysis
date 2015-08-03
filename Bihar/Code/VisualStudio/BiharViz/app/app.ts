/// <reference path="./BiharMap.ts"/>

function initialize() {
    var map: BiharElections.BiharMap = new BiharElections.BiharMap(document.getElementById('mapCanvas'));
    map.setMapStyles();
    map.loadGeoData();
    map.loadStyles();
}

// Use AngularJs
var controller = angular.module('controller', []);
controller.controller(BiharElectionsController.Controller.constructor);
var app = angular.module('BiharViz', ['controller']);

function HelloWorldCtrl($scope) {
    $scope.helloMessage = "D";
    $scope.vm = "B";
}
//google.maps.event.addDomListener(window, 'load', initialize);

