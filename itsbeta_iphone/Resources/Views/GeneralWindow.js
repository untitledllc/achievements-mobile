/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style : {
			className: "Ti.UI.View",
			top: 0,
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			layout: "vertical",
			backgroundColor: "white"
		},
		subviews:
		[
			{
				style : {
					className: "Ti.UI.View",
					top: 0,
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
				},
				subviews:
				[
					{
						outlet: "profile",
						style : {
							className: "Ti.UI.View",
							left: 0,
							height: 50,
							width: 50,
							backgroundImage: "images/NavBar_ProfileButton.png"
						}
					},
					{
						outlet: "counter",
						style : {
							className: "Ti.UI.Label",
							left: "45%",
							height: 50,
							width: Ti.UI.SIZE,
							text: "20"
						}
					},
					{
						outlet: "add",
						style : {
							className: "Ti.UI.View",
							right: 0,
							height: 50,
							width: 50,
							backgroundImage: "images/NavBar_addcode.png"
						}
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 20,
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
				},
				subviews:
				[
					{
						outlet: "typeProject",
						style : {
							className: "Ti.UI.Label",
							left: 0,
							height: 50,
							width: Ti.UI.SIZE,
							text: "type"
						}
					},
					{
						outlet: "nameProject",
						style : {
							className: "Ti.UI.Label",
							right: 0,
							height: 50,
							width: Ti.UI.SIZE,
							text: "name"
						}
					},
				]
			},
			{
				outlet: "preAchivs",
				style : {
					className: "Ti.UI.ScrollView",
					layout: "vertical",
					top: 20,
					height: Ti.UI.FILL,
					width: Ti.UI.FILL,
				}
			}
		]
	},
	{
		outlet: "list",
		style:{
			className: "Ti.UI.View",
			top: 80,
			height: Ti.UI.SIZE,
			width: Ti.UI.FILL,
			layout: "vertical",
			backgroundColor: "transparent",
			visible: false
		},
		subviews:
		[
			{
				outlet: "rowTextAchivs",
				style : {
					className : "Ti.UI.Label",
					left: "35%",
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
					text: "text"
				}
			},
			{
				style : {
					className : "Ti.UI.View",
					height: 1,
					width: Ti.UI.FILL,
					backgroundColor: "gray",
				}
			},
			{
				outlet: "placeList",
				style : {
					className: "Ti.UI.ScrollView",
					layout: "vertical",
					height: Ti.UI.FILL,
					width: Ti.UI.FILL,
				}
			}
		]
	}
]