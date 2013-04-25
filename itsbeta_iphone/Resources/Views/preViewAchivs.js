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
				outlet: "bonusView",
				style: {
					className: "Ti.UI.View",
					top: 90,
					bottom: 5,
					height: Ti.UI.SIZE,
					width: "90%",
					// backgroundLeftCap: 13,
					// backgroundTopCap: 13
				},
				subviews:
				[
					{
						style: {
							className: "Ti.UI.View",
							top: 0,
							left: 0,
							height: "100%",
							width: Ti.UI.FILL,
							backgroundColor: "transparent"
						},
						subviews:
						[
							{
								style: {
									className: "Ti.UI.View",
									top: 0,
									left: 0,
									height: 16,
									width: 16,
									backgroundImage: "images/paper/Paper_background_topLeft.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									top: 0,
									left: 16,
									right: 16,
									height: 16,
									backgroundImage: "images/paper/Paper_background_topMiddle.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									top: 0,
									right: 0,
									height: 16,
									width: 16,
									backgroundImage: "images/paper/Paper_background_topRight.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									top: 16,
									left: 0,
									width: 16,
									//top: "auto",
									height: Ti.UI.FILL,
									bottom: 16,
									backgroundImage: "images/paper/Paper_background_middleLeft.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									top: 16,
									left: 16,
									right: 16,
									//top: "auto",
									height: Ti.UI.FILL,
									bottom: 16,
									backgroundImage: "images/paper/Paper_background_middleMiddle.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									top: 16,
									right: 0,
									width: 16,
									//top: "auto",
									height: Ti.UI.FILL,
									bottom: 16,
									backgroundImage: "images/paper/Paper_background_middleRight.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									//top: "auto",
									left: 0,
									width: 16,
									height: 16,
									bottom: 0,
									backgroundImage: "images/paper/Paper_background_bottomLeft.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									//top: "auto",
									left: 16,
									right: 16,
									height: 16,
									bottom: 0,
									backgroundImage: "images/paper/Paper_background_bottomMiddle.png"
								}
							},
							{
								style: {
									className: "Ti.UI.View",
									//top: "auto",
									right: 0,
									height: 16,
									width: 16,
									bottom: 0,
									backgroundImage: "images/paper/Paper_background_bottomRight.png"
								}
							},
						]
					},
					//------------------------
					
					{
						outlet: "bonus",
						style: {
							className: "Ti.UI.View",
							layout: "vertical",
							top: 0,
							height: Ti.UI.SIZE,
							width: Ti.UI.FILL,

							//backgroundImage: "images/bg/Panel.png",
							//backgroundLeftCap: 13,
							//backgroundTopCap: 13
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
									font: {fontSize: 21, fontWeight: "regular",fontFamily: "Helvetica"}
									
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
									width: Ti.UI.SIZE,
								}
							}
						]
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