/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var TweetJudgingCtrl = (function () {
        function TweetJudgingCtrl($scope, $http, $q, $compile) {
            this.message = "hello";
            this.gender = "";
            this.judgement = "";
            this.tweetCategory = "";
            this.showInput = true;
            this.allHandles = [];
            this.allHandlesText = "";
            this.judge = "";
            this.tweetInclination = [];
            this.genderSelected = "";
            this.partySelected = "";
            this.successMesg = "";
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.compile = $compile;
            this.loadinfodiv();
            this.init();
        }
        TweetJudgingCtrl.prototype.loadinfodiv = function () {
            var div = document.getElementById("judgerInfo");
            var input = angular.element('<div> Your Name: <input type="text" ng-model="vMain.judge" ng-show="vMain.showInput"> <span ng-show="!vMain.showInput"> {{vMain.judge}} </span>  <a href="" ng-click="vMain.submit()">Submit</a> <span> {{vMain.successMesg}}</span> </div>');
            var compileFn = this.compile(input);
            compileFn(this.scope);
            var divElement = angular.element(div);
            divElement.append(input);
        };
        TweetJudgingCtrl.prototype.init = function () {
            var _this = this;
            var deferred = this.q.defer();
            var pPage = this.http.get("./allHandles.txt").success(function (data) { return deferred.resolve(data); });
            pPage.then(function (response) { return _this.loadAllHandles(response.data); });
        };
        TweetJudgingCtrl.prototype.loadAllHandles = function (data) {
            var allText = data;
            this.allHandles = allText.match(/[^\r\n]+/g);
            this.loadNext(this.allHandles[0]);
        };
        TweetJudgingCtrl.prototype.checkAndLoadNext = function () {
            var _this = this;
            this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=true&judge=" + this.judge)
                .then(function (response) { return _this.skipAndLoadNext(response.data); });
        };
        TweetJudgingCtrl.prototype.skipAndLoadNext = function (data) {
            var lastScreenNameForJudge = data;
            var lastScreenNameMatched = false;
            for (var handle in this.allHandles) {
                if (lastScreenNameMatched == true) {
                    this.loadNext(handle);
                    break;
                }
                if (lastScreenNameForJudge == handle) {
                    lastScreenNameMatched = true;
                }
            }
        };
        TweetJudgingCtrl.prototype.loadNext = function (screenName) {
            var _this = this;
            var deferred = this.q.defer();
            var pPage = this.http.get("./" + screenName + ".html").success(function (data) { return deferred.resolve(data); });
            pPage.then(function (response) { return _this.divLoader(response.data); });
        };
        TweetJudgingCtrl.prototype.divLoader = function (data) {
            var html = data;
            var div = document.getElementById("twitterPage");
            var input = angular.element(data);
            this.compile(input)(this.scope);
            var divElement = angular.element(div);
            divElement.append(input);
            this.resetJudgements();
        };
        TweetJudgingCtrl.prototype.resetJudgements = function () {
            this.tweetCategory = "";
            this.gender = "";
            this.judgement = "";
        };
        TweetJudgingCtrl.prototype.addElementsToProfileCard = function () {
            var profileDiv = document.getElementsByClassName("ProfileCardMini");
        };
        TweetJudgingCtrl.prototype.addElementsToEachTweet = function () {
            var tweetStreamDiv = document.getElementById("stream-items-id");
            var tweetNodes = Enumerable.From(tweetStreamDiv.childNodes);
            tweetNodes.ForEach(function (node, i) {
            });
        };
        TweetJudgingCtrl.prototype.submit = function () {
            var _this = this;
            this.gender = this.genderSelected;
            this.judgement = this.partySelected;
            this.tweetInclination.forEach(function (element) { return _this.tweetCategory = _this.tweetCategory + element; });
            this.submitJudgement(this.judge, this.gender, this.judgement, this.tweetCategory);
            if (this.judge.length > 2) {
                this.showInput = false;
            }
        };
        TweetJudgingCtrl.prototype.submitJudgement = function (judge, gender, judgement, tweetCategory) {
            var _this = this;
            this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?judge=" + judge
                + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory)
                .then(function () { return _this.displaySuccess(); });
        };
        TweetJudgingCtrl.prototype.displaySuccess = function () {
            this.successMesg = "Successfully Submitted. Next profile loaded";
        };
        return TweetJudgingCtrl;
    })();
    Controllers.TweetJudgingCtrl = TweetJudgingCtrl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
angular.module('controllers', []).controller(Controllers);
/// <reference path="./reference.ts" />
angular.module('TweetJudging', ['controllers']);
//grunt-start
/// <reference path="controllers/controller.ts" />
/// <reference path="vendor.d.ts" />
//grunt-end
/// <reference path="controllers/controllers.ts" />
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map