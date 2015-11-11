/// <reference path="../reference.ts" />
module Controllers {
    export class MapCtrl {
        private http: ng.IHttpService;
        private scope: ng.IScope;
        private q: ng.IQService;
        private timeout: ng.ITimeoutService;

        // Controllers
        private dataloader: DataLoader;
        private infoCtrl: InfoCtrl;
        
        private colors: string[];
        private topojson: any;
        private defaultCenter: google.maps.LatLng;
        private geocoder: google.maps.Geocoder;
        private acStyleMap: AcStyleMap;
        private resultsSummary: ResultsSummary;
        private defaultColorMap: PartyToColorMap; 
        private mapInstance: Models.Map;
        
        yearSelected: string;
        displayMode: string;
        acName = "Ac Name";

        // Years
        private years: string[] = ["2015", "2014", "2010", "2009"];
        private displayModes: string[] = ["Regular", "Stable"];

        public loadResultsHandler: { (response: any) } = (response) => this.loadResultsCallback(response);
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http, $q, $timeout) {
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            
            this.dataloader = new DataLoader(this.http, this.q);
            this.infoCtrl = new InfoCtrl(this.scope, this.http, this.q, this.timeout);
            
            this.mapInstance = Models.Map.Instance;
            this.geocoder = new google.maps.Geocoder();
            this.acStyleMap = new AcStyleMap();
            this.defaultColorMap = this.acStyleMap.colorMap;
            this.initialize();
        }
        
        // Trigger this on window load instead of triggering by constructor
        initialize() {
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.mapInstance.addEventHandler('click', this.mouseClickHandler);
            
            this.loadResults("2015");
            this.infoCtrl.setInfoDivVisibility("none");
        }
        
        mouseClick(event: any) {
            let id = event.feature.getProperty('ac');            
            let name = event.feature.getProperty('ac_name');
            this.acName = name;
            this.infoCtrl.displayInfo(id);
            console.log("In click with id:" + id + " " + this.acName);
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
            let pResults = this.dataloader.getResultsAsync(year);
            pResults.then(this.loadResultsHandler);
        }

        loadResultsCallback(acResults) {
            let styleMapsArray: AcStyleMap[];
            /*
            if (this.yearSelected == "2015") {
                styleMapsArray = this.acStyleMap.Generate2015InfoMaps(acResults);
            }
            else {
                styleMapsArray = this.acStyleMap.GenerateStyleMaps(acResults);
            } 
            */
            styleMapsArray = this.acStyleMap.GenerateStyleMaps(acResults);
            this.resultsSummary = this.acStyleMap.GenerateResultsSummary(acResults);
            let styleMaps = Enumerable.From(styleMapsArray);
            this.mapInstance.setStyle(function(feature) {
                let id = feature.getProperty('ac');
                return styleMaps.First(t=>t.Id == id).Style;
            });
        }
        
        changeDisplayMode() {
          let p2014 = this.dataloader.getResultsAsync("2014");
          let p2010 = this.dataloader.getResultsAsync("2010");
          let p2009 = this.dataloader.getResultsAsync("2009");
          this.q.all([p2014,p2010,p2009]).then((data) => this.loadDisplayMode(data[0], data[1], data[2]));
        }
        
        loadDisplayMode(ac2014, ac2010, ac2009)
        {
          let resultsHolder = new Models.ResultsHolder(ac2009, ac2010, ac2014);
          let stability: Models.Stability[] = resultsHolder.Stability;
          let styleMaps = Enumerable.From(this.acStyleMap.GenerateDisplayModeStyleMaps(stability));
          this.mapInstance.setStyle(function(feature) {
                let id = feature.getProperty('ac');
                return styleMaps.First(t=>t.Id == id).Style;
            });
        }
        
        yearSelectionChanged() {
            this.loadResults(this.yearSelected);
        }        
        
        displayModeChanged() {
            switch (this.displayMode)
            {
                case "Regular":                
                break;
                case "Stable":
                this.changeDisplayMode();
                    break;
                default:
            }
        }
    }
    
    export interface ResultsSummary {
        [alliance: string]: number;
    }    
}
