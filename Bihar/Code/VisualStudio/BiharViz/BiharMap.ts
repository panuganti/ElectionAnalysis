/// <reference path="./MapStyles.ts"/>
/// <reference path="./Scripts/typings/googlemaps/google.maps.d.ts"/>

module BiharElections {
    export class BiharMap {
        private mapDiv: HTMLElement;
        private map: google.maps.Map;
        private mapOptions: google.maps.MapOptions;

        constructor(mapDiv: HTMLElement) {
            this.mapDiv = mapDiv;
            this.initialize();
        }

        initialize() {
            this.mapOptions = {
                zoom: 5,
                center: new google.maps.LatLng(23, 84),
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                minZoom: 5,
                disableDefaultUI: true
            };
            this.map = new google.maps.Map(this.mapDiv, this.mapOptions);
        }

        setMapStyles() {
            var mapType = new ArchishaMapStyle();
            this.map.mapTypes.set(mapType.name, mapType.archishaMapType);
            this.mapOptions.mapTypeId = mapType.name;
            this.mapOptions.mapTypeControlOptions = { mapTypeIds: [mapType.name, google.maps.MapTypeId.TERRAIN] };
        }
    }
}

