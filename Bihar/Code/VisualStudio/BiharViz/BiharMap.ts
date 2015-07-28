/// <reference path="./MapStyles.ts"/>
/// <reference path="./Scripts/typings/googlemaps/google.maps.d.ts"/>
/// <reference path="./Scripts/typings/jquery/jquery.d.ts"/>

module BiharElections {
    export class BiharMap {
        private mapDiv: HTMLElement;
        private map: google.maps.Map;
        private mapOptions: google.maps.MapOptions;
        private colors: string[];

        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);
        public getStyleCallback: { (feature: google.maps.Data.StyleOptions): void }
                                            = (feature) => this.getStyle(feature);

        constructor(mapDiv: HTMLElement) {
            this.mapDiv = mapDiv;
            this.initialize();
        }

        initialize() {
            this.mapOptions = {
                zoom: 7,
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
            var parent = this;
            if (data.objects) { //topojson
                $.each(data.objects, function (i, layer) {
                    var geojson = topojson.feature(data, layer);
                    parent.map.data.addGeoJson(geojson);
                });
                console.log("Loading completed");
            } else { // geojson
                this.map.data.addGeoJson(data);
                console.log("Loading completed");
            }
        }

        //#region Toggle Data Layer
        viewDataLayer() {
            this.map.data.setStyle({ visible: true });
        }

        hideDataLayer() {
            this.map.data.setStyle({ visible: false });
        }
        //#endregion Toggle Data Layer

        //#region Colors
        generateRainbowColors(numColors: number): string[] {
            return ["Hello", "World"]; // TODO: Instead set this.colors
        }

        generateColorArray(color: string, noGradients: number) : string[] {
            return ["Hello", "World"]; // TODO: Instead set this.colors 
        }
        // #endregion Colors

        // TODO:
        getStyle(feature: google.maps.Data.StyleOptions): google.maps.Data.StyleOptions {
            // TODO: Lookup color based on feature (id) from colors generated
            var style: google.maps.Data.StyleOptions =
                {
                    fillColor: 'green', // TODO: Lookup color
                    strokeWeight: 1,
                    fillOpacity: 0.3,
                    strokeOpacity: 0.3,
                };
            return style;
        }

        loadStyles() {
            // TODO:
            // 0. Decide what kind of styling we want
            // 1. Generate colors array
            this.map.data.setStyle(this.getStyleCallback);
        }
    }
}

