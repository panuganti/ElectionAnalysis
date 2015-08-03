/// <reference path="../Scripts/typings/googlemaps/google.maps.d.ts"/>
/// <reference path="../Scripts/typings/jquery/jquery.d.ts"/>
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />

module BiharElectionsController {
    export class Controller {

        constructor($scope) {
            //$scope = this;
            $scope.message = "F";
        }

        testAngular($scope) {
            //$scope = this;
            $scope.message = "G";            
        }
        onConstituencyClicked(ac: number) {

        }

        onZoom() {
        }

        onLocalIssueClicked(localIssue: number) {
        }

        onCandidateClick(candidateId: number) {
        }

        onCasteClick(casteId: number) {
        }

        onCasteCategoryClick(casteCategory: number) {
        }

        onRegionClick(regionId: number) {
        }

        onDistrictClick(districtId: number) {
        }
    }
}

