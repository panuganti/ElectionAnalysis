/// <reference path="./MapStyles.ts"/>
/// <reference path="./Scripts/typings/googlemaps/google.maps.d.ts"/>
/// <reference path="./Scripts/typings/jquery/jquery.d.ts"/>

module BiharElections {
    export class BiharMap {
        private mapDiv: HTMLElement;
        private map: google.maps.Map;
        private mapOptions: google.maps.MapOptions;

        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);

        constructor(mapDiv: HTMLElement) {
            this.mapDiv = mapDiv;
            this.initialize();
        }

        initialize() {
            this.mapOptions = {
                zoom: 5,
                center: new google.maps.LatLng(23, 84),
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                minZoom: 4,
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

        loadGeoData() {
            var center = this.map.getCenter();
            this.mapOptions = {
                center: center,
                zoom: 11
            };
            console.log("Loading geo data...");
            var url: string = "http://www.archishainnovators.com/GeoJsons/Bihar.Assembly.10k.topo.json";
            var biharMap = this;
            var geoJsonObject;
            $.getJSON(url, this.loadGeoJson);
        }

        addGeoJson(data: any) {
            // Note: remember to clear map (refer shape escape website)
            if (data.objects) { //topojson
                $.each(data.objects, function (i, layer) {
                    var geojson = topojson.feature(data, layer);
                    this.map.data.addGeoJson(data);
                });
                console.log("Loading completed");
            } else { // geojson
                this.map.data.addGeoJson(data);
                console.log("Loading completed");
            }
        }

        loadStyles() {
            var style: google.maps.Data.StyleOptions =
                {
                    fillColor: 'green',
                    strokeWeight: 1,
                    fillOpacity: 0.3,
                    strokeOpacity: 0.3,
                };
            this.map.data.setStyle(style);
        }
    }
}

