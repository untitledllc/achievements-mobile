/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style : {
			className : "Ti.UI.View",
			layout: "vertical",
			height: Ti.UI.SIZE,
			width: Ti.UI.FILL,
			backgroundColor: "transpatent",
		},
		subviews:
		[
			{
				outlet: "me",
				style : {
					className : "Ti.UI.View",
					height: 50,
					left: 2,
					right: 2,
					width: Ti.UI.SIZE,
					backgroundColor: "transparent"
				},
				subviews:
				[
					{
						outlet: "type",
						style : {
							className : "Ti.UI.Label",
							left: 15,
							height: Ti.UI.SIZE,
							width: Ti.UI.FILL,
							color: "white",
							font : {
								fontSize : 20
							},
							text: "Бонус"
						}
					}
				]
			},
			{
				outlet: "desc",
				style : {
					className : "Ti.UI.Label",
					top: 20,
					left: 15,
					bottom: 20,
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
					font : {
						fontSize : 13
					},
					text: "Бонусkjghvhgvjhgvhjgv"
				}
			}
		]
	}
]