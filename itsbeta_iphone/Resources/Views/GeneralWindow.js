/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style : {
			className: "Ti.UI.View",
			top: 0,
			height: Ti.UI.SIZE,
			width: Ti.UI.FILL,
			layout: "vertical",
			backgroundColor: "white",
			zIndex: 12
		},
		subviews:
		[
			{
				style : {
					className: "Ti.UI.View",
					top: 0,
					height: 57,
					zIndex: 10,
					backgroundImage: "images/navbar/bg.png"
				},
				subviews:
				[
					{
						outlet: "profile",
						style : {
							className: "Ti.UI.View",
							top: 5,
							left: 10,
							width: 22,
							height: 22,
							backgroundImage: "images/buttons/Profile.Normal.png"
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
								style: {
									className: "Ti.UI.ImageView",
									width: 28,
									height: 28,
									top: 7,		
									left: 8,
									image: "images/icons/Goblet.png"
								}	
							},
							{
								outlet: "counter",
								style : {
									className: "Ti.UI.Label",
									color: "#7ed6f9",
									text: "50",
									font: {fontSize: 30, fontFamily: "Helvetica", fontWeight: "bold"}
								}
							}
						]
					},
					{
						outlet: "add",
						style : {
							className: "Ti.UI.View",
							top: 5,
							right: 10,
							height: 22,
							width: 22,
							backgroundImage: "images/buttons/Plus.Normal.png"
						}
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: -20,
					height: 70,
					width: Ti.UI.FILL,
					layout: "horizontal",
					backgroundImage: "images/navbar/Selects.Bg.png"
				},
				subviews:
				[
					{
						style: {
							className: "Ti.UI.View",
							top: 40,
							height: Ti.UI.SIZE,
							width: "50%",
							bottom: 10
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
										outlet: "typeProject",
										style : {
											className: "Ti.UI.Label",
											text: "Категория"
										}
									},
									{
										style: {
											className: "Ti.UI.ImageView",
											image: "images/navbar/Arrow.png",
											left: 5,
											width: 13,
											height: 13
										}
									}
								]
							}
						]	
					},
					// {
						// style: {
							// className: "Ti.UI.View",
							// backgroundImage: "images/navbar/Selects.Divider.png",
							// height: Ti.UI.SIZE,
							// width: 5
						// }
					// },
					{
						style: {
							className: "Ti.UI.View",
							top: 40,
							height: Ti.UI.SIZE,
							width: "50%",
							bottom: 10
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
										outlet: "nameProject",
										style : {
											className: "Ti.UI.Label",
											text: "Проект"
										}
									},
									{
										style: {
											className: "Ti.UI.ImageView",
											image: "images/navbar/Arrow.png",
											left: 5,
											width: 13,
											height: 13
										}
									}
								]
							}
						]	
					}
				]
			},
			{
				style: {
					className: "Ti.UI.View",
					backgroundImage: "images/navbar/Selects.Bottom.png",
					height: 8
				}
			}
		]
	},
	{
		outlet: "preAchivs",
		style : {
			className: "Ti.UI.ScrollView",
			layout: "vertical",
			top: 107,
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			zIndex: 10
		}
	},
	{
		outlet: "list",
		style:{
			className: "Ti.UI.View",
			top: 80,
			left: 0,
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
			}
		]
	},
	{
		outlet: "placeListView",
		style : {
			className: "Ti.UI.View",
			layout: "vertical",
			top: -248,
			height: 355,
			width: Ti.UI.FILL,
			backgroundColor: "transparent",
			zIndex: 10
		},
		subviews:
		[
			{
				outlet: "placeList",
				style : {
					className: "Ti.UI.ScrollView",
					layout: "vertical",
					top: 0,
					height: 355,
					width: Ti.UI.FILL,
					backgroundColor: "transparent",
					zIndex: 10
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
			visible: false,
			zIndex: 15
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