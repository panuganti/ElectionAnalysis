/// <reference path="../reference.ts" />
var services = angular.module('services', []);
/// <reference path="../reference.ts" />
var directives = angular.module('directives', []);
var testme;
(function (testme) {
    testme.html = '<div>Hey wassup yo!</div>';
})(testme || (testme = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var AcStyleMap = (function () {
        function AcStyleMap() {
            this.defaultStyle = {
                strokeWeight: 1,
                fillOpacity: 0.9,
                strokeOpacity: 0.3,
                strokeColor: "white"
            };
            this.resultsSummary = {};
            this.initializeColorMap();
            this.initializeAlliance();
            this.initializeAllianceColorMap();
        }
        AcStyleMap.prototype.initializeColorMap = function () {
            this.colorMap = {};
            this.colorMap["bjp"] = "orange";
            this.colorMap["jdu"] = "lightgreen";
            this.colorMap["rjd"] = "darkgreen";
            this.colorMap["inc"] = "lightblue";
            this.colorMap["ljp"] = "yellow";
            this.colorMap["rlsp"] = "brown";
            this.colorMap["cpi"] = "darkred";
            this.colorMap["ind"] = "black";
            this.colorMap["jmm"] = "purple";
        };
        AcStyleMap.prototype.initializeAlliance = function () {
            this.allianceMap = {};
            this.allianceMap["bjp"] = "BJP+";
            this.allianceMap["ljp"] = "BJP+";
            this.allianceMap["rlsp"] = "BJP+";
            this.allianceMap["rslp"] = "BJP+";
            this.allianceMap["ham"] = "BJP+";
            this.allianceMap["rjd"] = "JP+";
            this.allianceMap["jdu"] = "JP+";
            this.allianceMap["sp"] = "JP+";
            this.allianceMap["inc"] = "JP+";
            this.allianceMap["cpi"] = "O";
            this.allianceMap["ind"] = "O";
            this.allianceMap["jmm"] = "O";
            this.allianceMap["ncp"] = "O";
            this.allianceMap["bsp"] = "O";
            this.allianceMap["others"] = "O";
        };
        AcStyleMap.prototype.initializeAllianceColorMap = function () {
            this.allianceColorMap = {};
            this.allianceColorMap["BJP+"] = "orange";
            this.allianceColorMap["JP+"] = "green";
            this.allianceColorMap["O"] = "black";
        };
        AcStyleMap.prototype.Generate2015StyleMaps = function (acResults) {
            var _this = this;
            var colorService = new ColorService();
            var en = Enumerable.From(acResults);
            var acStyleMaps = [];
            en.ForEach(function (element) {
                var votes = Enumerable.From(en.Where(function (t) { return t.Id == element.Id; }).First().Votes);
                var party = votes.First(function (t) { return t.Position == 1; }).Party;
                var alliance = _this.allianceMap[party];
                var partyColor = _this.allianceColorMap[alliance];
                var styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                styleMap.Style = {
                    strokeWeight: _this.defaultStyle.strokeWeight,
                    fillOpacity: _this.defaultStyle.fillOpacity,
                    strokeOpacity: _this.defaultStyle.strokeOpacity,
                    fillColor: partyColor
                };
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        };
        AcStyleMap.prototype.GenerateStyleMaps = function (acResults) {
            var _this = this;
            var colorService = new ColorService();
            var en = Enumerable.From(acResults);
            var acStyleMaps = [];
            en.ForEach(function (element) {
                var votes = Enumerable.From(en.Where(function (t) { return t.Id == element.Id; }).First().Votes);
                var party = votes.First(function (t) { return t.Position == 1; }).Party;
                var alliance = _this.allianceMap[party];
                var partyColor = _this.allianceColorMap[alliance];
                var styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                var totalVotes = votes.Select(function (t) { return t.Votes; }).ToArray().reduce(function (a, b) { return a + b; });
                var margin = votes.First(function (t) { return t.Position == 1; }).Votes - votes.First(function (t) { return t.Position == 2; }).Votes;
                var marginPercent = Math.ceil((margin) * 100 / totalVotes);
                var color = colorService.getColor(partyColor, marginPercent, 1, 25);
                styleMap.Style = {
                    strokeWeight: _this.defaultStyle.strokeWeight,
                    fillOpacity: _this.defaultStyle.fillOpacity,
                    strokeOpacity: _this.defaultStyle.strokeOpacity,
                    fillColor: color
                };
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        };
        AcStyleMap.prototype.GenerateDisplayModeStyleMaps = function (stabilities) {
            var _this = this;
            var acStyleMaps = [];
            var en = Enumerable.From(stabilities);
            en.ForEach(function (element) {
                var styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                styleMap.Style = {
                    strokeWeight: _this.defaultStyle.strokeWeight,
                    fillOpacity: _this.defaultStyle.fillOpacity,
                    strokeOpacity: _this.defaultStyle.strokeOpacity,
                    fillColor: element.Stability ? _this.colorMap[element.Party] : _this.colorMap["ind"]
                };
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        };
        AcStyleMap.prototype.GenerateResultsSummary = function (acResults) {
            var resultsSummary = {};
            resultsSummary["BJP+"] = 0;
            resultsSummary["JP+"] = 0;
            resultsSummary["O"] = 0;
            var en = Enumerable.From(acResults);
            var allianceMap = this.allianceMap;
            acResults.forEach(function (element) {
                var votes = Enumerable.From(en.Where(function (t) { return t.Id == element.Id; }).First().Votes);
                var party = votes.First(function (v) { return v.Position == 1; }).Party;
                if (allianceMap[party] == undefined || allianceMap[party] == "O") {
                    resultsSummary["O"] = resultsSummary["O"] + 1;
                }
                if (allianceMap[party] == "BJP+") {
                    resultsSummary["BJP+"] = resultsSummary["BJP+"] + 1;
                }
                if (allianceMap[party] == "JP+") {
                    resultsSummary["JP+"] = resultsSummary["JP+"] + 1;
                }
            });
            return resultsSummary;
        };
        return AcStyleMap;
    })();
    Controllers.AcStyleMap = AcStyleMap;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var DataLoader = (function () {
        function DataLoader($http, $q) {
            this._acShapeFile = "json/Bihar.Assembly.10k.topo.json";
            this._allACsJson = "json/allACs.json";
            this._results2009Json = "json/results2009AcWise.json";
            this._results2010Json = "json/results2010AcWise.json";
            this._results2014Json = "json/results2014AcWise.json";
            this._results2015Json = "json/predictions2015.json";
            this._localIssues2015 = "";
            this._localIssues2010 = "";
            this._casteDistribution = "";
            this.casteCategoryDistribution = "";
            this.candidateInfo = "";
            this.vipConstituencies = "";
            this._predictions2015 = "";
            this._neighbors = "json/Neighbors.txt";
            this._results2015 = null;
            this._results2010 = null;
            this._results2014 = null;
            this._results2009 = null;
            this._acs = null;
            this.headers = { 'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ' };
            this.http = $http;
            this.q = $q;
        }
        DataLoader.prototype.getColorsJson = function (callback) {
        };
        DataLoader.prototype.getACTopoShapeFile = function (callback) {
            this.http.get(this._acShapeFile, this.headers).success(callback);
        };
        DataLoader.prototype.getAllAssemblyConstituencies = function () {
            var deferred = this.q.defer();
            if (this._acs !== null) {
                deferred.resolve(this._acs);
            }
            this.http.get(this._allACsJson, this.headers).success(function (data) { return deferred.resolve(data); });
            return deferred.promise;
        };
        DataLoader.prototype.getResultsAsync = function (year) {
            var deferred = this.q.defer();
            switch (year) {
                case "2009":
                    if (this._results2009 !== null) {
                        deferred.resolve(this._results2009);
                    }
                    this.http.get(this._results2009Json, this.headers)
                        .success(function (data) { return deferred.resolve(data); });
                    break;
                case "2010":
                    if (this._results2010 !== null) {
                        deferred.resolve(this._results2010);
                    }
                    this.http.get(this._results2010Json, this.headers)
                        .success(function (data) { return deferred.resolve(data); });
                    break;
                case "2014":
                    if (this._results2014 !== null) {
                        deferred.resolve(this._results2014);
                    }
                    this.http.get(this._results2014Json, this.headers)
                        .success(function (data) { return deferred.resolve(data); });
                    break;
                case "2015":
                    if (this._results2015 !== null) {
                        deferred.resolve(this._results2015);
                    }
                    this.http.get(this._results2015Json, this.headers)
                        .success(function (data) { return deferred.resolve(data); });
                    break;
            }
            return deferred.promise;
        };
        DataLoader.prototype.get2010LocalIssuesData = function (callback) {
            this.http.get(this._localIssues2010, this.headers).success(callback);
        };
        DataLoader.prototype.get2015LocalIssuesData = function (callback) {
            this.http.get(this._localIssues2015, this.headers).success(callback);
        };
        DataLoader.prototype.getCasteDistribution = function (callback) {
            this.http.get(this._casteDistribution, this.headers).success(callback);
        };
        DataLoader.prototype.getCasteCategoryDistribution = function (callback) {
            this.http.get(this.casteCategoryDistribution, this.headers).success(callback);
        };
        DataLoader.prototype.getCandidateData = function (callback) {
            this.http.get(this.candidateInfo, this.headers).success(callback);
        };
        DataLoader.prototype.getVIPConstituencies = function (callback) {
            this.http.get(this.vipConstituencies, this.headers).success(callback);
        };
        return DataLoader;
    })();
    Controllers.DataLoader = DataLoader;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var InfoCtrl = (function () {
        function InfoCtrl($scope, $http, $q, $timeout) {
            $scope.vInfo = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            this.dataloader = new Controllers.DataLoader(this.http, this.q);
            this.infoDiv = document.getElementById('info');
        }
        InfoCtrl.prototype.displayInfo = function (id) {
            var _this = this;
            var p2014 = this.dataloader.getResultsAsync("2014");
            var p2010 = this.dataloader.getResultsAsync("2010");
            var p2009 = this.dataloader.getResultsAsync("2009");
            var pR = this.q.all([p2009, p2010, p2014]);
            pR.then(function (_a) {
                var d1 = _a[0], d2 = _a[1], d3 = _a[2];
                return _this.loadResultsForAC(d1, d2, d3, id);
            });
        };
        InfoCtrl.prototype.loadResultsForAC = function (d1, d2, d3, id) {
            var _this = this;
            this.timeout(function () {
                console.log('in load results');
                var r2014 = d1;
                var r2010 = d2;
                var r2009 = d3;
                var en2014 = Enumerable.From(r2014);
                var en2010 = Enumerable.From(r2010);
                var en2009 = Enumerable.From(r2009);
                var results2014 = en2014.First(function (t) { return t.Id == id; });
                var results2010 = en2010.First(function (t) { return t.Id == id; });
                var results2009 = en2009.First(function (t) { return t.Id == id; });
                var title = results2014.Name;
                _this.info = new Models.InfoData(title, results2009, results2010, results2014);
                _this.setInfoDivVisibility("inline");
            }, 500);
        };
        InfoCtrl.prototype.setInfoDivVisibility = function (display) {
            this.infoDiv.style.display = display;
        };
        return InfoCtrl;
    })();
    Controllers.InfoCtrl = InfoCtrl;
})(Controllers || (Controllers = {}));
var Controllers;
(function (Controllers) {
    var MainControl = (function () {
        function MainControl($scope, logService) {
            this.message = "asdf";
            $scope.vm = this;
            logService.log('Some log');
        }
        return MainControl;
    })();
    Controllers.MainControl = MainControl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var MapCtrl = (function () {
        function MapCtrl($scope, $http, $q, $timeout) {
            var _this = this;
            this.acName = "Ac Name";
            this.years = ["2015", "2014", "2010", "2009"];
            this.displayModes = ["Regular", "Stable"];
            this.loadResultsHandler = function (response) { return _this.loadResultsCallback(response); };
            this.mouseClickHandler = function (event) { return _this.mouseClick(event); };
            this.getDefaultCenterCallback = function (results, status) { return _this.getDefaultCenter(results, status); };
            $scope.vMap = this;
            this.scope = $scope;
            this.http = $http;
            this.q = $q;
            this.timeout = $timeout;
            this.dataloader = new Controllers.DataLoader(this.http, this.q);
            this.infoCtrl = new Controllers.InfoCtrl(this.scope, this.http, this.q, this.timeout);
            this.mapInstance = Models.Map.Instance;
            this.geocoder = new google.maps.Geocoder();
            this.acStyleMap = new Controllers.AcStyleMap();
            this.defaultColorMap = this.acStyleMap.colorMap;
            this.initialize();
        }
        MapCtrl.prototype.initialize = function () {
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.mapInstance.addEventHandler('click', this.mouseClickHandler);
            this.loadResults("2010");
            this.infoCtrl.setInfoDivVisibility("none");
        };
        MapCtrl.prototype.mouseClick = function (event) {
            var id = event.feature.getProperty('ac');
            var name = event.feature.getProperty('ac_name');
            this.acName = name;
            this.infoCtrl.displayInfo(id);
            console.log("In click with id:" + id + " " + this.acName);
        };
        MapCtrl.prototype.getDefaultCenter = function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                this.mapInstance.setCenter(results[0].geometry.location);
            }
            else {
            }
        };
        MapCtrl.prototype.geocode = function (address) {
            var geocoderComponentRestrictions = {};
            var request = {
                address: address,
                componentRestrictions: geocoderComponentRestrictions
            };
            this.geocoder.geocode(request, this.getDefaultCenterCallback);
        };
        MapCtrl.prototype.loadGeoData = function () {
            console.log("Loading geo data...");
            this.dataloader.getACTopoShapeFile(this.mapInstance.loadGeoJson);
        };
        MapCtrl.prototype.loadResults = function (year) {
            var pResults = this.dataloader.getResultsAsync(year);
            pResults.then(this.loadResultsHandler);
        };
        MapCtrl.prototype.loadResultsCallback = function (acResults) {
            var styleMapsArray;
            if (this.yearSelected == "2015") {
                styleMapsArray = this.acStyleMap.Generate2015StyleMaps(acResults);
            }
            else {
                styleMapsArray = this.acStyleMap.GenerateStyleMaps(acResults);
            }
            this.resultsSummary = this.acStyleMap.GenerateResultsSummary(acResults);
            var styleMaps = Enumerable.From(styleMapsArray);
            this.mapInstance.setStyle(function (feature) {
                var id = feature.getProperty('ac');
                return styleMaps.First(function (t) { return t.Id == id; }).Style;
            });
        };
        MapCtrl.prototype.changeDisplayMode = function () {
            var _this = this;
            var p2014 = this.dataloader.getResultsAsync("2014");
            var p2010 = this.dataloader.getResultsAsync("2010");
            var p2009 = this.dataloader.getResultsAsync("2009");
            this.q.all([p2014, p2010, p2009]).then(function (data) { return _this.loadDisplayMode(data[0], data[1], data[2]); });
        };
        MapCtrl.prototype.loadDisplayMode = function (ac2014, ac2010, ac2009) {
            var resultsHolder = new Models.ResultsHolder(ac2009, ac2010, ac2014);
            var stability = resultsHolder.Stability;
            var styleMaps = Enumerable.From(this.acStyleMap.GenerateDisplayModeStyleMaps(stability));
            this.mapInstance.setStyle(function (feature) {
                var id = feature.getProperty('ac');
                return styleMaps.First(function (t) { return t.Id == id; }).Style;
            });
        };
        MapCtrl.prototype.yearSelectionChanged = function () {
            this.loadResults(this.yearSelected);
        };
        MapCtrl.prototype.displayModeChanged = function () {
            switch (this.displayMode) {
                case "Regular":
                    break;
                case "Stable":
                    this.changeDisplayMode();
                    break;
                default:
            }
        };
        return MapCtrl;
    })();
    Controllers.MapCtrl = MapCtrl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var SearchBoxCtrl = (function () {
        function SearchBoxCtrl($scope, $http, $q) {
            this.placeholder = "Search Constituency";
            $scope.searchBox = this;
            this.scope = $scope;
            this.dataloader = new Controllers.DataLoader($http, $q);
        }
        SearchBoxCtrl.prototype.userQueryChanged = function () {
            var _this = this;
            var pACs = this.dataloader.getAllAssemblyConstituencies();
            pACs.then(function (data) { return _this.displayTopSearchResults(data); });
        };
        SearchBoxCtrl.prototype.displayTopSearchResults = function (data) {
            var acs = data;
            var userQ = this.userQuery.trim();
            var en = Enumerable.From(acs);
            if (userQ.length == 0) {
                this.dropDownList = [];
                return;
            }
            var candidates = en.Where(function (t) { return t.Name.indexOf(userQ, 0) === 0; }).Take(5);
            this.dropDownList = candidates.ToArray();
        };
        return SearchBoxCtrl;
    })();
    Controllers.SearchBoxCtrl = SearchBoxCtrl;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
directives.directive('testme', function () {
    return {
        restrict: 'EAC',
        template: testme.html
    };
});
var Alliance;
(function (Alliance) {
    Alliance[Alliance["NDA"] = 0] = "NDA";
    Alliance[Alliance["UPA"] = 1] = "UPA";
    Alliance[Alliance["JP"] = 2] = "JP";
    Alliance[Alliance["IND"] = 3] = "IND";
    Alliance[Alliance["LEFT"] = 4] = "LEFT";
    Alliance[Alliance["O"] = 5] = "O";
})(Alliance || (Alliance = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var InfoData = (function () {
        function InfoData(Title, Results2009, Results2010, Results2014) {
            this.Title = Title;
            this.Results2009 = Results2009;
            this.Results2010 = Results2010;
            this.Results2014 = Results2014;
        }
        return InfoData;
    })();
    Models.InfoData = InfoData;
})(Models || (Models = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var Neighbors = (function () {
        function Neighbors() {
            if (Neighbors._instance)
                return;
            Neighbors._instance = this;
        }
        Object.defineProperty(Neighbors, "Instance", {
            get: function () {
                if (!(this._instance)) {
                    throw new Error("Neighbors data not yet instantiated.");
                }
                return this._instance;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Neighbors, "NeighborsMap", {
            get: function () {
                return this._neighborsDict;
            },
            enumerable: true,
            configurable: true
        });
        Neighbors.BuildNeighbors = function (data) {
            this._instance = new Neighbors();
            var lines = data.split('\n');
            for (var index = 0; index < lines.length; index++) {
                this._neighborsDict[index] = lines[index].split(',');
            }
            return this._instance;
        };
        Neighbors._neighborsDict = {};
        return Neighbors;
    })();
    Models.Neighbors = Neighbors;
})(Models || (Models = {}));
var Party;
(function (Party) {
    Party[Party["BJP"] = 0] = "BJP";
    Party[Party["JDU"] = 1] = "JDU";
    Party[Party["RJD"] = 2] = "RJD";
    Party[Party["INC"] = 3] = "INC";
    Party[Party["LJP"] = 4] = "LJP";
    Party[Party["IND"] = 5] = "IND";
    Party[Party["BSP"] = 6] = "BSP";
    Party[Party["CPI"] = 7] = "CPI";
    Party[Party["O"] = 8] = "O";
})(Party || (Party = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var Result = (function () {
        function Result() {
        }
        Result.prototype.GetWinner = function () {
            var en = Enumerable.From(this.Votes);
            return en.First(function (t) { return t.Position == 1; }).Name;
        };
        Result.prototype.GetWinningParty = function () {
            var en = Enumerable.From(this.Votes);
            return en.First(function (t) { return t.Position == 1; }).Party;
        };
        return Result;
    })();
    Models.Result = Result;
    var CandidateVote = (function () {
        function CandidateVote() {
        }
        return CandidateVote;
    })();
    Models.CandidateVote = CandidateVote;
})(Models || (Models = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var ResultsHolder = (function () {
        function ResultsHolder(Results2009, Results2010, Results2014) {
            this.Results2009 = Results2009;
            this.Results2010 = Results2010;
            this.Results2014 = Results2014;
            this._stability = null;
        }
        Object.defineProperty(ResultsHolder.prototype, "Stability", {
            get: function () {
                if (this._stability !== null) {
                    return this._stability;
                }
                var en2014 = Enumerable.From(this.Results2014);
                var en2010 = Enumerable.From(this.Results2010);
                var en2009 = Enumerable.From(this.Results2009);
                var stability = en2014.Select(function (t) {
                    var ac2009 = en2009.First(function (x) { return x.Id == t.Id; });
                    var winningParty2009 = ac2009.Votes[0].Party;
                    var winningParty2010 = en2010.First(function (y) { return y.Id == t.Id; }).Votes[0].Party;
                    var stability = new Models.Stability();
                    stability.Id = t.Id;
                    stability.Stability = t.Votes[0].Party === winningParty2009 && t.Votes[0].Party == winningParty2010;
                    stability.Party = t.Votes[0].Party;
                    return stability;
                }).ToArray();
                this._stability = stability;
                return this._stability;
            },
            enumerable: true,
            configurable: true
        });
        return ResultsHolder;
    })();
    Models.ResultsHolder = ResultsHolder;
})(Models || (Models = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var Stability = (function () {
        function Stability() {
        }
        return Stability;
    })();
    Models.Stability = Stability;
})(Models || (Models = {}));
/// <reference path="../reference.ts" />
var Models;
(function (Models) {
    var Map = (function () {
        function Map(mapDiv) {
            var _this = this;
            this.loadGeoJson = function (data) { return _this.addGeoJson(data); };
            if (Map._instance)
                return;
            Map._instance = this;
            this._mapDiv = mapDiv;
        }
        Map.prototype.addGeoJson = function (data) {
            var parentThis = this;
            if (data.objects) {
                $.each(data.objects, function (i, layer) {
                    var geojson = topojson.feature(data, layer);
                    parentThis._map.data.addGeoJson(geojson);
                });
            }
        };
        Map.prototype.setCenter = function (center) {
            this._map.setCenter(center);
        };
        Map.prototype.addEventHandler = function (eventType, callback) {
            this._map.data.addListener(eventType, callback);
        };
        Map.prototype.setStyle = function (callback) {
            this._map.data.setStyle(callback);
        };
        Map.prototype.viewDataLayer = function () {
            this._map.data.setStyle({ visible: true });
        };
        Map.prototype.hideDataLayer = function () {
            this._map.data.setStyle({ visible: false });
        };
        Object.defineProperty(Map, "Instance", {
            get: function () {
                if (!(this._instance)) {
                    var mapDiv = document.getElementById('mapCanvas');
                    this._instance = new Map(mapDiv);
                    this._instance._defaultCenter = new google.maps.LatLng(23, 84);
                    this._instance._mapOptions = {
                        zoom: 8,
                        center: this._instance._defaultCenter,
                        minZoom: 4,
                        disableDefaultUI: true
                    };
                    var mapType = this.getDefaultMapType();
                    this._instance._map = new google.maps.Map(this._instance._mapDiv, this._instance._mapOptions);
                    this._instance._map.mapTypes.set('customtype', mapType);
                    this._instance._map.setMapTypeId('customtype');
                }
                return this._instance;
            },
            enumerable: true,
            configurable: true
        });
        Map.getDefaultMapType = function () {
            var defaultMapType = new google.maps.StyledMapType([
                {
                    featureType: 'water',
                    elementType: 'geometry',
                    stylers: [
                        { hue: '#0000ff' },
                        { saturation: 50 },
                        { lightness: -50 }
                    ]
                },
                {
                    featureType: 'road.arterial',
                    elementType: 'all',
                    stylers: [
                        { visibility: 'off' },
                    ]
                }, {
                    featureType: 'road.local',
                    elementType: 'all',
                    stylers: [
                        { visibility: 'off' },
                    ]
                },
                {
                    featureType: 'road.highway',
                    elementType: 'geometry',
                    stylers: [
                        { hue: '#ff0000' },
                        { lightness: 50 },
                        { visibility: 'on' },
                        { saturation: 98 }
                    ]
                },
                {
                    featureType: 'road.highway',
                    elementType: 'labels',
                    stylers: [
                        { visibility: 'off' },
                    ]
                },
                {
                    featureType: 'administrative',
                    elementType: 'labels',
                    stylers: [
                        { visibility: 'on' },
                    ]
                },
                {
                    featureType: 'transit.line',
                    elementType: 'geometry',
                    stylers: [
                        { hue: '#ff0000' },
                        { visibility: 'on' },
                        { lightness: -70 }
                    ]
                }
            ], { name: 'customtype' });
            return defaultMapType;
        };
        Object.defineProperty(Map.prototype, "Map", {
            get: function () {
                return this._map;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Map.prototype, "MapDiv", {
            get: function () {
                return this._mapDiv;
            },
            enumerable: true,
            configurable: true
        });
        return Map;
    })();
    Models.Map = Map;
})(Models || (Models = {}));
/// <reference path="../reference.ts" />
var ColorService = (function () {
    function ColorService() {
        var _this = this;
        this.loadColorJson = function (data) { return _this.returnColorJson(data); };
    }
    ColorService.prototype.returnColorJson = function (data) {
        var colorsObj = angular.fromJson(data);
        for (var i = 0; i < colorsObj.Colors.length; i++) {
            var color = colorsObj.Colors[i];
            this.colorMap[color.Name] = color.Color;
        }
        return this.colorMap;
    };
    ColorService.prototype.getColorJson = function () {
        if (this.colorMap != null) {
            return this.colorMap;
        }
    };
    ColorService.prototype.getPartyColor = function (party) {
        return this.colorMap[party];
    };
    ColorService.prototype.getAllianceColor = function (alliance) {
        return this.colorMap[alliance];
    };
    ColorService.prototype.getColor = function (color, value, min, max, nLevels) {
        if (min === void 0) { min = 0; }
        if (max === void 0) { max = 100; }
        if (nLevels === void 0) { nLevels = 9; }
        var colors = colorbrewer.Oranges[nLevels];
        switch (color) {
            case "orange":
                colors = colorbrewer.Oranges[nLevels];
                break;
            case "green":
                colors = colorbrewer.Greens[nLevels];
                break;
            case "red":
                colors = colorbrewer.Reds[nLevels];
                break;
            case "black":
                colors = colorbrewer.Greys[nLevels];
                break;
            case "blue":
                colors = colorbrewer.Blues[nLevels];
                break;
            default:
                throw new Error("color not supported: " + color);
        }
        var colorScale = d3.scale.quantize()
            .domain([min - 10, max]).range(colors);
        return colorScale(value);
    };
    return ColorService;
})();
services.service('colorService', ColorService);
var LogService = (function () {
    function LogService() {
    }
    LogService.prototype.log = function (msg) {
        console.log(msg);
    };
    return LogService;
})();
services.service('logService', LogService);
/// <reference path="../reference.ts" />
angular.module('controllers', []).controller(Controllers);
/// <reference path="./reference.ts" />
angular.module('ElectionVisualization', ['controllers', 'services', 'directives']);
/// <reference path="services/services.ts" />
/// <reference path="directives/directives.ts" />
/// <reference path="directives/testme.html.ts" />
/// <reference path="controllers/AcStyleMap.ts" />
/// <reference path="controllers/Constituency.ts" />
/// <reference path="controllers/DataLoader.ts" />
/// <reference path="controllers/InfoCtrl.ts" />
/// <reference path="controllers/MainController.ts" />
/// <reference path="controllers/MapCtrl.ts" />
/// <reference path="controllers/SearchBoxCtrl.ts" />
/// <reference path="directives/testme.ts" />
/// <reference path="models/Alliance.ts" />
/// <reference path="models/InfoData.ts" />
/// <reference path="models/Neighbors.ts" />
/// <reference path="models/Party.ts" />
/// <reference path="models/Result.ts" />
/// <reference path="models/ResultsHolder.ts" />
/// <reference path="models/Stability.ts" />
/// <reference path="models/map.ts" />
/// <reference path="services/ColorService.ts" />
/// <reference path="services/LogService.ts" />
/// <reference path="vendor.d.ts" />
/// <reference path="controllers/controllers.ts" />
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map