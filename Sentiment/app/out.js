/// <reference path="../reference.ts" />
var services = angular.module('services', []);
/// <reference path="../reference.ts" />
var directives = angular.module('directives', []);
var testme;
(function (testme) {
    testme.html = '<div>Hey wassup yo!</div>';
})(testme || (testme = {}));
var Controllers;
(function (Controllers) {
    var MainControl = (function () {
        function MainControl($scope, logService) {
            this.message = "asdf";
            $scope.vm = this;
            logService.log('Some log');
        }
        return MainControl;
    })();
    Controllers.MainControl = MainControl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var MainCtrl = (function () {
        function MainCtrl($scope, $http, $q, $timeout) {
            this.headers = { 'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ' };
            this.json = "./trump_summary.json";
            this.tweets_json = "./trump_tweets.json";
            this.trumpjson = "./trump_summary.json";
            this.tweets_trump = "./trump_tweets.json";
            this.clintonjson = "./clinton_summary.json";
            this.tweets_clinton = "./trump_tweets.json";
            this.sandersjson = "./sanders_summary.json";
            this.tweets_sanders = "./trump_tweets.json";
            this.obamajson = "./obama_summary.json";
            this.tweets_obama = "./trump_tweets.json";
            this.candidate = "trump";
            this.candidates = ["trump", "clinton", "etisalat"];
            this.tweets = [];
            this.data = [];
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            this.drawChart();
        }
        MainCtrl.prototype.loadData = function () {
            var deferred = this.q.defer();
            switch (this.candidate) {
                case "trump":
                    this.json = this.trumpjson;
                    break;
                case "clinton":
                    this.json = this.clintonjson;
                    break;
                case "sanders":
                    this.json = this.sandersjson;
                    break;
                case "obama":
                    this.json = this.obamajson;
                    break;
                default: this.json = this.trumpjson;
            }
            this.http.get(this.json, this.headers)
                .success(function (data) { return deferred.resolve(data); });
            return deferred.promise;
        };
        MainCtrl.prototype.loadTweets = function () {
            var deferred = this.q.defer();
            switch (this.candidate) {
                case "trump":
                    this.tweets_json = this.tweets_trump;
                    break;
                case "clinton":
                    this.tweets_json = this.tweets_clinton;
                    break;
                case "sanders":
                    this.tweets_json = this.tweets_sanders;
                    break;
                case "obama":
                    this.tweets_json = this.tweets_obama;
                    break;
                default: this.tweets_json = this.tweets_trump;
            }
            this.http.get(this.tweets_json, this.headers)
                .success(function (data) { return deferred.resolve(data); });
            return deferred.promise;
        };
        MainCtrl.prototype.drawChart = function () {
            var options = {
                title: 'Sentiment Trend',
                curveType: 'function',
                legend: { position: 'bottom' }
            };
            var chart = new google.visualization.LineChart(document.getElementById('LineChart'));
            this.candidateSelectionChanged();
            chart.draw(this.vizData, options);
        };
        MainCtrl.prototype.prepareData = function (x) {
            var _this = this;
            this.vizData = new google.visualization.DataTable();
            this.vizData.addColumn('string', 'Date');
            this.vizData.addColumn('number', 'Total');
            this.vizData.addColumn('number', 'Positive');
            this.vizData.addColumn('number', 'Negative');
            this.data = x;
            this.data.forEach(function (y) {
                _this.vizData.addRow([y.Time, y.Total, y.Positive, y.Negative]);
            });
        };
        MainCtrl.prototype.prepareTweets = function (x) {
            this.tweets = x;
        };
        MainCtrl.prototype.candidateSelectionChanged = function () {
            var _this = this;
            this.loadData().then(function (x) { return _this.prepareData(x); });
            this.loadTweets().then(function (x) { return _this.prepareTweets(x); });
        };
        return MainCtrl;
    })();
    Controllers.MainCtrl = MainCtrl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
directives.directive('testme', function () {
    return {
        restrict: 'EAC',
        template: testme.html
    };
});
var LogService = (function () {
    function LogService() {
    }
    LogService.prototype.log = function (msg) {
        console.log(msg);
    };
    return LogService;
})();
services.service('logService', LogService);
/// <reference path="../reference.ts" />
angular.module('controllers', []).controller(Controllers);
/// <reference path="./reference.ts" />
angular.module('SentimentTracker', ['controllers', 'services', 'directives']);
/// <reference path="services/services.ts" />
/// <reference path="directives/directives.ts" />
/// <reference path="directives/testme.html.ts" />
/// <reference path="controllers/MainController.ts" />
/// <reference path="controllers/MainCtrl.ts" />
/// <reference path="directives/testme.ts" />
/// <reference path="services/LogService.ts" />
/// <reference path="vendor.d.ts" />
/// <reference path="controllers/controllers.ts" />
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map