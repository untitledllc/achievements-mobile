/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "rowAchivs",
	style : {
		className : "Ti.UI.View",
		height: 50,
		width: Ti.UI.FILL,
		backgroundImage: "images/navbar/Selects.Bg.png",
		lastRow: undefined
	},
	subviews:
	[
		{
			outlet: "rowTextAchivs",
			style : {
				className : "Ti.UI.Label",
				// left: 15,
				// right: 15,
				height: Ti.UI.SIZE,
				width: Ti.UI.SIZE,
				color: "#646464",
				text: "text",
				touchEnabled: false
			}
		},
		{
			style : {
				className : "Ti.UI.View",
				bottom: 0,
				height: 1,
				backgroundImage: "images/others/Divider.Horizontal.png",
				touchEnabled: false
			}
		}
	]
}