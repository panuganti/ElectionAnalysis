/// <reference path="../reference.ts" />
module Controllers {
	export class TweetJudging {
        private http: ng.IHttpService;
        private scope: ng.IScope;
		
		constructor($scope, $http) {
            $scope.vMain = this;
            this.scope = $scope;
            this.http = $http;			
		}
	}	
}