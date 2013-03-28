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
					height: 57,
					zIndex: 10,
					backgroundImage: "images/navbar/Bg.png"
				},
				subviews:
				[
					{
						outlet: "back",
						style : {
							className: "Ti.UI.Button",
							left: 0,
							height: 50,
							width: 50,
							title: "back"
						}
					},
					{
						style: {
							className: "Ti.UI.View",
							width: 134,
							height: 46,							
							backgroundImage: "images/navbar/Scoreboard.png"
						},
						subviews: [
							{
								style : {
									className: "Ti.UI.Label",
									color: "#7ed6f9",
									text: "ADD",
									font: {fontSize: 20, fontWeight: "bold"}
								}
							}
						]
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 30,
					height: 300,
					width: "90%",
					layout: "vertical",
					backgroundImage: "images/bg/Panel.png",
					backgroundLeftCap: 30,
					backgroundTopCap: 30
				},
				subviews:
				[
					{
						outlet: "qr",
						style : {
							className: "Ti.UI.View",
							top: 30,
							height: 60,
							width: "80%",
							backgroundColor: "blue"
						},
						subviews:
						[
							{
								style : {
									className: "Ti.UI.Label",
									height: 20,
									width: "80%",
									font : {
										fontSize : 15
									},
									text: "Использовать QR-ридер"
								}
							}
						]
					},
					{
						outlet: "code",
						style : {
							className: "Ti.UI.View",
							top: 5,
							height: 60,
							width: "80%",
							backgroundColor: "blue"
						},
						subviews:
						[
							{
								style : {
									className: "Ti.UI.Label",
									height: 20,
									width: "80%",
									font : {
										fontSize : 15
									},
									text: "Ввести код"
								}
							}
						]
					}
				]
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