/// <reference path="../reference.ts" />
module Controllers {
  export class TweetJudgingCtrl {
    private http: ng.IHttpService;
    private scope: ng.IScope;
    private q: ng.IQService;
    private compile: ng.ICompileService;
    message = "hello";
    gender = "";
    judgement = "";
    tweetCategory = "";
    showInput: boolean = true;
    allHandles = [];
    allHandlesText: string = "";  
    
    // Responses
    judge = "";
    profile = "";
    tweetInclination = [];
    genderSelected = "";
    partySelected = "";  
    
    successMesg = "";  

    constructor($scope, $http, $q, $compile) {
      $scope.vMain = this;
      this.scope = $scope;
      this.http = $http;
      this.q = $q;
      this.compile = $compile;
      this.loadinfodiv();
      this.init();
    }

    loadinfodiv() {
        let div = document.getElementById("judgerInfo");
        var input = angular.element('<div> Your Name: <input type="text" ng-model="vMain.judge" ng-show="vMain.showInput"> <span ng-show="!vMain.showInput"> {{vMain.judge}} </span>  <a href="" ng-click="vMain.submit()">Submit</a> <span> {{vMain.successMesg}}</span> </div>');
        var compileFn = this.compile(input);
        compileFn(this.scope);
        var divElement = angular.element(div);
        divElement.append(input);
    }

    init() {
      let deferred = this.q.defer();
      var pPage = this.http.get("./allHandles.txt").success((data) => deferred.resolve(data));
      pPage.then((response) => this.loadAllHandles(response.data));
    }  
      
    loadAllHandles(data) {
        var allText: string = data;
        this.allHandles = allText.match(/[^\r\n]+/g);
        // TODO: Check until what point judge has judged
        this.loadNext(this.allHandles[0]);
     }
    
    checkAndLoadNext() {
        this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=true&judge=" + this.judge)
        .then((response) => this.skipAndLoadNext(response.data));   
    }  
      
    skipAndLoadNext(data: any) {
        let lastScreenNameForJudge = data;
        if (lastScreenNameForJudge == "none") {
          this.loadNext(this.allHandles[0]);
          return;
        }
        let lastScreenNameMatched = false;
        for (var handle in this.allHandles)
        {
            if (lastScreenNameMatched == true) {
                this.loadNext(handle);
                break;
            }
            if (lastScreenNameForJudge == handle) { lastScreenNameMatched = true;}
        }    
    }
      
    loadNext(screenName: string) {
      let deferred = this.q.defer();
      var pPage = this.http.get("./" + screenName + ".html").success((data) => deferred.resolve(data));
      pPage.then((response) => this.divLoader(response.data));
      this.profile = screenName;
    }

    divLoader(data) {
      let html: string = data;
      let div = document.getElementById("twitterPage");
      var input = angular.element(data);
      this.compile(input)(this.scope);
      var divElement = angular.element(div);
      divElement.append(input);
        
      // reset the values for this div
      this.resetJudgements();
    }
    
    resetJudgements()
    {
        this.tweetCategory = "";
        this.gender = "";
        this.judgement = "";
    }  

    addElementsToProfileCard()
    { 
      let profileDiv = document.getElementsByClassName("ProfileCardMini");
    }

    addElementsToEachTweet()
    { 
      let tweetStreamDiv = document.getElementById("stream-items-id");
      var tweetNodes = Enumerable.From(tweetStreamDiv.childNodes);
      tweetNodes.ForEach((node, i) => {
        
      });
    }
    
    submit() {
        this.gender = this.genderSelected;
        this.judgement = this.partySelected;
        this.tweetInclination.forEach(element => this.tweetCategory = this.tweetCategory + element);
        this.submitJudgement(this.judge, this.profile, this.gender, this.judgement, this.tweetCategory);
        
        if (this.judge.length > 2) {
            this.showInput = false;
        }    
    }

    submitJudgement(judge: string, profile: string, gender: string, judgement: string, tweetCategory: string) {
      this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=false&judge=" + judge 
      + "&profile=" + profile + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory)
      .then(() => this.displaySuccess());
    }

    displaySuccess()
    { 
        this.successMesg = "Successfully Submitted. Next profile loaded";
    }      
  }
}
