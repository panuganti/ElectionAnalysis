/// <reference path="../reference.ts" />

module Controllers {
    export class AcStyleMap {
        public Id: number;
        public Style: google.maps.Data.StyleOptions;
        public colorMap: PartyToColorMap;
        public allianceColorMap: PartyToColorMap;
        public allianceMap: Alliance;
        public defaultStyle: google.maps.Data.StyleOptions = {
                strokeWeight: 1,
                fillOpacity: 0.9,
                strokeOpacity: 0.3,
                strokeColor: "white"
        };          
        public resultsSummary: ResultsSummary = {};
        
        constructor() {
            this.initializeColorMap();
            this.initializeAlliance();
            this.initializeAllianceColorMap();
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
            this.allianceMap["rslp"] = "BJP+"; // TODO: Fix error in 2014 json .. it should be rlsp
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
        }
        
        initializeAllianceColorMap() {
            this.allianceColorMap = {};
            this.allianceColorMap["BJP+"] = "orange";
            this.allianceColorMap["JP+"] = "green";
            this.allianceColorMap["O"] = "black";
        }

        public Generate2015InfoMaps(acDistrib: Models.Distribution[]): AcStyleMap[] {
            var colorService = new ColorService();
            let en = Enumerable.From(acDistrib);
            let acStyleMaps: AcStyleMap[] = [];
            en.ForEach(element => {
                let styleMap = new AcStyleMap();
                styleMap.Id = element.AcNo;
                let percent = element.Percent;
                let color = colorService.getColor("red", percent,0,40);
                styleMap.Style = {
                    strokeWeight: this.defaultStyle.strokeWeight,
                    fillOpacity: this.defaultStyle.fillOpacity,
                    strokeOpacity: this.defaultStyle.strokeOpacity,
                    fillColor: color
                }
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;            
        }
        
        public Generate2015StyleMaps(acResults: Models.Result[]): AcStyleMap[]{
            var colorService = new ColorService();
            let en = Enumerable.From(acResults);
            let acStyleMaps: AcStyleMap[] = [];
            en.ForEach(element => {
                let votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                let party = votes.First(t=> t.Position == 1).Party;
                let alliance = this.allianceMap[party];
                let partyColor = this.allianceColorMap[alliance];
                let styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                let phase = votes.First(t=> t.Position == 1).Votes;
                let opacity = phase != 5 ? 0: this.defaultStyle.fillOpacity;
                styleMap.Style = {
                    strokeWeight: this.defaultStyle.strokeWeight,
                    fillOpacity: opacity,
                    strokeOpacity: 1,
                    fillColor: partyColor
                }
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        }

        
        public GenerateStyleMaps(acResults: Models.Result[]): AcStyleMap[]{
            var colorService = new ColorService();
            let en = Enumerable.From(acResults);
            let acStyleMaps: AcStyleMap[] = [];
            en.ForEach(element => {
                let votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                let party = votes.First(t=> t.Position == 1).Party;
                let alliance = this.allianceMap[party];
                let partyColor = this.allianceColorMap[alliance];
                let styleMap = new AcStyleMap();
                styleMap.Id = element.Id;
                let totalVotes = votes.Select(t=>t.Votes).ToArray().reduce((a,b) => a + b);
                let margin = votes.First(t=> t.Position == 1).Votes - votes.First(t=> t.Position == 2).Votes;
                let marginPercent = Math.ceil((margin)*100/totalVotes);
                let color = colorService.getColor(partyColor, marginPercent,1,25); 
                styleMap.Style = {
                    strokeWeight: this.defaultStyle.strokeWeight,
                    fillOpacity: this.defaultStyle.fillOpacity,
                    strokeOpacity: this.defaultStyle.strokeOpacity,
                    fillColor: color
                }
                acStyleMaps.push(styleMap);
            });
            return acStyleMaps;
        }
        
        public GenerateDisplayModeStyleMaps(stabilities: Models.Stability[]) : AcStyleMap[] {
            let acStyleMaps: AcStyleMap[] = [];
            let en = Enumerable.From(stabilities);
            en.ForEach( element => {
              let styleMap = new AcStyleMap();
              styleMap.Id = element.Id;              
              styleMap.Style = {
                strokeWeight: this.defaultStyle.strokeWeight,
                fillOpacity: this.defaultStyle.fillOpacity,
                strokeOpacity: this.defaultStyle.strokeOpacity,
                fillColor: element.Stability ? this.colorMap[element.Party] : this.colorMap["ind"]
              };
              acStyleMaps.push(styleMap);
            }
            );
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
