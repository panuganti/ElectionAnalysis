/// <reference path="../Scripts/typings/googlemaps/google.maps.d.ts"/>
/// <reference path="./BiharMap.ts"/>

module BiharElections {

    export class ArchishaMapStyle {
        archishaMapType : google.maps.StyledMapType;
        name: any;

        constructor() {
            this.createMapType();
            this.name = 'ArchishaStyle';
        }
        createMapType() {
            var featureOpts: google.maps.MapTypeStyle[] = [
                {
                    stylers: [
                        { hue: '#890000' },
                        { visibility: 'simplified' },
                        { gamma: 0.5 },
                        { weight: 0.5 }
                    ]
                },
                {
                    elementType: 'labels',
                    stylers: [
                        { visibility: 'off' }
                    ]
                },
                {
                    featureType: 'water',
                    stylers: [
                        { color: '#890000' }
                    ]
                }
            ];

            var styledMapOptions: google.maps.StyledMapTypeOptions = {
                name: 'Archisha Style'
            };

            this.archishaMapType = new google.maps.StyledMapType(featureOpts, styledMapOptions);
        }
    }
}

