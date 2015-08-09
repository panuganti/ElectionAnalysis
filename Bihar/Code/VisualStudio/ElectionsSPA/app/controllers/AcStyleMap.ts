/// <reference path="../../vendor/types/googlemaps/google.maps.d.ts"/>

module Controllers {
    export class AcStyleMap {
        public Id: number;
        public WinningColor: string;
        public Style: google.maps.Data.StyleOptions;
                        
        public parseJson(json: any) : AcStyleMap[] {
            var results: AcStyleMap[] = [];
            var defaultStyle: google.maps.Data.StyleOptions = {
                strokeWeight: 1,
                fillOpacity: 0.5,
                strokeOpacity: 0.3
            }; 
            json.forEach(element => {
                var result = new AcStyleMap();
                for (var prop in element) result[prop] = element[prop];
                result.Style = {
                    fillColor: result.WinningColor,
                    strokeWeight: defaultStyle.strokeWeight,
                    fillOpacity: defaultStyle.fillOpacity,
                    strokeOpacity: defaultStyle.strokeOpacity
                };
                results.push(result);
            });
            return results;
        }
    }
}
