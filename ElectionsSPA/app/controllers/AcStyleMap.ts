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
            let en = Enumerable.From(acResults);
            let acStyleMaps: AcStyleMap[] = [];
            en.Select(element => {
                let styleMap = new AcStyleMap();
                let votes = Enumerable.From(en.Where(t=> t.Id == element.Id).First().Votes)
                let party = votes.First(t=>t.Position == 1).Party;
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
