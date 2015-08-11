/// <reference path="../../vendor/types/jquery/jquery.d.ts"/>
/// <reference path="../../vendor/types/googlemaps/google.maps.d.ts"/>
/// <reference path="../reference.ts" />


module Controllers {
    export class BiharMap {
        private http: ng.IHttpService;
        private mapDiv: HTMLElement;
        private infoDiv: HTMLElement;
        private dataloader: DataLoader;
        private map: google.maps.Map;
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;
        private defaultColorMap: AcStyleMap; 
        private mapInstance: Models.Map;

        public load2010ResultsHandler: { (response: any) } = (response) => this.load2010ResultsCallback(response);
        public mouseOverHandler: { (event: any): void } = (event) => this.mouseOver(event);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);
        public getStyleCallback: { (feature: google.maps.Data.StyleOptions): void }
        = (feature) => this.getStyle(feature);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http) {
            $scope.vm = this;
            this.http = $http;
            this.mapInstance = Models.Map.Instance;
            this.mapDiv = this.mapInstance.MapDiv;
            this.infoDiv = document.getElementById('info');
            this.dataloader = new DataLoader(this.http);
            this.initialize();
        }
        
        initialize() {
            this.map = this.mapInstance.Map;
            this.geocoder = new google.maps.Geocoder();
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.loadStyles();
            this.load2010results();
            this.setInfoDivVisibility("none");
        }
        
        setInfoDivVisibility(display: string)
        {
            this.infoDiv.style.display = display;
        }

        mouseOver(event: any) {
            this.setInfoDivVisibility("inline");
            var id = event.feature.getProperty('ac');
            console.log("In mouseOver with id:" + id );
            /*
            1. Set small info window to visible
            2. Display Name of constituency
            3. Change color
            */
        }

        mouseClick(event: any) {
            this.setInfoDivVisibility("inline");
            var id = event.feature.getProperty('ac');
            
            console.log("In mouseclick with id:" + id);
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
            this.dataloader.getACTopoShapeFile(this.mapInstance.loadGeoJson);
        }

        addEventHandler(eventType: string, callback: (ev: Event) => any) {
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

        generateColorArray(color: string, noGradients: number): string[] {
            return ["Hello", "World"]; // TODO: Instead set this.colors 
        }
        // #endregion Colors

        // #region default style        
        // TODO:
        getStyle(feature: google.maps.Data.StyleOptions): google.maps.Data.StyleOptions {
            // TODO: Lookup color based on feature (id) from colors generated
            var style: google.maps.Data.StyleOptions =
                {
                    fillColor: 'black', // TODO: Lookup color
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
        
        // #region 2010 results
        
        load2010results() {            
            this.dataloader.get2010Results(this.load2010ResultsHandler);
        }

        load2010ResultsCallback(response) {
            console.log("loaded 2010 results");
            var results = new AcStyleMap().parseJson(response);
            this.map.data.setStyle(function(feature) {
                var id = feature.getProperty('ac');
                var winner = $.grep(results, function(e) { return e.Id == id; })[0];
                return winner.Style;
            });
            return results;
        }        
    }
}
