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
            this.parties = ["NotRelevant", "ProDemocratic", "ProRepublican", "AntiDemocratic", "AntiRepublican", "Spam"];
            this.NotRelevant = "NotRelevant";
            this.genders = ["Male", "Female", "FakePic", "GenericPic"];
            this.judge = "";
            this.profile = "";
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
            var input = angular.element('<div> Your Name: <input type="text" ng-model="vMain.judge" ng-show="vMain.showInput"> <span ng-show="!vMain.showInput"> {{vMain.judge}} </span>  <a href="" ng-click="vMain.submit()" ng-show="vMain.showInput">Submit</a> <a href="" ng-click="vMain.recordJudgement()" ng-show="!vMain.showInput">RecordJudgement</a>  <span ng-show="!vMain.showInput"> {{vMain.successMesg}}</span> </div>');
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
        };
        TweetJudgingCtrl.prototype.checkAndLoadNext = function () {
            var _this = this;
            this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=true&judge=" + this.judge)
                .then(function (response) { return _this.skipAndLoadNext(response.data); });
        };
        TweetJudgingCtrl.prototype.skipAndLoadNext = function (data) {
            var lastScreenNameForJudge = data;
            if (lastScreenNameForJudge == "none") {
                this.loadNext(this.allHandles[0]);
                return;
            }
            var lastScreenNameMatched = false;
            for (var _i = 0, _a = this.allHandles; _i < _a.length; _i++) {
                var handle = _a[_i];
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
            var pPage = this.http.get("./profiles/" + screenName + ".html").success(function (data) { return deferred.resolve(data); });
            pPage.then(function (response) { return _this.divLoader(response.data); });
            this.profile = screenName;
        };
        TweetJudgingCtrl.prototype.divLoader = function (data) {
            var html = data;
            var div = document.getElementById("twitterPage");
            div.innerHTML = html;
            this.compile(div)(this.scope);
            this.resetJudgements();
        };
        TweetJudgingCtrl.prototype.resetJudgements = function () {
            this.tweetCategory = "";
            this.gender = "";
            this.judgement = "";
            this.tweetInclination = [];
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
            if (this.judge.length > 2) {
                this.showInput = false;
            }
            this.checkAndLoadNext();
        };
        TweetJudgingCtrl.prototype.recordJudgement = function () {
            this.gender = this.genderSelected;
            this.judgement = this.partySelected;
            for (var i = 0; i < this.tweetInclination.length; i++) {
                if (this.tweetInclination[i] == null) {
                    this.tweetInclination[i] = "NotJudged";
                }
                this.tweetCategory = this.tweetCategory + ";" + this.tweetInclination[i];
            }
            this.submitJudgement(this.judge, this.profile, this.gender, this.judgement, this.tweetCategory);
        };
        TweetJudgingCtrl.prototype.submitJudgement = function (judge, profile, gender, judgement, tweetCategory) {
            var _this = this;
            var deferred = this.q.defer();
            var submitJudgement = this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=false&judge=" + judge
                + "&profile=" + profile + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory).success(function (data) { return deferred.resolve(data); });
            submitJudgement.then(function (response) { return _this.displaySuccess(response.data); });
            this.skipAndLoadNext(this.profile);
        };
        TweetJudgingCtrl.prototype.displaySuccess = function (data) {
            this.successMesg = "Success...";
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