/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "viewAchivs",
	style : {
		className : "Ti.UI.View",
		height: 200,
		width: Ti.UI.FILL,
		backgroundColor: "white",
	},
	subviews:
	[
		{
			outlet: "image",
			style : {
				className : "Ti.UI.ImageView",
				top: 0,
				left: 0,
				height: 60,
				width: 60,
				image: "images/FirstBadge.PNG",
				touchEnabled: false
			}
		},
		{
			outlet: "date",
			style : {
				className : "Ti.UI.Label",
				top: 80,
				left: 0,
				height: Ti.UI.SIZE,
				width: Ti.UI.SIZE,
				text: "18.02.12",
				touchEnabled: false
			}
		},
		{
			outlet: "name",
			style : {
				className : "Ti.UI.Label",
				top: 0,
				left: 70,
				height: Ti.UI.SIZE,
				width: 200,
				text: "kzjdhfkasdjhf",
				touchEnabled: false
			}
		},
		{
			outlet: "desc",
			style : {
				className : "Ti.UI.Label",
				top: 10,
				left: 70,
				height: 70,
				width: 240,
				text: "kzjdhfkasdjhxcdf  sdf sdf sdf sdfg sdfg sfd gsdfgs dfgsfdg sdfgf",
				touchEnabled: false
			}
		}
	]
}