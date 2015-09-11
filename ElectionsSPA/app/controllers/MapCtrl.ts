/// <reference path="../reference.ts" />
module Controllers {
    export class MapCtrl {
        private http: ng.IHttpService;
        private scope: ng.IScope;
        private q: ng.IQService;
        private timeout: ng.ITimeoutService;
        
        private infoDiv: HTMLElement;
        private info: Models.Info;
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
        public mouseClickHandler: { (event: any): void } = (event) => this.mouseClick(event);

        public getDefaultCenterCallback: { (results: any, status: any): void } = (results, status) => this.getDefaultCenter(results, status);

        constructor($scope, $http, $q, $timeout) {
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            
            this.mapInstance = Models.Map.Instance;
            this.infoDiv = document.getElementById('info');
            this.dataloader = new DataLoader(this.http, this.q);
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
            
            this.loadResults("2010");
            this.setInfoDivVisibility("none");
        }
        
        setInfoDivVisibility(display: string)
        {
            this.infoDiv.style.display = display;
        }

        mouseClick(event: any) {
            let id = event.feature.getProperty('ac');            
            let name = event.feature.getProperty('ac_name');
            this.acName = name;
            this.scope.$apply();
            this.displayInfo(id);
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
        
        displayInfo(id: string) {
            this.setInfoDivVisibility("inline");
            let p2014 = this.dataloader.getResultsAsync("2014");
            let p2010 = this.dataloader.getResultsAsync("2010");
            let p2009 = this.dataloader.getResultsAsync("2009");
            let pR: ng.IPromise<any[]>  = this.q.all([p2009, p2010, p2014]);
            pR.then(([d1, d2, d3]) => this.loadResultsForAC(d1, d2, d3, id));
        }
        
        loadResultsForAC(d1,d2,d3, id) {
            console.log('in load results');
            let r2014: Models.Result[] = d1;
            let r2010: Models.Result[] = d2;
            let r2009: Models.Result[] = d3;
            var en2014 = Enumerable.From(r2014);
            var en2010 = Enumerable.From(r2010);
            var en2009 = Enumerable.From(r2009);
            var results2014 = en2014.First(t=> t.Id == id);
            var results2010 = en2010.First(t=> t.Id == id);
            var results2009 = en2009.First(t=> t.Id == id);
            var title = results2014.Name;
            var info: Models.Info = new Models.Info(title, results2009, results2010, results2014);
            this.setInfoDivVisibility("inline");
            this.info = info;
        }
    }
    
    
    
    export interface ResultsSummary {
        [alliance: string]: number;
    }
}
