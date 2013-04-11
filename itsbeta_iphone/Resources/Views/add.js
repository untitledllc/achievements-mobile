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
							className: "Ti.UI.View",
							top: 5,
							left: 5,
							height: 30,
							width: Ti.UI.SIZE,
							layout: "horizontal"
						},
						subviews: [
							{
								style : {
									className: "Ti.UI.View",
									backgroundImage: "images/buttons/Back.Left.Normal.png",
									width: 14,
									height: Ti.UI.FILL
								}
							},
							{
								style : {
									className: "Ti.UI.View",
									backgroundImage: "images/buttons/Navbar.Middle.Normal.png",
									width: Ti.UI.SIZE,
									height: Ti.UI.FILL
								},
								subviews: [
									{
										style: {
											className: "Ti.UI.Label",
											textid: "button_back",
											color: "#646464",
											height: Ti.UI.FILL,
											bottom: 2,
											font: {fontSize: 12, fontWeight: "bold"}
										}
									}								
								]
							},
							{
								style : {
									className: "Ti.UI.View",
									backgroundImage: "images/buttons/Navbar.Right.Normal.png",
									width: 6,
									height: Ti.UI.FILL
								}
							}
						]
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
									textid: "label_add",
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
					backgroundLeftCap: 13,
					backgroundTopCap: 13
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
							backgroundImage: "images/buttons/Qr.Normal.png"
						},
						subviews:
						[
							{
								style: {
									className: "Ti.UI.Label",
									textAlign: Ti.UI.TEXT_ALIGNMENT_CENTER,
									font : {
										fontSize : 16
									},
									color: "#fff",
									textid: "button_use_qr_reader"
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
							backgroundImage: "images/buttons/Code.Normal.png"
						},
						subviews:
						[
							{
								style: {
									className: "Ti.UI.Label",
									textAlign: Ti.UI.TEXT_ALIGNMENT_CENTER,
									font: {
										fontSize : 16
									},
									color: "#fff",
									textid: "button_enter_code"
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