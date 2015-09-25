/// <reference path="../reference.ts" />
angular.module('controllers', []).controller(Controllers);
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var TweetJudging = (function () {
        function TweetJudging($scope, $http) {
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;
        }
        return TweetJudging;
    })();
    Controllers.TweetJudging = TweetJudging;
})(Controllers || (Controllers = {}));
/// <reference path="./reference.ts" />
angular.module('TweetJudging', ['controllers']);
/// <reference path="controllers/controllers.ts" />
//grunt-start
/// <reference path="controllers/controller.ts" />
/// <reference path="vendor.d.ts" />
//grunt-end
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map