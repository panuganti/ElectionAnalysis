﻿/// <reference path="../reference.ts" />
module Controllers {
    export class DataLoader {
        private _acShapeFile: string = "json/Bihar.Assembly.10k.topo.json";
        private _allACsJson: string = "json/allACs.json";
        private _results2009: string = "json/results2009AcWise.json";
        private _results2010: string = "json/results2014AcWise.json";
        //private results2010: string = "json/results2010.json";
        private _results2014: string = "json/results2014AcWise.json";
        private _localIssues2015: string = "";
        private _localIssues2010: string = "";
        private _casteDistribution: string = "";
        private casteCategoryDistribution: string = "";
        private candidateInfo: string = "";
        private vipConstituencies: string = "";
        private _predictions2015: string = "";
        private _neighbors: string = "json/Neighbors.txt";

        private http: ng.IHttpService;
        private headers: any = {'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ'}

        constructor($http) {
            this.http = $http;
        }

        getColorsJson(callback: (ev: Event)=> any) {        
        }

        getACTopoShapeFile(callback: (ev: Event) => any) {            
            this.http.get(this._acShapeFile, this.headers).success(callback);
        _}

        getAllAssemblyConstituencies(callback: (ev: Event)=> any) {
            this.http.get(this._allACsJson, this.headers).success(callback);
        }

        get2010Results(callback: (ev: Event)=> any) {
            this.http.get(this._results2010, this.headers).success(callback);
        }

        get2014Results(callback: (ev: Event)=> any) {
            this.http.get(this._results2014, this.headers).success(callback);
        }

        get2010LocalIssuesData(callback: (ev: Event)=> any) {
            this.http.get(this._localIssues2010, this.headers).success(callback);
        }

        get2015LocalIssuesData(callback: (ev: Event)=> any) {
            this.http.get(this._localIssues2015, this.headers).success(callback);
        }

        getCasteDistribution(callback: (ev: Event)=> any) {
            this.http.get(this._casteDistribution, this.headers).success(callback);
        }

        getCasteCategoryDistribution(callback: (ev: Event)=> any) {
            this.http.get(this.casteCategoryDistribution, this.headers).success(callback);
        }

        getCandidateData(callback: (ev: Event)=> any) {
            this.http.get(this.candidateInfo, this.headers).success(callback);
        }

        getVIPConstituencies(callback: (ev: Event)=> any) {
            this.http.get(this.vipConstituencies, this.headers).success(callback);
        }

        get2015Predictions(callback: (ev: Event)=> any) {
            this.http.get(this._predictions2015, this.headers).success(callback);
        }        
        //Note: Use linq.js to query within the data fetched
    }
}

