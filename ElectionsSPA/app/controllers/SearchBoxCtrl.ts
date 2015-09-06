module Controllers{
    export class SearchBoxCtrl {
        userQuery: string
        
        placeholder: string = "Search Constituency";
        constructor($scope){
            $scope.searchBox = this;
        }
    }
}