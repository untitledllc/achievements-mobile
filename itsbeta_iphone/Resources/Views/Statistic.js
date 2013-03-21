/**
 * @author Gom_Dzhabbar
 */
module.exports = [
	{
		style: {
			className: "Ti.UI.View",
			top: 0,
			height: 50,
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
					height: Ti.UI.SIZE,
					width: Ti.UI.SIZE,
					text: "category"
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