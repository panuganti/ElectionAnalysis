module Controllers {
    export class MainControl {
        message = "asdf";
        constructor($scope,logService:LogService){
            $scope.vm = this;
            logService.log('Some log');
        }
    }
}