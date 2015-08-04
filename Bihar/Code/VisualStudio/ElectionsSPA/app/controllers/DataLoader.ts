module Controllers {
    export class DataLoader {
        private allACsJson: string = "";
        private results2010: string = "";
        private results2014: string = "";
        private localIssues2015: string = "";
        private localIssues2010: string = "";
        private casteDistribution: string = "";
        private casteCategoryDistribution: string = "";
        private candidateInfo: string = "";
        private vipConstituencies: string = "";
        private predictions2015: string = "";

        private http: ng.IHttpService;
        private headers: any = {'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ'}

        constructor($http) {
            this.http = $http;
        }

        getAllAssemblyConstituencies(url: string, callback) {
            this.http.get(allACsJson, headers: headers).success(callback);
        }

        get2010Results(url: string, callback) {
            this.http.get(results2010, headers: headers).success(callback);
        }

        get2014Results(url: string, callback) {
            this.http.get(results2014, headers: headers).success(callback);
        }

        get2010LocalIssuesData(url: string, callback) {
            this.http.get(localIssues2010, headers: headers).success(callback);
        }

        get2015LocalIssuesData(url: string, callback) {
            this.http.get(localIssues2015, headers: headers).success(callback);
        }

        getCasteDistribution(url: string, callback) {
            this.http.get(casteDistribution, headers: headers).success(callback);
        }

        getCasteCategoryDistribution(url: string, callback) {
            this.http.get(casteCategoryDistribution, headers: headers).success(callback);
        }

        getCandidateData(url: string, callback) {
            this.http.get(candidateInfo, headers: headers).success(callback);
        }

        getVIPConstituencies(url: string, callback) {
            this.http.get(vipConstituencies, headers: headers).success(callback);
        }

        get2015Predictions(url: string, callback) {
            this.http.get(predictions2015, headers: headers).success(callback);
        }
        //Note: Use linq.js to query within the data fetched
    }
}

