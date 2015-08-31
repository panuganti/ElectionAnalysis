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
                fillOpacity: 0.5,
                strokeOpacity: 0.3
            };
            this.colorMap = {
                "bjp": "orange",
                "jdu": "lightgreen",
                "rjd": "darkgreen",
                "inc": "lightblue",
                "ljp": "yellow",
                "rlsp": "yellow",
                "cpi": "red",
                "ind": "black",
                "jmm": "purple"
            };
        }
        AcStyleMap.prototype.GenerateStyleMaps = function (acResults) {
            var _this = this;
            var en = Enumerable.From(acResults);
            var acStyleMaps = [];
            acResults.forEach(function (element) {
                var styleMap = new AcStyleMap();
                var votes = Enumerable.From(en.Where(function (t) { return t.Id == element.Id; }).First().Votes);
                var party = votes.First(function (t) { return t.Position == 1; }).Party;
                styleMap.Id = element.Id;
                styleMap.Style = {
                    strokeWeight: _this.defaultStyle.strokeWeight,
                    fillOpacity: _this.defaultStyle.fillOpacity,
                    strokeOpacity: _this.defaultStyle.strokeOpacity,
                    fillColor: _this.colorMap[party]
                };
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        };
        return AcStyleMap;
    })();
    Controllers.AcStyleMap = AcStyleMap;
})(Controllers || (Controllers = {}));
/// <reference path="../reference.ts" />
var Controllers;
(function (Controllers) {
    var DataLoader = (function () {
        function DataLoader($http) {
            this.acShapeFile = "json/Bihar.Assembly.10k.topo.json";
            this.allACsJson = "json/allACs.json";
            this.results2009 = "json/results2009AcWise.json";
            this.results2010 = "json/results2014AcWise.json";
            this.results2014 = "json/results2014AcWise.json";
            this.localIssues2015 = "";
            this.localIssues2010 = "";
            this.casteDistribution = "";
            this.casteCategoryDistribution = "";
            this.candidateInfo = "";
            this.vipConstituencies = "";
            this.predictions2015 = "";
            this.neighbors = "json/Neighbors.txt";
            this.headers = { 'Authorization': 'OAuth AIzaSyD4of1Mljc1T1HU0pREX7fvfUKZX-lx2HQ' };
            this.http = $http;
        }
        DataLoader.prototype.getColorsJson = function (callback) {
        };
        DataLoader.prototype.getACTopoShapeFile = function (callback) {
            this.http.get(this.acShapeFile, this.headers).success(callback);
        };
        DataLoader.prototype.getAllAssemblyConstituencies = function (callback) {
            this.http.get(this.allACsJson, this.headers).success(callback);
        };
        DataLoader.prototype.get2010Results = function (callback) {
            this.http.get(this.results2010, this.headers).success(callback);
        };
        DataLoader.prototype.get2014Results = function (callback) {
            this.http.get(this.results2014, this.headers).success(callback);
        };
        DataLoader.prototype.get2010LocalIssuesData = function (callback) {
            this.http.get(this.localIssues2010, this.headers).success(callback);
        };
        DataLoader.prototype.get2015LocalIssuesData = function (callback) {
            this.http.get(this.localIssues2015, this.headers).success(callback);
        };
        DataLoader.prototype.getCasteDistribution = function (callback) {
            this.http.get(this.casteDistribution, this.headers).success(callback);
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
        DataLoader.prototype.get2015Predictions = function (callback) {
            this.http.get(this.predictions2015, this.headers).success(callback);
        };
        return DataLoader;
    })();
    Controllers.DataLoader = DataLoader;
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
        function MapCtrl($scope, $http) {
            var _this = this;
            this.loadResultsHandler = function (response) { return _this.loadResultsCallback(response); };
            this.mouseOverHandler = function (event) { return _this.mouseOver(event); };
            this.mouseClickHandler = function (event) { return _this.mouseClick(event); };
            this.getDefaultCenterCallback = function (results, status) { return _this.getDefaultCenter(results, status); };
            $scope.vm = this;
            this.http = $http;
            this.mapInstance = Models.Map.Instance;
            this.infoDiv = document.getElementById('info');
            this.dataloader = new Controllers.DataLoader(this.http);
            this.geocoder = new google.maps.Geocoder();
            this.initialize();
        }
        MapCtrl.prototype.initialize = function () {
            this.geocode("Patna, Bihar, India");
            this.loadGeoData();
            this.mapInstance.addEventHandler('mouseover', this.mouseOverHandler);
            this.mapInstance.addEventHandler('mouseclick', this.mouseClickHandler);
            this.load2010results();
            this.setInfoDivVisibility("none");
        };
        MapCtrl.prototype.setInfoDivVisibility = function (display) {
            this.infoDiv.style.display = display;
        };
        MapCtrl.prototype.mouseOver = function (event) {
            this.setInfoDivVisibility("inline");
            var id = event.feature.getProperty('ac');
            console.log("In mouseOver with id:" + id);
        };
        MapCtrl.prototype.mouseClick = function (event) {
            this.setInfoDivVisibility("inline");
            var id = event.feature.getProperty('ac');
            console.log("In mouseclick with id:" + id);
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
        MapCtrl.prototype.load2010results = function () {
            this.dataloader.get2010Results(this.loadResultsHandler);
        };
        MapCtrl.prototype.loadResultsCallback = function (response) {
            var acStyleMap = new Controllers.AcStyleMap();
            var acResults = response;
            var styleMaps = Enumerable.From(acStyleMap.GenerateStyleMaps(acResults));
            this.mapInstance.setStyle(function (feature) {
                var id = feature.getProperty('ac');
                return styleMaps.First(function (t) { return t.Id == id; }).Style;
            });
        };
        return MapCtrl;
    })();
    Controllers.MapCtrl = MapCtrl;
})(Controllers || (Controllers = {}));
var Controllers;
(function (Controllers) {
    var SearchBoxCtrl = (function () {
        function SearchBoxCtrl($scope) {
            this.message = "Search Constituency";
            $scope.vm = this;
        }
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
                console.log("Loading completed");
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
                        mapTypeId: google.maps.MapTypeId.TERRAIN,
                        minZoom: 4,
                        disableDefaultUI: true
                    };
                    this._instance._map = new google.maps.Map(this._instance._mapDiv, this._instance._mapOptions);
                }
                return this._instance;
            },
            enumerable: true,
            configurable: true
        });
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
var ColorService = (function () {
    function ColorService($http) {
        var _this = this;
        this.loadColorJson = function (data) { return _this.returnColorJson(data); };
        this.dataloader = new Controllers.DataLoader($http);
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
        this.dataloader.getColorsJson(this.loadColorJson);
    };
    ColorService.prototype.getPartyColor = function (party) {
        return this.colorMap[party];
    };
    ColorService.prototype.getAllianceColor = function (alliance) {
        return this.colorMap[alliance];
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
"use strict";
angular.module('ElectionVisualization', ['controllers', 'services', 'directives']);
/// <reference path="services/services.ts" />
/// <reference path="directives/directives.ts" />
/// <reference path="directives/testme.html.ts" />
/// <reference path="controllers/AcStyleMap.ts" />
/// <reference path="controllers/Constituency.ts" />
/// <reference path="controllers/DataLoader.ts" />
/// <reference path="controllers/MainController.ts" />
/// <reference path="controllers/MapCtrl.ts" />
/// <reference path="controllers/SearchBoxCtrl.ts" />
/// <reference path="directives/testme.ts" />
/// <reference path="models/Alliance.ts" />
/// <reference path="models/Party.ts" />
/// <reference path="models/Result.ts" />
/// <reference path="models/map.ts" />
/// <reference path="models/neighbors.ts" />
/// <reference path="services/ColorService.ts" />
/// <reference path="services/LogService.ts" />
/// <reference path="vendor.d.ts" />
/// <reference path="controllers/controllers.ts" />
/// <reference path="main.ts" />
//# sourceMappingURL=out.js.map