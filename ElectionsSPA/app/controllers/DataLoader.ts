module Controllers {
    export class DataLoader {
        private acShapeFile: string = "json/Bihar.Assembly.10k.topo.json";
        private allACsJson: string = "json/allACs.json";
        private results2009: string = "json/results2009AcWise.json";
        //private results2010: string = "json/results2010AcWise.json";
        private results2010: string = "json/results2010.json";
        private results2014: string = "json/results2014AcWise.json";
        private localIssues2015: string = "";
        private localIssues2010: string = "";
        private casteDistribution: string = "";
        private casteCategoryDistribution: string = "";
        private candidateInfo: string = "";
        private vipConstituencies: string = "";
        private predictions2015: string = "";
        private neighbors: string = "json/Neighbors.txt";

        private http: ng.IHttpService;
        private headers: any = {'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ'}

        constructor($http) {
            this.http = $http;
        }

        getColorsJson(callback: (ev: Event)=> any) {        
        }

        getACTopoShapeFile(callback: (ev: Event) => any) {            
            this.http.get(this.acShapeFile, this.headers).success(callback);
        }

        getAllAssemblyConstituencies(callback: (ev: Event)=> any) {
            this.http.get(this.allACsJson, this.headers).success(callback);
        }

        get2010Results(callback: (ev: Event)=> any) {
            this.http.get(this.results2010, this.headers).success(callback);
        }

        get2014Results(callback: (ev: Event)=> any) {
            this.http.get(this.results2014, this.headers).success(callback);
        }

        get2010LocalIssuesData(callback: (ev: Event)=> any) {
            this.http.get(this.localIssues2010, this.headers).success(callback);
        }

        get2015LocalIssuesData(callback: (ev: Event)=> any) {
            this.http.get(this.localIssues2015, this.headers).success(callback);
        }

        getCasteDistribution(callback: (ev: Event)=> any) {
            this.http.get(this.casteDistribution, this.headers).success(callback);
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
            this.http.get(this.predictions2015, this.headers).success(callback);
        }        
        //Note: Use linq.js to query within the data fetched
    }
}

