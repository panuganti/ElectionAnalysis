/// <reference path="../reference.ts" />
/// <reference path="../vendor.d.ts" />

module Models {
	export class Map {
		constructor(mapDiv: HTMLElement) {
            if (Map._instance) return;
            Map._instance = this;
			this._mapDiv = mapDiv;
		}

		addGeoJson(data: any) {
            // Note: remember to clear map (refer shape escape website)
            var parentThis = this;
            if (data.objects) { 
                $.each(data.objects, (i, layer) => {
                    var geojson = topojson.feature(data, layer);
                    parentThis._map.data.addGeoJson(geojson);
                });
                console.log("Loading completed");
            }
			// TODO: Set event handlers
            //this.addEventHandler('mouseover', this.mouseOverHandler);
            //this.addEventHandler('mouseclick', this.mouseClickHandler);
		}

        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);
        		
		public static get Instance() {
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
				this._instance._map = new google.maps.Map(this._instance._mapDiv,
														this._instance._mapOptions);
			}
            return this._instance;
        }

		public get Map(): google.maps.Map {
			return this._map;
		}
        
		public get MapDiv(): HTMLElement {
			return this._mapDiv;
		}

		private static _instance: Map;
        private _mapDiv: HTMLElement;
        private _defaultCenter: google.maps.LatLng;
        private _mapOptions: google.maps.MapOptions;
		private _map: google.maps.Map;
	}
}