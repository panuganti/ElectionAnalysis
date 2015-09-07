interface ColorsMap {
	[name: string] : string;
}

class ColorService{
	private colorMap: ColorsMap;
	private dataloader: any;

    public loadColorJson: { (data: any): void } = (data) => this.returnColorJson(data);

	 constructor($http, $q)  {
	 	this.dataloader = new Controllers.DataLoader($http, $q);
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
	 	this.dataloader.getColorsJson(this.loadColorJson);	
	}

    getPartyColor(party: string) {
    	return this.colorMap[party];
    }

    getAllianceColor(alliance: string) {
    	return this.colorMap[alliance];
    }
}

services.service('colorService',ColorService);