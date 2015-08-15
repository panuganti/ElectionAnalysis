interface ColorsMap {
	[name: string] : string;
}

class ColorService{
	private colorMap: ColorsMap;
	private dataloader: any;

    public loadColorJson: { (data: any): void } = (data) => this.returnColorJson(data);

	 constructor($http)  {
	 	this.dataloader = new Controllers.DataLoader($http);
	 }

	returnColorJson(data: any) { 
		var colorsObj = angular.fromJson(data);
		for (var i = 0; i < colorsObj.Colors.length; i++)
		{	
			var color = colorsObj.Colors[i];
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