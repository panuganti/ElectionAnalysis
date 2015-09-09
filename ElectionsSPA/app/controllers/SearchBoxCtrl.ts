/// <reference path="../reference.ts" />

module Controllers {
    export class SearchBoxCtrl {
        private scope: ng.IScope;
        userQuery: string;
        dropDownList: AC[];
        placeholder: string = "Search Constituency";
        dataloader: DataLoader;


        constructor($scope, $http, $q) {
            $scope.searchBox = this;
            this.scope = $scope;
            this.dataloader = new DataLoader($http, $q);
        }

        userQueryChanged() {
            let pACs = this.dataloader.getAllAssemblyConstituencies();
            pACs.then((data) => this.displayTopSearchResults(data));
        }
        
        displayTopSearchResults(data:AC[]) {
            let acs: AC[] = data;
            let userQ = this.userQuery.trim();
            let en = Enumerable.From(acs);
            
            if (userQ.length == 0) { this.dropDownList = []; return; }
            let candidates = en.Where(t=> t.Name.indexOf(userQ, 0) === 0).Take(5);
            this.dropDownList = candidates.ToArray();
        }
    }
    
    export interface AC {
        Id: string,
        Name: string
    }
}