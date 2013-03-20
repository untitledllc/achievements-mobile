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
		},
		subviews:
		[
			{
				outlet: "bonus",
				style: {
					className: "Ti.UI.View",
					layout: "vertical",
					top: 70,
					height: Ti.UI.SIZE,
					width: "90%",
					backgroundColor: "white"
				},
				subviews:
				[
					{
						outlet: "close",
						style: {
							className: "Ti.UI.ImageView",
							top: 5,
							right: 5,
							height: 44,
							width: 44,
							image: "images/Paper_icon_close_norm.png"
						}
					},
					{
						outlet: "nameAchivs",
						style: {
							className: "Ti.UI.Label",
							top: 30,
							left: 20,
							height: 20,
							width: 220,
							text: "Крутая ачивка"
						}
					},
					{
						outlet: "textAchivs",
						style: {
							className: "Ti.UI.View",
							top: 10,
							left: 15,
							height: Ti.UI.SIZE,
							width: 220,
						}
					}
				]
			},
			{
				outlet: "image",
				style: {
					className: "Ti.UI.ImageView",
					top: 30,
					left: 5,
					height: 100,
					width: 100,
					image: "images/FirstBadge.PNG"
				}
			}
		]
	}
]