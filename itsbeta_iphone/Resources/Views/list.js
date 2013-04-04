/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "rowAchivs",
	style : {
		className : "Ti.UI.View",
		height: 50,
		width: Ti.UI.FILL,
		// backgroundColor: "white",
		backgroundImage: "images/navbar/Selects.Bg.png"
	},
	subviews:
	[
		{
			outlet: "rowTextAchivs",
			style : {
				className : "Ti.UI.Label",
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
				width: Ti.UI.FILL,
				backgroundColor: "#bebebe",
				touchEnabled: false
			}
		}
	]
}