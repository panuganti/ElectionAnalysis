/// <reference path="../../vendor/types/jquery/jquery.d.ts"/>
/// <reference path="../../vendor/types/googlemaps/google.maps.d.ts"/>


module Controllers{
    export class BiharMap {
        private http: ng.IHttpService;
        private mapDiv: HTMLElement;
        private dataloader: DataLoader;
        private map: google.maps.Map;
        private mapOptions: google.maps.MapOptions;
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;

        public mouseOverHandler: { (event: any): void } = (event) => this.mouseOver(event);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);
        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);
        public getStyleCallback: { (feature: google.maps.Data.StyleOptions): void }
                                            = (feature) => this.getStyle(feature);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http) {
            this.http = $http;
            this.mapDiv = document.getElementById('mapCanvas');
            this.dataloader = new DataLoader($http);
            this.initialize();
        }

        initialize() {
            this.mapOptions = {
                zoom: 7,
                center: this.defaultCenter,
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                minZoom: 4,
                disableDefaultUI: true
            };
            this.map = new google.maps.Map(this.mapDiv, this.mapOptions);
            this.geocoder = new google.maps.Geocoder();
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.loadStyles();
        }


        mouseOver(event: any) {
            var id = event.feature.getProperty('ac');
            /*
            1. Set small info window to visible
            2. Display Name of constituency
            3. Change color
            */
        }

        mouseClick(event: any) {
            var id = event.feature.getProperty('ac');
            /*
            1. Set info div to visible
            2. Set values for the info div
            */
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

        loadGeoData() {
            console.log("Loading geo data...");
            this.dataloader.getACTopoShapeFile(this.loadGeoJson);
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
            // Add changes and event handlers
            this.map.setZoom(8);
            // TODO: Set styles/colors
            this.addEventHandler('mouseover', this.mouseOverHandler);
            this.addEventHandler('mouseclick', this.mouseClickHandler);
        }

        addEventHandler(eventType: string, callback: (ev: Event)=> any)
        {
            this.map.data.addListener(eventType, callback);
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
