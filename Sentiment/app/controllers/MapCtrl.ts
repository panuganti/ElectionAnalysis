/// <reference path="../reference.ts" />
module Controllers {
    export class MainCtrl {
        private http: ng.IHttpService;
        private scope: ng.IScope;
        private q: ng.IQService;
        private timeout: ng.ITimeoutService;
                
        constructor($scope, $http, $q, $timeout) {
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
        }                               
    }    
}
