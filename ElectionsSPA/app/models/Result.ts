/// <reference path="../reference.ts" />

module Models {
	export class Result {		
		Id: number;
		Name: string;
		Votes: CandidateVote[];
		
		constructor() { }
		
		GetWinner(): string {
			var en = Enumerable.From(this.Votes);
			return en.First(t=>t.Position == 1).Name;
		}
		
		GetWinningParty(): string {
			var en = Enumerable.From(this.Votes);
			return en.First(t=>t.Position == 1).Party;
		}
	}
		
	export class CandidateVote {
		Votes: number;
		Position: number;
		Name: string;
		Party: string;
	}		
}