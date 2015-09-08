/// <reference path="../reference.ts" />

module Controllers {
    export class SearchBoxCtrl {
        private scope: ng.IScope;
        userQuery: string;
        dropDownList: string[];
        placeholder: string = "Search Constituency";


        constructor($scope) {
            $scope.searchBox = this;
            this.scope = $scope;

        }

        getTopChoices() {
            console.log('hello' + this.userQuery);
            //this.dropDownList = ["hello", "world"];
        }
    }
}