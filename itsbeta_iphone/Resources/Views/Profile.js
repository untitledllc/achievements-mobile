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
							text: "PROFILE"
						}
					}
				]
			},
			{
				style : {
					className: "Ti.UI.View",
					top: 30,
					height: 150,
					width: "90%",
					layout: "vertical",
					backgroundColor: "red"
				},
				subviews:
				[
					{
						outlet: "profileName",
						style : {
							className: "Ti.UI.Label",
							top: 0,
							height: 40,
							width: Ti.UI.SIZE,
							font : {
								fontSize : 20
							},
							text : "asdasdasd"
						}
					},
					{
						outlet: "profileInfo",
						style : {
							className: "Ti.UI.Label",
							top: 10,
							height: 40,
							width: Ti.UI.SIZE,
							font : {
								fontSize : 10
							},
							text: "afasdasdas"
						}
					},
				]
			}
		]
	}	
]