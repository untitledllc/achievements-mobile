/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style: {
			className: "Ti.UI.View",
			height: Ti.UI.FILL,
			width: Ti.UI.FILL
		},
		subviews: [
			{
				style: {
					className: "Ti.UI.ImageView",
					height: 175,
					width: 175,
					image: "images/logos/Itsbeta.png",
					zIndex: 10,
					top: 15
				}
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 100,
					left: 10,
					right: 10,
					bottom: 10,
					backgroundImage: "images/bg/Panel.png",
					backgroundLeftCap: 13,
					backgroundTopCap: 13
				},
				subviews: [
					{
						style: {
							className: "Ti.UI.View",
							height: Ti.UI.FILL,
							top: 100,
							left: 10,
							right: 10,
							bottom: 10
						},
						subviews: [
							{
								style: {
									className: "Ti.UI.Label",
									top: 25,
									color: "#646464",
									text: "Collect all your achievements",
									font: {fontSize: 14}
								}
							},
							{
								outlet: "infacebook",
								style: {
									className: "Ti.UI.View",
									height: 46,
									width: 248,
									backgroundImage: "images/buttons/Facebook.Normal.png"
								},
								subviews: [
									{
										style: {
											className: "Ti.UI.View",
											layout: "horizontal",
											height: Ti.UI.SIZE
										}, 
										subviews: [
											{
												style: {
													className: "Ti.UI.Label",
													color: "#fff",
													width: "52%",
													textAlign: Ti.UI.TEXT_ALIGNMENT_RIGHT,
													font: {fontSize: 15},
													text: "Sign in with"
												}
											},
											{
												style: {
													className: "Ti.UI.ImageView",
													image: "images/logos/Facebook.png",
													width: 74,
													height: 22,
													left: 2
												}
											}						
										]	
									}
								]
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