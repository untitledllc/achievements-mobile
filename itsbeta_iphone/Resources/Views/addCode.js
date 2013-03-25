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
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
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
						outlet: "counter",
						style : {
							className: "Ti.UI.Label",
							left: "45%",
							height: 50,
							width: Ti.UI.SIZE,
							text: "ADD"
						}
					},
					{
						outlet: "done",
						style : {
							className: "Ti.UI.Button",
							right: 0,
							height: 50,
							width: 50,
							title: "done"
						}
					},
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 30,
					height: 300,
					width: "90%",
					layout: "vertical",
					backgroundColor: "red"
				},
				subviews:
				[
					{
						style : {
							className: "Ti.UI.View",
							top: 30,
							height: 40,
							width: "90%",
							backgroundColor: "white"
						},
						subviews:
						[
							{
								outlet: "code",
								style : {
									className: "Ti.UI.TextField",
									height: 40,
									width: "80%",
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