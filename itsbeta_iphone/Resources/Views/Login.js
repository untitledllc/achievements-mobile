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
			top: 0,
			left: 10,
			right: 10,
			bottom: 10,
			backgroundImage: "images/bg/Panel.png",
			backgroundLeftCap: 30,
			backgroundTopCap: 30
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
					className: "Ti.UI.View",
					top: "5%",
					height: "10%",
					width: "80%",
					backgroundImage: "images/buttons/Facebook.Normal.png"
				},
				subviews: [
					{
						style: {
							className: "Ti.UI.View",
							height: Ti.UI.SIZE,
							width: Ti.UI.SIZE,
							layout: "horizontal"
						}, 
						subviews: [
							{
								style: {
									className: "Ti.UI.Label",
									color: "#fff",
									// height: Ti.UI.FILL,
									text: "Sign in with"
								}
							},
							{
								style: {
									className: "Ti.UI.Label",
									color: "#fff",
									left: 5,
									// height: Ti.UI.FILL,
									text: "facebook"
								}
							}						
						]	
					}
				]
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