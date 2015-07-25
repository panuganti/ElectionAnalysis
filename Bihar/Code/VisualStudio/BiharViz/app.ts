/// <reference path="./Scripts/typings/googlemaps/google.maps.d.ts"/>

module BiharElections {
    export class BiharMap {
        private mapDiv: HTMLElement;

        constructor(mapDiv: HTMLElement) {
            this.mapDiv = mapDiv;
            this.initialize();
        }

        initialize() {
            var map;
            var mapOptions: google.maps.MapOptions = {
                zoom: 5,
                center: new google.maps.LatLng(23, 84),
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                minZoom: 5,
                disableDefaultUI: true
            };
            map = new google.maps.Map(this.mapDiv, mapOptions);
        }
    }
}

function initialize() {
    var map = new BiharElections.BiharMap(document.getElementById('map-canvas'));
}

//google.maps.event.addDomListener(window, 'load', initialize);


window.onload = () => {
    var map = new BiharElections.BiharMap(document.getElementById('map-canvas'));
};


