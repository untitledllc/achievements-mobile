/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style: {
			className: "Ti.UI.View",
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			opacity: 0.5,
			backgroundColor: "black"
		},
	},
	{
		outlet: "shadowClose",
		style: {
			className: "Ti.UI.ScrollView",
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			backgroundColor: "transparent",
			visible: false
		},
		subviews:
		[
			{
				outlet: "clickClose",
				style: {
					className: "Ti.UI.View",
					top: 0,
					height: Ti.UI.FILL,
					width: Ti.UI.FILL,
					backgroundColor: "transparent"
				}
			},
			{
				outlet: "bonus",
				style: {
					className: "Ti.UI.View",
					layout: "vertical",
					top: 90,
					bottom: 5,
					height: Ti.UI.SIZE,
					width: "90%",
					backgroundImage: "images/bg/Panel.png",
					backgroundLeftCap: 13,
					backgroundTopCap: 13
				},
				subviews:
				[
					{
						outlet: "closeClick",
						style: {
							className: "Ti.UI.View",
							top: 0,
							right: 0,
							height: 40,
							width: 40,
							backgroundColor: "transparent"
						},
						subviews:
						[
							{
								outlet: "close",
								style: {
									className: "Ti.UI.View",
									top: 5,
									right: 5,
									height: 22,
									width: 22,
									backgroundImage: "images/buttons/Close.Normal.png"
								}
							}
						]
					},
					{
						outlet: "nameAchivs",
						style: {
							className: "Ti.UI.Label",
							top: 30,
							left: 20,
							height: Ti.UI.SIZE,
							width: 220,
							text: "Крутая ачивка",
							color: "#777575",
							font: {fontSize: 18, fontWeight: "regular",fontFamily: "Helvetica"}
							
						}
					},
					{
						outlet: "textAchivs",
						style: {
							className: "Ti.UI.View",
							top: 10,
							left: 15,
							bottom: 0,
							backgroundColor: "transparent",
							height: Ti.UI.SIZE,
							width: 220,
						}
					}
				]
			},
			{
				style: {
					className: "Ti.UI.View",
					top: 10,
					left: 5,
					width: 130,
					height: 130,
					touchEnabled: false,
					
				},
				subviews:
				[
					{
						outlet: "image",
						style: {
							className: "Ti.UI.ImageView",
							width: "100%",
							height: "100%",
							touchEnabled: false,
						}
					}
				]
			}
		]
	},
	{
		outlet: "act",
		style : {
			className : "Ti.UI.ActivityIndicator",
			height: 50,
			width: 50
		}
	}
]