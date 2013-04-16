/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "viewAchivs",
	style : {
		className : "Ti.UI.View",
		height: 150,
		layout: "vertical"
	},
	subviews: [
		{
			style : {
				className : "Ti.UI.View",
				height: Ti.UI.FILL,
				layout: "horizontal",
				touchEnabled: false,
				bottom: -20
			},
			subviews: [
				{
					style: {
						className : "Ti.UI.View",
						layout: "vertical",
						width: "30%",
						height: Ti.UI.FILL,
						touchEnabled: false,
						top: 0
					},
					subviews: [
						{
							outlet: "image",
							style: {
								className: "Ti.UI.ImageView",
								top: 10,
								width: "100%",
								height: "50%",
								touchEnabled: false,
							}
						},
						{
							outlet: "date",
							style: {
								className: "Ti.UI.Label",
								top: 10,
								text: "---",
								color: "#5dbce2",
								font: {
									fontSize: 12
								},
								textAlign: Ti.UI.TEXT_ALIGNMENT_CENTER,
								touchEnabled: false
							}
						}		
					]
				},
				{
					style: {
						className : "Ti.UI.View",
						layout: "vertical",
						width: "60%",
						height: Ti.UI.FILL,
						touchEnabled: false,
						top: 5
					},
					subviews: [
						{
							outlet: "name",
							style : {
								className : "Ti.UI.Label",
								top: 10,
								left: 10,
								right: 10,
								color: "red",
								touchEnabled: false
							}
						},
						{
							outlet: "desc",
							style : {
								className : "Ti.UI.Label",
								top: 10,
								left: 10,
								right: 10,
								text : "...",
								color: "#777575",
								font : {
									fontSize : 13
								},
								touchEnabled: false
							}
						}	
					]
				},
				{
					outlet: "addBonus",
					style: {
						className: "Ti.UI.View",
						layout: "vertical",
						top: 3,
						right: 0,
						height: Ti.UI.FILL,
						touchEnabled: false
					}
				}
			]
		},
		{
			style: {
				className: "Ti.UI.View",
				top: -15,
				height: 3,
				backgroundImage: "%ResourcesPath%images/others/Divider.Horizontal.png",
				touchEnabled: false
			}
		}
	]
	
}