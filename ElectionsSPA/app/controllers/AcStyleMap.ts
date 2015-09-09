/// <reference path="../reference.ts" />

module Controllers {
    export class AcStyleMap {
        public Id: number;
        public Style: google.maps.Data.StyleOptions;
        public colorMap: PartyToColorMap;
        public allianceMap: Alliance;
        public defaultStyle: google.maps.Data.StyleOptions = {
                strokeWeight: 1,
                fillOpacity: 0.8,
                strokeOpacity: 0.3,
                strokeColor: "white"
        };          
        public resultsSummary: ResultsSummary = {};
        
        constructor() {
            this.initializeColorMap();
            this.initializeAlliance();
        }
        
        initializeColorMap() {
            this.colorMap = {};
            this.colorMap["bjp"] = "orange";
            this.colorMap["jdu"] = "lightgreen";
            this.colorMap["rjd"] = "darkgreen";
            this.colorMap["inc"] = "lightblue"
            this.colorMap["ljp"] = "yellow";
            this.colorMap["rlsp"] = "brown";
            this.colorMap["cpi"] = "darkred";
            this.colorMap["ind"] = "black";
            this.colorMap["jmm"] = "purple";
        }
        
        initializeAlliance() {
            this.allianceMap = {};            
            this.allianceMap["bjp"] = "BJP+";
            this.allianceMap["ljp"] = "BJP+";
            this.allianceMap["rlsp"] = "BJP+";
            this.allianceMap["ham"] = "BJP+";
            this.allianceMap["rjd"] = "JP+";
            this.allianceMap["jdu"] = "JP+";
            this.allianceMap["sp"] = "JP+";
            this.allianceMap["inc"] = "JP+";
            this.allianceMap["cpi"] = "O";            
        }
                        
        public GenerateStyleMaps(acResults: Models.Result[]): AcStyleMap[]{
            let en = Enumerable.From(acResults);
            let acStyleMaps: AcStyleMap[] = [];
            en.ForEach(element => {
                let votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                let party = votes.First(t=> t.Position == 1).Party;
                let styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                styleMap.Style = {
                    strokeWeight: this.defaultStyle.strokeWeight,
                    fillOpacity: this.defaultStyle.fillOpacity,
                    strokeOpacity: this.defaultStyle.strokeOpacity,
                    fillColor: this.colorMap[party]
                }
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        }
        
        public GenerateResultsSummary(acResults: Models.Result[]): ResultsSummary {
            var resultsSummary: ResultsSummary = {};
            resultsSummary["BJP+"] = 0;
            resultsSummary["JP+"] = 0;
            resultsSummary["O"] = 0;

            var en = Enumerable.From(acResults);
            var allianceMap = this.allianceMap;
            acResults.forEach(element => {
                let votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                let party = votes.First(v => v.Position == 1).Party;
                if (allianceMap[party] == undefined || allianceMap[party] == "O")
                {
                    resultsSummary["O"] = resultsSummary["O"] + 1;
                }    
                if (allianceMap[party] == "BJP+")
                {
                    resultsSummary["BJP+"] = resultsSummary["BJP+"] + 1;
                }    
                if (allianceMap[party] == "JP+")
                {
                    resultsSummary["JP+"] = resultsSummary["JP+"] + 1;
                }                
            });
            return resultsSummary;
        }
    }
    
    export interface Alliance {
        [party: string]: string;    
    }
    
     export interface PartyToColorMap {
        [party: string]: string;
     }
}
