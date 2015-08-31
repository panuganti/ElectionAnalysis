/// <reference path="../reference.ts" />

module Models {
	export class Neighbors {
		constructor() {
            if (Neighbors._instance) return;
            Neighbors._instance = this;
		}
		
		public static get Instance() {
			if (!(this._instance)) {
				throw new Error("Neighbors data not yet instantiated.");
			}
			return this._instance;
		}
		
		public static get NeighborsMap() {
			return this._neighborsDict;
		}
		
		public static BuildNeighbors(data) {
			this._instance = new Neighbors();
			let lines = data.split('\n');
			for (let index = 0; index < lines.length; index++) {
				this._neighborsDict[index] = lines[index].split(',');
			}
			return this._instance;
		}
		
		private static _neighborsDict: { [id: number]: number[] } = {};
		private static _instance: Neighbors;
	}	
}