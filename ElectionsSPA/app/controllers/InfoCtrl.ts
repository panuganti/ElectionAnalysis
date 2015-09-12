/// <reference path="../reference.ts" />
module Controllers {
        export class InfoCtrl {
                // Angular
                private http: ng.IHttpService;
                private scope: ng.IScope;
                private q: ng.IQService;
                private timeout: ng.ITimeoutService;
		
                // Services
                private dataloader: DataLoader;
                private info: Models.InfoData;
                private infoDiv: HTMLElement;

                //constructor() {}        
        
                constructor($scope, $http, $q, $timeout) {
                        $scope.vInfo = this;
                        this.scope = $scope;
                        this.http = $http;
                        this.q = $q;
                        this.timeout = $timeout;

                        this.dataloader = new DataLoader(this.http, this.q);
                        this.infoDiv = document.getElementById('info');
                }

                displayInfo(id: string) {
                        let p2014 = this.dataloader.getResultsAsync("2014");
                        let p2010 = this.dataloader.getResultsAsync("2010");
                        let p2009 = this.dataloader.getResultsAsync("2009");
                        let pR: ng.IPromise<any[]> = this.q.all([p2009, p2010, p2014]);
                        pR.then(([d1, d2, d3]) => this.loadResultsForAC(d1, d2, d3, id));
                }


                loadResultsForAC(d1, d2, d3, id) {
                        console.log('in load results');
                        let r2014: Models.Result[] = d1;
                        let r2010: Models.Result[] = d2;
                        let r2009: Models.Result[] = d3;
                        var en2014 = Enumerable.From(r2014);
                        var en2010 = Enumerable.From(r2010);
                        var en2009 = Enumerable.From(r2009);
                        var results2014 = en2014.First(t=> t.Id == id);
                        var results2010 = en2010.First(t=> t.Id == id);
                        var results2009 = en2009.First(t=> t.Id == id);
                        var title = results2014.Name;
                        this.info = new Models.InfoData(title, results2009, results2010, results2014);
                        this.setInfoDivVisibility("inline");
                }

                setInfoDivVisibility(display: string) {
                        this.infoDiv.style.display = display;
                }
        }
}