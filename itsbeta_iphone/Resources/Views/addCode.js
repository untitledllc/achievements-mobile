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
						outlet: "cancel",
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
									backgroundImage: "images/buttons/Navbar.Left.Normal.png",
									width: 6,
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
											textid: "button_cancel",
											color: "#949494",
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
					},
					{
						outlet: "done",
						style : {
							className: "Ti.UI.View",
							top: 5,
							right: 5,
							height: 30,
							width: Ti.UI.SIZE,
							layout: "horizontal"
						},
						subviews: [
							{
								style : {
									className: "Ti.UI.View",
									backgroundImage: "images/buttons/Navbar.Left.Normal.png",
									width: 6,
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
											textid: "button_done",
											color: "#949494",
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
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 15,
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
						style : {
							className: "Ti.UI.View",
							top: 30,
							height: 40,
							width: "90%"
							//backgroundColor: "white"
						},
						subviews:
						[
							{
								outlet: "code",
								style : {
									className: "Ti.UI.TextField",
									borderStyle: Ti.UI.INPUT_BORDERSTYLE_ROUNDED,
									height: 40,
									width: Ti.UI.FILL
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