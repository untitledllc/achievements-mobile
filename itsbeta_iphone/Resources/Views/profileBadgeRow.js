module.exports = {
	outlet: "me",
	style : {
		className: "Ti.UI.View",
		top: 5,
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		layout: "vertical"
	},
	subviews: [
		{
			outlet: "title",
			style: {
				className: "Ti.UI.Label",
				left: 5,
				color: "#000",
				font: {fontSize: 12}
			}
		},
		{
			outlet: "progress",
			style: {
				className: "Ti.UI.Slider",
				top: 5,
			    min: 0,
			    max: 100,
			    height: 11,
			    rightTrackImage: "images/slider/tatusBar_norm.png",
			    leftTrackImage: "images/slider/tatusBar_active.png",
			    thumbImage: "images/slider/thumb.png",
			    width: '100%',
			    value: 10
			}
		}	
	]
}
