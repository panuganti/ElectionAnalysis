/// <reference path="./MapStyles.ts"/>
/// <reference path="./Scripts/typings/googlemaps/google.maps.d.ts"/>
/// <reference path="./Scripts/typings/jquery/jquery.d.ts"/>

module BiharElections {
    export class BiharMap {
        private mapDiv: HTMLElement;
        private map: google.maps.Map;
        private mapOptions: google.maps.MapOptions;
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;

        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);
        public getStyleCallback: { (feature: google.maps.Data.StyleOptions): void }
                                            = (feature) => this.getStyle(feature);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor(mapDiv: HTMLElement) {
            this.mapDiv = mapDiv;
            this.initialize();
    }

        getDefaultCenter(results: any, status: any) {
            if (status == google.maps.GeocoderStatus.OK) {
                this.map.setCenter(results[0].geometry.location);
            }
            else {
            }
        }

        geocode(address: string) {
            var geocoderComponentRestrictions: google.maps.GeocoderComponentRestrictions = {};
            var request: google.maps.GeocoderRequest = {
                address: address,
                componentRestrictions: geocoderComponentRestrictions
            };
            this.geocoder.geocode(request, this.getDefaultCenterCallback);
        }

        initialize() {
            this.geocoder = new google.maps.Geocoder();
            this.geocode("Patna, Bihar, India");

            this.mapOptions = {
                zoom: 7,
                center: this.defaultCenter,
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
            this.map.setZoom(7);
            console.log("Loading geo data...");
            var url: string = "http://www.archishainnovators.com/GeoJsons/Bihar.Assembly.10k.topo.json";
            $.getJSON(url, this.loadGeoJson);
        }

        addGeoJson(data: any) {
            // Note: remember to clear map (refer shape escape website)
            var parentThis = this;
            this.topojson = data;
            if (data.objects) { //topojson
                $.each(data.objects, (i, layer) => {
                    var geojson = topojson.feature(data, layer);
                    parentThis.map.data.addGeoJson(geojson);
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

