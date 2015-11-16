/// <reference path="../reference.ts" />
module Controllers {
    export class MainCtrl {
        private http: ng.IHttpService;
        private scope: ng.IScope;
        private q: ng.IQService;
        private timeout: ng.ITimeoutService;


        private headers: any = { 'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ' }
        private json: string = "./trump_summary.json";
        private tweets_json: string = "./trump_tweets.json";
        private trumpjson: string = "./trump_summary.json";
        private tweets_trump: string = "./trump_tweets.json";
        private clintonjson: string = "./clinton_summary.json";
        private tweets_clinton: string = "./trump_tweets.json";
        private sandersjson: string = "./sanders_summary.json";
        private tweets_sanders: string = "./trump_tweets.json";
        private obamajson: string = "./obama_summary.json";
        private tweets_obama: string = "./trump_tweets.json";

        candidate: string = "trump";
        candidates: string[] = ["trump", "clinton", "etisalat"];
        tweets: Tweet[] = [];
        data: ChartData[] = [];
        vizData: google.visualization.DataTable;

        loadData() : ng.IPromise<any> {
            let deferred = this.q.defer();
            switch(this.candidate)
            {
                case "trump" : this.json = this.trumpjson; break;
                case "clinton": this.json = this.clintonjson; break;
                case "sanders": this.json = this.sandersjson; break;
                case "obama": this.json = this.obamajson; break;
                default: this.json = this.trumpjson;
            }
            this.http.get(this.json, this.headers)
                .success((data) => deferred.resolve(data));
            return deferred.promise;
        }

        loadTweets() : ng.IPromise<any> {
            let deferred = this.q.defer();
            switch(this.candidate)
            {
                case "trump" : this.tweets_json = this.tweets_trump; break;
                case "clinton": this.tweets_json = this.tweets_clinton; break;
                case "sanders": this.tweets_json = this.tweets_sanders; break;
                case "obama": this.tweets_json = this.tweets_obama; break;
                default: this.tweets_json = this.tweets_trump;
            }
            this.http.get(this.tweets_json, this.headers)
                .success((data) => deferred.resolve(data));
            return deferred.promise;
        }



        constructor($scope, $http, $q, $timeout) {
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            this.drawChart();
        }

        drawChart() {
            var options = {
                title: 'Sentiment Trend',
                curveType: 'function',
                legend: { position: 'bottom' }
            };

            var chart = new google.visualization.LineChart(document.getElementById('LineChart'));
            this.candidateSelectionChanged();
            chart.draw(this.vizData, options);
        }

        prepareData(x) {
            this.vizData = new google.visualization.DataTable();
            this.vizData.addColumn('string', 'Date');
            this.vizData.addColumn('number', 'Total');
            this.vizData.addColumn('number', 'Positive');
            this.vizData.addColumn('number', 'Negative');
            this.data = x;
            this.data.forEach(y => {
                this.vizData.addRow([y.Time, y.Total, y.Positive, y.Negative]);
            });
        }
        
        prepareTweets(x) {
            this.tweets = x;
        }

        candidateSelectionChanged() {
            this.loadData().then((x) => this.prepareData(x));
            this.loadTweets().then((x) => this.prepareTweets(x));
        }
    }

    interface ChartData {
        Time: string;
        Total: number;
        Positive: number;
        Negative: number;
    }

    interface Tweet {
        Text: string;
        Sentiment: string;
        Time: string;
    }
}
