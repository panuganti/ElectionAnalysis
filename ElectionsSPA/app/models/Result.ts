/// <reference path="../reference.ts" />

module Models {
	export class Result {		
		Id: number;
		Name: string;
		Votes: CandidateVote[]
	}
		
	export class CandidateVote {
		Name: string;
		Party: string;
		Votes: number;
		Position: number;
	}	
}