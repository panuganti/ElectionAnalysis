/// <reference path="../reference.ts" />

module Models {
	export class ResultsHolder {
		_stability: Stability[] = null;
		constructor(public Results2009: Models.Result[], public Results2010: Models.Result[], public Results2014: Models.Result[]) { }
		
		get Stability() : Stability[] {
			if (this._stability !== null) { return this._stability;}
			let en2014 = Enumerable.From(this.Results2014);
			let en2010 = Enumerable.From(this.Results2010);
			let en2009 = Enumerable.From(this.Results2009);
			let stability = en2014.Select(t=> {
				let ac2009: Models.Result = en2009.First(x => x.Id == t.Id)
				let winningParty2009 = ac2009.Votes[0].Party;
				let winningParty2010 = en2010.First(y => y.Id == t.Id).Votes[0].Party;
				let stability = new Stability();
				stability.Id = t.Id;
				stability.Stability = t.Votes[0].Party === winningParty2009 && t.Votes[0].Party == winningParty2010;
				stability.Party = t.Votes[0].Party;
				return stability;
			}).ToArray();
			this._stability = stability;			
			return this._stability;
		}
	}	
}