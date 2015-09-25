/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var TweetJudgingCtrl = (function () {
        function TweetJudgingCtrl($scope, $http, $q, $compile) {
            this.message = "hello";
            this.judge = "";
            this.gender = "";
            this.judgement = "";
            this.tweetCategory = "";
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.compile = $compile;
            this.loadinfodiv();
            this.initialize();
        }
        TweetJudgingCtrl.prototype.loadinfodiv = function () {
            var div = document.getElementById("judgerInfo");
            return {
                link: function (scope, element, attrs) {
                    element.replaceWith(this.compile('Your Name: <input type="text" ng-model="vMain.judge"> {{vMain.judge}} <a href="" ng-click="vMain.submit()">Submit</a>')(scope));
                }
            };
        };
        TweetJudgingCtrl.prototype.initialize = function () {
            var _this = this;
            var deferred = this.q.defer();
            var pPage = this.http.get("./panuganti.html").success(function (data) { return deferred.resolve(data); });
            pPage.then(function (response) { return _this.divLoader(response.data); });
        };
        TweetJudgingCtrl.prototype.divLoader = function (data) {
            var html = data;
            var div = document.getElementById("twitterPage");
            div.innerHTML = html;
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
            this.submitJudgement(this.judge, this.gender, this.judgement, "hello");
        };
        TweetJudgingCtrl.prototype.submitJudgement = function (judge, gender, judgement, tweetCategory) {
            var _this = this;
            this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?judge=" + judge
                + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory)
                .then(function () { return _this.displaySuccess(); });
        };
        TweetJudgingCtrl.prototype.displaySuccess = function () { };
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