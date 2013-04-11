exports = {
	outlet: "me",
	style: {
		className: "Ti.UI.View",
		backgroundColor: "#ededed",
		height: 80
	},
	subviews: [
		{
			outlet: "hypno",
			style: {
				className: "Ti.UI.ImageView",
				images: [
					"images/pullToRefresh/step_00.png",
					"images/pullToRefresh/step_01.png",
					"images/pullToRefresh/step_02.png",
					"images/pullToRefresh/step_03.png",
					"images/pullToRefresh/step_04.png",
					"images/pullToRefresh/step_05.png",
					"images/pullToRefresh/step_06.png",
					"images/pullToRefresh/step_07.png",
					"images/pullToRefresh/step_08.png",
					"images/pullToRefresh/step_09.png",
					"images/pullToRefresh/step_10.png",
					"images/pullToRefresh/step_11.png",
					"images/pullToRefresh/step_12.png",
					"images/pullToRefresh/step_13.png",
					"images/pullToRefresh/step_14.png",
					"images/pullToRefresh/step_15.png",
					"images/pullToRefresh/step_16.png",
					"images/pullToRefresh/step_17.png",
					"images/pullToRefresh/step_18.png",
					"images/pullToRefresh/step_19.png",
					"images/pullToRefresh/step_20.png",
					"images/pullToRefresh/step_21.png",
					"images/pullToRefresh/step_22.png",
					"images/pullToRefresh/step_23.png",
					"images/pullToRefresh/step_24.png",
					"images/pullToRefresh/step_25.png",
					"images/pullToRefresh/step_26.png",
					"images/pullToRefresh/step_27.png",
					"images/pullToRefresh/step_28.png",
					"images/pullToRefresh/step_29.png",
					"images/pullToRefresh/step_30.png",
					"images/pullToRefresh/step_31.png",
					"images/pullToRefresh/step_32.png",
					"images/pullToRefresh/step_33.png",
					"images/pullToRefresh/step_34.png",
					"images/pullToRefresh/step_35.png",
					"images/pullToRefresh/step_36.png",
					"images/pullToRefresh/step_37.png",
					"images/pullToRefresh/step_38.png",
					"images/pullToRefresh/step_39.png",
					"images/pullToRefresh/step_40.png",
					"images/pullToRefresh/step_41.png",
					"images/pullToRefresh/step_42.png",
					"images/pullToRefresh/step_43.png",
					"images/pullToRefresh/step_44.png",
					"images/pullToRefresh/step_45.png",
					"images/pullToRefresh/step_46.png",
					"images/pullToRefresh/step_47.png",
					"images/pullToRefresh/step_48.png",
				],
				duration: 25, 
				width: 50,
				height: 50,
				repeatCount: 0,
				top: 5
			}
	 	},		
	 	{
	 		outlet: "status",
	 		style: {
	 			className: "Ti.UI.Label",
	 			text: "Pull to refresh",
				top: 55,
				color: "#737474",
				font: {fontSize: "14pt"}
	 		}
	 	},
	 	{
	 		outlet: "refreshing",
	 		style: {
	 			className: "Ti.UI.Label",
	 			text: "Refreshing",
	 			visible: false,
				top: 55,
				color: "#737474",
				font: {fontSize: "14pt"}
	 		}
	 	},
	 	{
			style: {
				className: "Ti.UI.View",
				height: 3,
				bottom: 0,
				backgroundImage: "%ResourcesPath%images/others/Divider.Horizontal.png",
				touchEnabled: false
			}
		}
	]
}
