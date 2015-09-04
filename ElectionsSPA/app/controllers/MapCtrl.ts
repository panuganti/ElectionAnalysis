﻿/// <reference path="../reference.ts" />


module Controllers {
    export class MapCtrl {
        private http: ng.IHttpService;
        private scope: ng.IScope;
        private infoDiv: HTMLElement;
        private dataloader: DataLoader;
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;
        private defaultColorMap: AcStyleMap; 
        private mapInstance: Models.Map;
        acName = "Ac Name";

        public loadResultsHandler: { (response: any) } = (response) => this.loadResultsCallback(response);
        public mouseOverHandler: { (event: any): void } = (event) => this.mouseOver(event);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http) {
            $scope.vMap = this;
            this.scope = $scope;
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
            let id = event.feature.getProperty('ac');
            let name = event.feature.getProperty('ac_name');
            this.acName = name;
            this.scope.$apply();
            console.log("In mouseOver with id:" + id + " " + name);
        }

        mouseClick(event: any) {
            this.setInfoDivVisibility("inline");
            let id = event.feature.getProperty('ac');            
            let name = event.feature.getProperty('ac_name');
            this.acName = name;
            this.scope.$apply();
            console.log("In mouseOver with id:" + id + " " + name);
        }

        getDefaultCenter(results: any, status: any) {
            if (status == google.maps.GeocoderStatus.OK) {
                this.mapInstance.setCenter(results[0].geometry.location);
            }
            else {
            }
        }

        geocode(address: string) {
            let geocoderComponentRestrictions: google.maps.GeocoderComponentRestrictions = {};
            let request: google.maps.GeocoderRequest = {
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
            let acStyleMap = new AcStyleMap();
            let acResults: Models.Result[] = response;
            let styleMapsArray = acStyleMap.GenerateStyleMaps(acResults)
            let styleMaps = Enumerable.From(styleMapsArray);
            this.mapInstance.setStyle(function(feature) {
                let id = feature.getProperty('ac');
                return styleMaps.First(t=>t.Id == id).Style;
            });
        }        
    }
}
