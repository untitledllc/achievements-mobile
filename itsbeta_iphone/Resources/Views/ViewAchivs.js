/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "viewAchivs",
	style : {
		className : "Ti.UI.View",
		height: Ti.UI.SIZE,
		width: Ti.UI.FILL,
		backgroundColor: "white",
	},
	subviews:
	[
		{
			outlet: "image",
			style : {
				className : "Ti.UI.ImageView",
				top: 0,
				left: 0,
				height: 60,
				width: 60,
				image: "images/FirstBadge.PNG",
				touchEnabled: false
			}
		},
		{
			outlet: "date",
			style : {
				className : "Ti.UI.Label",
				top: 80,
				left: 0,
				height: Ti.UI.SIZE,
				width: Ti.UI.SIZE,
				text: "18.02.12",
				color: "#14CCD2",
				font : {
					fontSize : 10
				},
				touchEnabled: false
			}
		},
		{
			outlet: "name",
			style : {
				className : "Ti.UI.Label",
				top: 0,
				left: 70,
				height: Ti.UI.SIZE,
				width: 250,
				color: "red",
				touchEnabled: false
			}
		},
		{
			outlet: "desc",
			style : {
				className : "Ti.UI.Label",
				top: 0,
				left: 70,
				height: 70,
				width: 240,
				text : " ",
				color: "gray",
				font : {
					fontSize : 13
				},
				touchEnabled: false
			}
		},
		{
			style: {
				className: "Ti.UI.View",
				bottom: 0,
				height: 1,
				width: Ti.UI.FILL,
				backgroundColor: "gray"
			}
		}
	]
}