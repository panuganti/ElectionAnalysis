/// <reference path="../reference.ts" />
module Controllers {
  export class TweetJudgingCtrl {
    private http: ng.IHttpService;
    private scope: ng.IScope;
    private q: ng.IQService;
    private compile: ng.ICompileService;
    private timeout: ng.ITimeoutService;  
    message = "hello";
    gender = "";
    judgement = "";
    tweetCategory = "";
    showInput: boolean = true;
    allHandles = [];
    allHandlesText: string = "";  
    
    parties = ["NotRelevant", "JustReporting", "ProDemocratic", "ProRepublican", "AntiDemocratic", "AntiRepublican", "CantSay", "Spam"];
    NotRelevant = "NotRelevant";
    genders = ["Male", "Female", "FakePic", "GenericPic"];  
      
    // Responses
    judge = "";
    profile = "";
    tweetInclination = [];
    genderSelected = "";
    partySelected = "";  
    successMesg = "";  
    
    // Show/Hide RecordJugement
    showRecordJudgement = false;
    overallJudementSelection = false;
    genderJudementSelection = false;
    tweetJugementSelection = false;  
      
    constructor($scope, $http, $q, $compile, $timeout) {
      $scope.vMain = this;
      this.scope = $scope;
      this.http = $http;
      this.q = $q;
      this.timeout = $timeout;  
      this.compile = $compile;
      this.loadinfodiv();
      this.init();
    }

    loadinfodiv() {
        let div = document.getElementById("judgerInfo");
        var input = angular.element('<div> Your Name: <input type="text" ng-model="vMain.judge" ng-show="vMain.showInput"> <span ng-show="!vMain.showInput"> {{vMain.judge}} </span>  <a href="" ng-click="vMain.submit()" ng-show="vMain.showInput">Submit</a> <a href="" ng-click="vMain.recordJudgement()" ng-show="!vMain.showInput &&vMain.showRecordJudgement">RecordJudgement</a>  <span ng-show="!vMain.showInput"> {{vMain.successMesg}}</span> </div>');
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
    }
     
    overallJudgementSelected()
    {
        this.overallJudementSelection = true;
        this.checkIfRecordJudgementCanBeShown();
    }  
    
    genderJudgementSelected() 
    {
        this.genderJudementSelection = true
        this.checkIfRecordJudgementCanBeShown();
    }  
    
    tweetJudgementSelected() 
    {
        this.tweetJugementSelection = true;
        this.checkIfRecordJudgementCanBeShown();
    }  
      
    checkIfRecordJudgementCanBeShown() {
        if (this.overallJudementSelection && this.genderJudementSelection && this.tweetJugementSelection)
        {
            this.showRecordJudgement = true;
        }    
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
        for (let handle of this.allHandles)
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
      var pPage = this.http.get("./profiles/" + screenName + ".html").success((data) => deferred.resolve(data));
      pPage.then((response) => this.divLoader(response.data));
      this.profile = screenName;
    }

    divLoader(data) {
      let html: string = data;
      let div = document.getElementById("twitterPage");
      div.innerHTML = html;  
      //var input = angular.element(data);
      this.compile(div)(this.scope);
      //var divElement = angular.element(div);        
      //divElement.add(input);  
      //divElement.append(input);
        
      // reset the values for this div
      this.resetJudgements();
    }
    
    resetJudgements()
    {
        this.tweetCategory = "";
        this.gender = "";
        this.judgement = "";
        this.tweetInclination = [];
        this.showRecordJudgement = false;
        this.genderSelected = "reset";
        this.partySelected = "reset";         
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
        if (this.judge.length > 2) {
            this.showInput = false;
        }   
        this.checkAndLoadNext();
    }
      
    recordJudgement() {
        this.gender = this.genderSelected;
        this.judgement = this.partySelected;
        for (var i = 0; i < this.tweetInclination.length; i++)
        {
            if (this.tweetInclination[i] == null)
            { this.tweetInclination[i] = "NotJudged";}    
            this.tweetCategory = this.tweetCategory + ";" + this.tweetInclination[i];            
        }    
        this.submitJudgement(this.judge, this.profile, this.gender, this.judgement, this.tweetCategory);        
    }

    submitJudgement(judge: string, profile: string, gender: string, judgement: string, tweetCategory: string) {
      let deferred = this.q.defer();
        var submitJudgement = this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?getJudgements=false&judge=" + judge
            + "&profile=" + profile + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory).success((data) => deferred.resolve(data));
          submitJudgement.then((response) => this.displaySuccess(response.data));
        this.genderSelected = "reset";
        this.partySelected = "reset";
         this.skipAndLoadNext(this.profile);      
  }
    
    displaySuccess(data)
    {
        this.successMesg = "Success...";
    }  
  }
}
