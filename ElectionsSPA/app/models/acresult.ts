/// <reference path="../reference.ts" />

module Models {
	"use strict";
	export interface Result {		
		Id: number;
		Name: string;
		Votes: CandidateVote[]
	}
		
	export interface CandidateVote {
		Name: string;
		Party: Party;
		Votes: number;
	}
	
	export class ResultsLoader {
		public static LoadResultsFromJson(json: string): Result[]{
			var resultsObj: Result[] = JSON.parse(json);
			return resultsObj;
		}
	}
}