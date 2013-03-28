/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style: {
			className: "Ti.UI.View",
			top: 0,
			left: 10,
			height: Ti.UI.SIZE,
			width: Ti.UI.FILL,
			layout: "vertical",
			backgroundColor: "white"
		},
		subviews:
		[
			{
				outlet : "category",
				style : {
					className: "Ti.UI.Label",
					text: "category",
					color: "#625f5e",
					left: 0,
					font: {fontSize: 13, fontWeight: "bold"}
				}
			},
			{
				outlet: "item",
				style: {
					className: "Ti.UI.View",
					height: Ti.UI.SIZE,
					width: Ti.UI.FILL,
					layout: "vertical",
					backgroundColor: "white"
				}
			}
		]
	}
]