/// <reference path="../reference.ts" />

module Models {
	export class Map {
		constructor(mapDiv: HTMLElement) {
            if (Map._instance) return;
            Map._instance = this;
			this._mapDiv = mapDiv;
		}

		addGeoJson(data: any) {
            // Note: remember to clear map (refer shape escape website)
            let parentThis = this;
            if (data.objects) {
                $.each(data.objects, (i, layer) => {
                    let geojson = topojson.feature(data, layer);
                    parentThis._map.data.addGeoJson(geojson);
                });
                console.log("Loading completed");
            }
		}

		setCenter(center: google.maps.LatLng) {
			this._map.setCenter(center);
		}

		addEventHandler(eventType: string, callback: (ev: Event) => any) {
            this._map.data.addListener(eventType, callback);
        }

		setStyle(callback) {
			this._map.data.setStyle(callback);
		}        
        //#region Toggle Data Layer
        viewDataLayer() {
            this._map.data.setStyle({ visible: true });
        }

        hideDataLayer() {
            this._map.data.setStyle({ visible: false });
        }
        //#endregion Toggle Data Layer

        public loadGeoJson: { (data: any): void } = (data) => this.addGeoJson(data);

		public static get Instance() {
			if (!(this._instance)) {
				let mapDiv = document.getElementById('mapCanvas');
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
        }

		public static getDefaultMapType() {
			var defaultMapType = new google.maps.StyledMapType([
				{
					featureType: 'all',
					stylers: [
						{ visibility: 'off' },
					]
				}
				/*, {
					featureType: 'road.arterial',
					elementType: 'all',
					stylers: [
						{ hue: '#2200ff' },
						{ lightness: -40 },
						{ visibility: 'simplified' },
						{ saturation: 30 }
					]
				}, {
					featureType: 'road.local',
					elementType: 'all',
					stylers: [
						{ hue: '#f6ff00' },
						{ saturation: 50 },
						{ gamma: 0.7 },
						{ visibility: 'simplified' }
					]
				}, {
					featureType: 'water',
					elementType: 'geometry',
					stylers: [
						{ saturation: 40 },
						{ lightness: 40 }
					]
				}, {
					featureType: 'road.highway',
					elementType: 'labels',
					stylers: [
						{ visibility: 'on' },
						{ saturation: 98 }
					]
				}, {
					featureType: 'administrative.locality',
					elementType: 'labels',
					stylers: [
						{ hue: '#0022ff' },
						{ saturation: 50 },
						{ lightness: -10 },
						{ gamma: 0.90 }
					]
				}, {
					featureType: 'transit.line',
					elementType: 'geometry',
					stylers: [
						{ hue: '#ff0000' },
						{ visibility: 'on' },
						{ lightness: -70 }
					]
				}*/
			], { name: 'customtype' });
			return defaultMapType;
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