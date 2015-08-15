﻿/// <reference path="../reference.ts" />


module Controllers {
    export class MapCtrl {
        private http: ng.IHttpService;
        private infoDiv: HTMLElement;
        private dataloader: DataLoader;
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;
        private defaultColorMap: AcStyleMap; 
        private mapInstance: Models.Map;

        public loadResultsHandler: { (response: any) } = (response) => this.loadResultsCallback(response);
        public mouseOverHandler: { (event: any): void } = (event) => this.mouseOver(event);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http) {
            $scope.vm = this;
            this.http = $http;
            
            this.mapInstance = Models.Map.Instance;
            this.infoDiv = document.getElementById('info');
            this.dataloader = new DataLoader(this.http);
            this.geocoder = new google.maps.Geocoder();
            
            this.initialize();
        }
        
        // Trigger this on window load instead of triggering by constructor
        initialize() {
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.mapInstance.addEventHandler('mouseover', this.mouseOverHandler);
            this.mapInstance.addEventHandler('mouseclick', this.mouseClickHandler);
            
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
        }

        mouseClick(event: any) {
            this.setInfoDivVisibility("inline");
            var id = event.feature.getProperty('ac');            
            console.log("In mouseclick with id:" + id);
        }

        getDefaultCenter(results: any, status: any) {
            if (status == google.maps.GeocoderStatus.OK) {
                this.mapInstance.setCenter(results[0].geometry.location);
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

        // #region 2010 results
        
        load2010results() {            
            this.dataloader.get2010Results(this.loadResultsHandler);
        }

        loadResultsCallback(response) {
            var results = new AcStyleMap().parseJson(response);
            this.mapInstance.setStyle(function(feature) {
                var id = feature.getProperty('ac');
                var winner = $.grep(results, function(e) { return e.Id == id; })[0];
                return winner.Style;
            });
            return results;
        }        
    }
}