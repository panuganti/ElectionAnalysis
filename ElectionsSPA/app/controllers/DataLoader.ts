/// <reference path="../reference.ts" />
module Controllers {
    export class DataLoader {
        private _acShapeFile: string = "json/Bihar.Assembly.10k.topo.json";
        private _allACsJson: string = "json/allACs.json";
        private _results2009Json: string = "json/results2009AcWise.json";
        private _results2010Json: string = "json/results2010AcWise.json";
        private _results2014Json: string = "json/results2014AcWise.json";
        private _results2015Json: string = "json/results2015AcWise.json";
        private _localIssues2015: string = "";
        private _localIssues2010: string = "";
        private _casteDistribution: string = "";
        private casteCategoryDistribution: string = "";
        private candidateInfo: string = "";
        private vipConstituencies: string = "";
        private _predictions2015: string = "";
        private _neighbors: string = "json/Neighbors.txt";

        private _results2015 = null;
        private _results2010 = null;
        private _results2014 = null;
        private _results2009 = null;
        
        private http: ng.IHttpService;
        private q: ng.IQService;
        private headers: any = { 'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ' }

        constructor($http, $q) {
            this.http = $http;
            this.q = $q;
        }

        getColorsJson(callback: (ev: Event) => any) {
        }

        getACTopoShapeFile(callback: (ev: Event) => any) {
            this.http.get(this._acShapeFile, this.headers).success(callback);
        }

        getAllAssemblyConstituencies(callback: (ev: Event) => any) {
            this.http.get(this._allACsJson, this.headers).success(callback);
        }

        get2010ResultsAsync(): ng.IPromise<string[]> {
            var deferred = this.q.defer();
            if (this._results2010 !== null) { deferred.resolve(this._results2010); }
            this.http.get(this._results2010Json, this.headers)
                     .success((data) => deferred.resolve(data));
            return deferred.promise;
        }

        get2014ResultsAsync(): ng.IPromise<string[]> {
            var deferred = this.q.defer();
            if (this._results2014 !== null) { deferred.resolve(this._results2014); }
            this.http.get(this._results2014Json, this.headers)
                     .success((data) => deferred.resolve(data));
            return deferred.promise;
        }

        getResultsAsync(year: string): ng.IPromise<string[]> {
            var deferred = this.q.defer();
            switch (year)
            {
                case "2009":
                    if (this._results2009 !== null) { deferred.resolve(this._results2009); }
                    this.http.get(this._results2009Json, this.headers)
                     .success((data) => deferred.resolve(data));
                    break;
                case "2010":
                    if (this._results2010 !== null) { deferred.resolve(this._results2010); }
                    this.http.get(this._results2010Json, this.headers)
                     .success((data) => deferred.resolve(data));
                    break;
                case "2014":
                    if (this._results2014 !== null) { deferred.resolve(this._results2014); }
                    this.http.get(this._results2010Json, this.headers)
                     .success((data) => deferred.resolve(data));
                    break;
                case "2015":    
                    if (this._results2015 !== null) { deferred.resolve(this._results2015); }
                    this.http.get(this._results2015Json, this.headers)
                     .success((data) => deferred.resolve(data));
                    break;
            }
            return deferred.promise;
        }
        
        get2010LocalIssuesData(callback: (ev: Event) => any) {
            this.http.get(this._localIssues2010, this.headers).success(callback);
        }

        get2015LocalIssuesData(callback: (ev: Event) => any) {
            this.http.get(this._localIssues2015, this.headers).success(callback);
        }

        getCasteDistribution(callback: (ev: Event) => any) {
            this.http.get(this._casteDistribution, this.headers).success(callback);
        }

        getCasteCategoryDistribution(callback: (ev: Event) => any) {
            this.http.get(this.casteCategoryDistribution, this.headers).success(callback);
        }

        getCandidateData(callback: (ev: Event) => any) {
            this.http.get(this.candidateInfo, this.headers).success(callback);
        }

        getVIPConstituencies(callback: (ev: Event) => any) {
            this.http.get(this.vipConstituencies, this.headers).success(callback);
        }

        get2015Predictions(callback: (ev: Event) => any) {
            this.http.get(this._predictions2015, this.headers).success(callback);
        }        
        //Note: Use linq.js to query within the data fetched
    }
}

