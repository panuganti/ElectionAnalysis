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