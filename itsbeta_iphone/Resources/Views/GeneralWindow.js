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
			backgroundColor: "#ededed"
		},
		subviews:
		[
			{
				style : {
					className: "Ti.UI.View",
					top: 0,
					height: 57,
					zIndex: 9,
					backgroundImage: "images/navbar/Bg.png",
					zIndex: 12
				},
				subviews:
				[
					{
						outlet: "profileClick",
						style : {
							className: "Ti.UI.View",
							top: 0,
							left: 0,
							width: 40,
							height: 40,
							backgroundColor: "transparent"
						},
						subviews:
						[
							{
								outlet: "profile",
								style : {
									className: "Ti.UI.View",
									top: 5,
									left: 10,
									width: 24,
									height: 24,
									backgroundImage: "images/buttons/Profile.Normal.png"
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
								style: {
									className: "Ti.UI.ImageView",
									width: 28,
									height: 28,
									top: 7,		
									left: 8,
									image: "images/icons/Candy.png"
								}	
							},
							{
								outlet: "counter",
								style : {
									className: "Ti.UI.Label",
									color: "#7ed6f9",
									text: "50",
									font: {fontSize: 20, fontWeight: "bold"}
								}
							}
						]
					},
					{
						outlet: "addClick",
						style : {
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
								outlet: "add",
								style : {
									className: "Ti.UI.View",
									top: 5,
									right: 10,
									height: 24,
									width: 24,
									backgroundImage: "images/buttons/Plus.Normal.png"
								}
							}
						]
					}
				]
			},
			{
				outlet: "placeListView",
				style : {
					className: "Ti.UI.View",
					layout: "vertical",
					top: -395,
					height: 440,
					width: Ti.UI.FILL,
					zIndex: 11
				},
				subviews:
				[
					{
						outlet: "placeList",
						style : {
							className: "Ti.UI.ScrollView",
							layout: "vertical",
							top: 0,
							height: 440,
							width: Ti.UI.FILL,
							verticalBounce: false,
							backgroundColor: "#f7f7f7",
							zIndex: 11
						}
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 30,
					height: 54,
					zIndex: 6,
					backgroundImage: "images/navbar/Selects.Bg.png"
				},
				subviews:
				[
					{
						style: {
							className: "Ti.UI.View",
							top: 30,
							left: 0,
							height: Ti.UI.SIZE,
							width: "50%"
						},
						subviews: [
							{
								outlet: "typeProjectClick",
								style: {
									className: "Ti.UI.View",
									width: Ti.UI.SIZE,
									layout: "horizontal"
								},
								subviews: [
									{
										outlet: "typeProject",
										style : {
											className: "Ti.UI.Label",
											height: 20,
											width: 120,
											color: "#646464",
											textAlign: Ti.UI.TEXT_ALIGNMENT_CENTER,
											font: {fontSize: 17},
											textid: "label_categories"
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
					{
						style: {
							className: "Ti.UI.View",
							backgroundImage: "images/navbar/Selects.Divider.png",
							width: 5
						}
					},
					{
						style: {
							className: "Ti.UI.View",
							top: 30,
							right: 0,
							height: Ti.UI.SIZE,
							width: "50%"
						},
						subviews: [
							{
								outlet: "nameProjectClick",
								style: {
									className: "Ti.UI.View",
									width: Ti.UI.SIZE,
									layout: "horizontal",
									backgroundColor: "transparent"
								},
								subviews: [
									{
										outlet: "nameProject",
										style : {
											className: "Ti.UI.Label",
											height: 20,
											width: 120,
											textAlign: Ti.UI.TEXT_ALIGNMENT_CENTER,
											color: "#646464",
											font: {fontSize: 17},
											textid: "label_subcategories"
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
					top: 84,
					zIndex: 6,
					height: 8
				},
				subviews: [
				 	{
						style: {
							className: "Ti.UI.View",
							backgroundImage: "images/navbar/Selects.Divider.png",
							width: 5,
							bottom: 3
						}
					}
				]
			},
			{
				outlet: "preAchivs",
				style : {
					className: "Ti.UI.TableView",
					layout: "vertical",
					top: 90,
					height: Ti.UI.FILL,
					width: Ti.UI.FILL,
					backgroundColor: "#ededed",
					visible: false,
					zIndex: 5
				}
			}
		]
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
		}
	},
	{
		outlet: "placeListViewCancel",
		style : {
			className: "Ti.UI.View",
			layout: "vertical",
			top: 0,
			height: 45,
			width: Ti.UI.FILL,
			backgroundColor: "transparent",
			visible: false,
			zIndex: 20
		}
	},
		{
		outlet: "transparentView",
		style : {
			className: "Ti.UI.View",
			top: 0,
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			backgroundColor: "transparent",
			visible: false,
			touchEnabled: false,
			zIndex: 30
		}
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