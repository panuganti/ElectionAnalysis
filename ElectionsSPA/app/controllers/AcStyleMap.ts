/// <reference path="../reference.ts" />

module Controllers {
    export class AcStyleMap {
        public Id: number;
        public Style: google.maps.Data.StyleOptions;
        public defaultStyle: google.maps.Data.StyleOptions = {
                strokeWeight: 1,
                fillOpacity: 0.5,
                strokeOpacity: 0.3
        };                     
        public colorMap: PartyToColorMap = {
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
                        
        public GenerateStyleMaps(acResults: Models.Result[]): AcStyleMap[]{
            var en = Enumerable.From(acResults);
            var acStyleMaps: AcStyleMap[] = [];
            acResults.forEach(element => {
                var styleMap = new AcStyleMap();
                var votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                var party = votes.First(t=>t.Position == 1).Party;
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
    }
    
    
     interface PartyToColorMap {
        [party: string]: string;
     }
}
