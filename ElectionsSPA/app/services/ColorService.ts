/// <reference path="../reference.ts" />
interface ColorsMap {
	[name: string] : string;
}

class ColorService{
	private colorMap: ColorsMap;
	//private dataloader: any;

    public loadColorJson: { (data: any): void } = (data) => this.returnColorJson(data);

	 constructor()  {
	 //	this.dataloader = new Controllers.DataLoader($http, $q);
	 }

	returnColorJson(data: any) { 
		let colorsObj = angular.fromJson(data);
		for (let i = 0; i < colorsObj.Colors.length; i++)
		{	
			let color = colorsObj.Colors[i];
			this.colorMap[color.Name] = color.Color;
		}
		return this.colorMap;
	}

	getColorJson()
	{
		if (this.colorMap != null) 
		{
			return this.colorMap;
		}
	 	//this.dataloader.getColorsJson(this.loadColorJson);	
	}

    getPartyColor(party: string) {
    	return this.colorMap[party];
    }

    getAllianceColor(alliance: string) {
    	return this.colorMap[alliance];
    }
	
	getColor(color: string, value: number, min = 0, max = 100, nLevels = 9): any {
		let colors: string[] = colorbrewer.Oranges[nLevels];
		switch (color) {
			case "orange":
				colors = colorbrewer.Oranges[nLevels]; break;
			case "green":
				colors = colorbrewer.Greens[nLevels]; break;
			case "red":
				colors = colorbrewer.Reds[nLevels]; break;
			case "black":
				colors = colorbrewer.Greys[nLevels]; break;
			case "blue":
				colors = colorbrewer.Blues[nLevels]; break;
			default:
				throw new Error("color not supported: " + color)							    
		}
		var colorScale = d3.scale.quantize()
			.domain([min-10, max]).range(colors);
		return colorScale(value);
	}
}

services.service('colorService',ColorService);