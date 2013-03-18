/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style : {
			className : "Ti.UI.View",
			layout: "vertical",
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			backgroundColor: "white",
		},
		subviews:[
			{
				style: {
					className: "Ti.UI.ImageView",
					height: "50%",
					width: "50%",
					image: "images/FirstBadge.PNG"
				}
			},
			{
				style: {
					className: "Ti.UI.Label",
					top: "5%",
					height: Ti.UI.SIZE,
					wigth: Ti.UI.SIZE,
					text: "Collect all your achivements"
				}
			},
			{
				outlet: "infacebook",
				style: {
					className: "Ti.UI.Button",
					top: "5%",
					height: "10%",
					width: "80%",
					title: "Sing in facebook",
				}
			},
			{
				style: {
					className: "Ti.UI.Label",
					top: "10%",
					height: Ti.UI.SIZE,
					width: Ti.UI.SIZE,
					text: "Facebook sing up"
				}
			}
		]
	},
	{
		outlet: "actView",
		style : {
			className : "Ti.UI.View",
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			backgroundColor: "black",
			opacity: 0.5,
			visible: false
		},
		subviews:
		[
			{
				outlet: "act",
				style : {
					className : "Ti.UI.ActivityIndicator",
					height: 50,
					width: 50
				}
			}
		]
	}
]