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
        private acStyleMap: AcStyleMap;
        private resultsSummary: ResultsSummary;
        private defaultColorMap: PartyToColorMap; 
        private mapInstance: Models.Map;
        private years: string[] = ["2014", "2010", "2009"];
        yearSelected: string;
        acName = "Ac Name";

        public loadResultsHandler: { (response: any) } = (response) => this.loadResultsCallback(response);
        public mouseOverHandler: { (event: any): void } = (event) => this.mouseOver(event);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http, $q) {
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            
            this.mapInstance = Models.Map.Instance;
            this.infoDiv = document.getElementById('info');
            this.dataloader = new DataLoader(this.http, $q);
            this.geocoder = new google.maps.Geocoder();
            this.acStyleMap = new AcStyleMap();
            this.defaultColorMap = this.acStyleMap.colorMap;
            this.initialize();
        }
        
        // Trigger this on window load instead of triggering by constructor
        initialize() {
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.mapInstance.addEventHandler('mouseover', this.mouseOverHandler);
            this.mapInstance.addEventHandler('mouseclick', this.mouseClickHandler);
            
            this.loadResults("2010");
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
        
        loadResults(year: string) {            
            var pResults = this.dataloader.getResultsAsync(year);
            pResults.then(this.loadResultsHandler);
        }

        loadResultsCallback(acResults) {
            let styleMapsArray = this.acStyleMap.GenerateStyleMaps(acResults);
            this.resultsSummary = this.acStyleMap.GenerateResultsSummary(acResults);
            let styleMaps = Enumerable.From(styleMapsArray);
            this.mapInstance.setStyle(function(feature) {
                let id = feature.getProperty('ac');
                return styleMaps.First(t=>t.Id == id).Style;
            });
        }
        
        yearSelectionChanged() {
            this.loadResults(this.yearSelected);
        }
    }
    
    export interface ResultsSummary {
        [alliance: string]: number;
    }
}
/// <reference path="../reference.ts" />
