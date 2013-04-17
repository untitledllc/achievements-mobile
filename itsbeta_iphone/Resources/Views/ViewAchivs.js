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
				touchEnabled: false,
				bottom: -20
			},
			subviews: [
				{
					style: {
						className : "Ti.UI.View",
						layout: "vertical",
						width: "30%",
						height: Ti.UI.SIZE,
						touchEnabled: false,
						top: 0
					},
					subviews: [
						{
							style: {
								className: "Ti.UI.View",
								top: 10,
								width: 90,
								height: 90,
								touchEnabled: false,
								
							},
							subviews:
							[
								{
									outlet: "image",
									style: {
										className: "Ti.UI.ImageView",
										width: "100%",
										height: "100%",
										touchEnabled: false,
									}
								}
							]
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
						height: Ti.UI.SIZE,
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
						top: 15,
						right: 0,
						height: Ti.UI.SIZE,
						touchEnabled: false
					}
				}
			]
		},
		{
			style: {
				className: "Ti.UI.View",
				height: 3,
				backgroundImage: "%ResourcesPath%images/others/Divider.Horizontal.png",
				touchEnabled: false
			}
		}
	]
	
}