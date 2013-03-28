/**
 * @author Gom_Dzhabbar
 */
module.exports = {
	outlet: "viewAchivs",
	style : {
		className : "Ti.UI.View",
		height: Ti.UI.SIZE,
		layout: "vertical"
	},
	subviews: [
		{
			style : {
				className : "Ti.UI.View",
				height: Ti.UI.SIZE,
				layout: "horizontal",
				touchEnabled: false
			},
			subviews: [
				{
					style: {
						className : "Ti.UI.View",
						layout: "vertical",
						width: "20%",
						height: Ti.UI.SIZE,
						touchEnabled: false
					},
					subviews: [
						{
							outlet: "image",
							style: {
								className: "Ti.UI.ImageView",
								top: 10,
								height: 60,
								width: 60,
								image: "images/FirstBadge.PNG",
								touchEnabled: false
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
									fontSize: 10
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
						width: "70%",
						height: Ti.UI.SIZE,
						touchEnabled: false
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
						top: 20,
						//right: 0,
						height: Ti.UI.SIZE,
						width: "10%",
						backgroundColor: "yellow",
						touchEnabled: false
					}
				}
			]
		},
		{
			style: {
				className: "Ti.UI.View",
				height: 1,
				backgroundColor: "gray",
				touchEnabled: false
			}
		}
	]
	
}