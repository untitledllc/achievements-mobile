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
					image: "images/Logo.png",
					zIndex: 10,
					top: 30
				}
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 115,
					left: 10,
					right: 10,
					bottom: 10,
					backgroundImage: "images/bg/Panel.png",
					backgroundLeftCap: 30,
					backgroundTopCap: 30
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
									text: "Collect all your achievements",
									font: {fontSize: 15}
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
											layout: "horizontal"
										}, 
										subviews: [
											{
												style: {
													className: "Ti.UI.Label",
													color: "#fff",
													width: "52%",
													textAlign: Ti.UI.TEXT_ALIGNMENT_RIGHT,
													text: "Sign in with"
												}
											},
											{
												style: {
													className: "Ti.UI.Label",
													color: "#fff",
													width: "45%",
													left: "3%",
													textAlign: Ti.UI.TEXT_ALIGNMENT_LEFT,
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
									bottom: 10,
									height: Ti.UI.SIZE,
									width: Ti.UI.SIZE,
									text: "Facebook sing up"
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