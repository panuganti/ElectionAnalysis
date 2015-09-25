/// <reference path="../reference.ts" />
module Controllers {
  export class TweetJudgingCtrl {
    private http: ng.IHttpService;
    private scope: ng.IScope;
    private q: ng.IQService;
    message = "hello";
    judge = "";
    gender = "";
    judgement = "";
    tweetCategory = "";

    constructor($scope, $http, $q) {
      $scope.vMain = this;
      this.scope = $scope;
      this.http = $http;
      this.q = $q;
      this.initialize();
    }

    initialize() {
      let deferred = this.q.defer();
      var pPage = this.http.get("./panuganti.html").success((data) => deferred.resolve(data));
      pPage.then((response) => this.divLoader(response.data));
    }

    divLoader(data) {
      let html: string = data;
      let div = document.getElementById("twitterPage");
      div.innerHTML = html;
      //this.scope.$apply();
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
      this.submitJudgement(this.judge, this.gender, this.judgement, "hello");
    }

    submitJudgement(judge: string, gender: string, judgement: string, tweetCategory: string) {
      this.http.get("https://script.google.com/macros/s/AKfycbz2ZMnHuSR4GmTjsuIo6cmh433RRpPRH7TwMaJhbAUr/dev?judge=" + judge 
      + "&gender=" + gender + "&judgement=" + judgement + "&tweetCategory=" + tweetCategory)
      .then(() => this.displaySuccess());
    }

    displaySuccess()
    { }
  }
}