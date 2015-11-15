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
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
        }
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
angular.module('ElectionVisualization', ['controllers', 'services', 'directives']);
/// <reference path="services/services.ts" />
/// <reference path="directives/directives.ts" />
/// <reference path="directives/testme.html.ts" />
/// <reference path="controllers/MainController.ts" />
/// <reference path="controllers/MapCtrl.ts" />
/// <reference path="directives/testme.ts" />
/// <reference path="services/LogService.ts" />
/// <reference path="vendor.d.ts" />
/// <reference path="controllers/controllers.ts" />
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map