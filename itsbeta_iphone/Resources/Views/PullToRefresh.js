exports = {
	outlet: "me",
	style: {
		className: "Ti.UI.View",
		backgroundColor: "#ededed",
		height: 60
	},
	subviews: [		
	 	{
	 		outlet: "status",
	 		style: {
	 			className: "Ti.UI.Label",
	 			text: L("pullToRefresh"),
				left: 60,
				bottom: 22,
				color: "#737474",
				font: {fontSize: "14pt", fontFamily: "BarnaulGroteskExtraBold-Reg"}
	 		}
	 	},
	 	{
	 		outlet: "lastUpdated",
	 		style: {
	 			className: "Ti.UI.Label",
	 			text: L("lastUpdated"),
				left: 60,	
				bottom: 7,
				color: "#737474",
				font: {fontSize: "12pt", fontFamily: "BarnaulGrotesk-Reg"}
	 		}
	 	}
	]
}
