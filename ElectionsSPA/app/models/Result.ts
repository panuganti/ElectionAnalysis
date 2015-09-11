/// <reference path="../reference.ts" />

module Models {
	export class Result {		
		Id: number;
		Name: string;
		Votes: CandidateVote[]
		
		constructor() { }
		
		get Winner(): string {
			var en = Enumerable.From(this.Votes);
			return en.Aggregate((l, r) => l.Votes > r.Votes ? l : r).Name;
		}
		
		get WinningParty(): string {
			var en = Enumerable.From(this.Votes);
			return en.Aggregate((l, r) => l.Votes > r.Votes ? l : r).Party;
		}
	}
		
	export class CandidateVote extends Candidate {
		Votes: number;
		Position: number;
	}	
	
	export class Candidate {
		Name: string;
		Party: string;
	}	
}