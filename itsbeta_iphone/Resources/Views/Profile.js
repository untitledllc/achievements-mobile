/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style : {
			className: "Ti.UI.View",
			top: 0,
			height: 57,
			zIndex: 10,
			backgroundImage: "images/navbar/Bg.png",
			backgroundColor: "transparent"
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
							textid: "label_profile",
							font: {fontSize: 18, fontWeight: "bold"}
						}
					}
				]
			},
			{
				outlet: "logOut",
				style : {
					className: "Ti.UI.Button",
					top: 5,
					right: 5,
					height: 22,
					width: 22,
					backgroundImage: "images/buttons/Exit.Normal.png"
				}
			}
		]
	},
	{
		style : {
			className: "Ti.UI.ScrollView",
			top: 60,
			bottom: 0,
			height: Ti.UI.SIZE,
			layout: "vertical"
		},
		subviews: [
			{
				style : {
					className: "Ti.UI.View",
					top: 10,
					left: 10, 
					right: 10,
					height: Ti.UI.SIZE,
					layout: "horizontal",
					backgroundImage: "images/bg/Panel.png",
					backgroundLeftCap: 13,
					backgroundTopCap: 13
				},
				subviews:
				[
					{
						style: {
							className: "Ti.UI.ImageView",
							image: "images/icons/Facebook.png",
							width: 40,
							height: 40,
							top: 20,
							left: 15,
							right: 15,
							bottom: 20
						}
					},
					{
						style: {
							className: "Ti.UI.View",
							height: Ti.UI.SIZE,
							layout: "vertical"
						}, 
						subviews: [
							{
								outlet: "profileName",
								style: {
									className: "Ti.UI.Label",
									top: 0,
									left: 0,
									color: "#625f5e",
									font: { fontSize: 20 }
								}
							},
							{
								outlet: "profileInfo",
								style: {
									className: "Ti.UI.Label",
									top: 5,
									left: 0,
									color: "#a9a7a3",
									font: { fontSize: 10 }
								}
							}
						]
					}
				]
			},
			{
				style : {
					className: "Ti.UI.Label",
					top: 10,
					left: 12,
					height: 40,
					width: Ti.UI.SIZE,
					font : {
						fontSize : 20
					},
					color: "#625f5e",
					textid: "label_statistic"
				}
			},
			{
				style: {
					className: "Ti.UI.View",
					height: Ti.UI.SIZE,
					left: 10,
					layout: "horizontal"
				},
				subviews: [
					{
						style: {
							className: "Ti.UI.View",
							width: 97,
							height: 73,
							backgroundImage: "images/profile/Statistic.Badges.png"
						},
						subviews: [
							{
								style: {
									className: "Ti.UI.View",
									layout: "vertical",
									left: 5,
									height: Ti.UI.SIZE
								},
								subviews: [
									{
										style : {
											className: "Ti.UI.Label",
											left: 5,
											color: "#fff",
											font : {
												fontSize : 12
											},
											textid: "label_all_badges"
										}
									},
									{
										outlet: "all",
										style: {
											className: "Ti.UI.Label",
											left: 0,
											color: "#fff",
											font: {
												fontSize: 20
											},
											text: " "
										}
									}
								]
							}
						]
					},
					{
						style: {
							className: "Ti.UI.View",
							width: 97,
							height: 73,
							left: 5,
							backgroundImage: "images/profile/Statistic.Bonuses.png"
						},
						subviews: [
							{
								style: {
									className: "Ti.UI.View",
									layout: "vertical",
									left: 5,
									height: Ti.UI.SIZE
								},
								subviews: [
									{
										style: {
											className: "Ti.UI.Label",
											left: 5,
											color: "#fff",
											font: {
												fontSize: 12
											},
											textid: "label_bonuses"
										}
									},
									{
										outlet: "bonus",
										style: {
											className: "Ti.UI.Label",
											left: 0,
											color: "#fff",
											font: {
												fontSize: 20
											},
											text: " "
										}
									}
								]
							}
							
						]
					},
					{
						style: {
							className: "Ti.UI.View",
							width: 97,
							height: 73,
							left: 5,
							backgroundImage: "images/profile/Statistic.Subcategories.png"
						},
						subviews: [
							{	
								style: {
									className: "Ti.UI.View",
									layout: "vertical",
									left: 5,
									height: Ti.UI.SIZE
								},
								subviews: [
									{
										style : {
											className: "Ti.UI.Label",
											left: 5,
											color: "#fff",
											font : {
												fontSize : 12
											},
											textid: "label_profile_subcategories"
										}
									},
									{
										outlet: "sub",
										style: {
											className: "Ti.UI.Label",
											left: 0,
											color: "#fff",
											font: {
												fontSize: 20
											},
											text: " "
										}
									}
								]
							}
						]
					}
				]
			},
			{
				outlet: "list",
				style : {
					className: "Ti.UI.View",
					top: 10,
					left: 5,
					right: 10,
					height: Ti.UI.SIZE,
					layout: "vertical"
				}
			}
		]
	}
]