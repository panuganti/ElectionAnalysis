/// <reference path="../reference.ts" />

module Models {
	export class ResultsHolder {
		_stability: Stability[] = null;
		constructor(public Results2009: Models.Result[], public Results2010: Models.Result[], public Results2014: Models.Result[]) { }
		
		get Stability() {
			if (this._stability !== null) { return this._stability;}
			var en2014 = Enumerable.From(this.Results2014);
			var en2010 = Enumerable.From(this.Results2010);
			var en2009 = Enumerable.From(this.Results2009);
			var stability = en2014.Select(t=> {
				let winningParty2009 = en2009.First(x => x.Id == t.Id).WinningParty;
				let winningParty2010 = en2009.First(y => y.Id == t.Id).WinningParty;
				let stability = new Stability();
				stability.Id = t.Id;
				stability.Stability = t.WinningParty == winningParty2009 && t.WinningParty == winningParty2010;
				stability.Party = t.WinningParty;
				return stability;
			}).ToArray();
			this._stability = stability;			
			return this._stability;
		}
	}	
}