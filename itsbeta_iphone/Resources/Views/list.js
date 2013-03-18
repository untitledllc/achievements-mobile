/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "rowAchivs",
	style : {
		className : "Ti.UI.View",
		height: 50,
		width: Ti.UI.FILL,
		backgroundColor: "white",
	},
	subviews:
	[
		{
			outlet: "rowTextAchivs",
			style : {
				className : "Ti.UI.Label",
				height: Ti.UI.SIZE,
				width: Ti.UI.SIZE,
				text: "text",
				touchEnabled: false
			}
		},
		{
			style : {
				className : "Ti.UI.View",
				bottom: 0,
				height: 1,
				width: Ti.UI.FILL,
				backgroundColor: "gray",
				touchEnabled: false
			}
		}
	]
}