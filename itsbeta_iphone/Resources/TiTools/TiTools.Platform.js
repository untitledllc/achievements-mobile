//---------------------------------------------//

var SCREEN_MODE_UNKNOWN = 'Unknown';
var SCREEN_MODE_SMALL = 'Small';
var SCREEN_MODE_NORMAL = 'Normal';
var SCREEN_MODE_LARGE = 'Large';
var SCREEN_MODE_EXTRA_LARGE = 'ExtraLarge';

//---------------------------------------------//

var isSimulator = ((Ti.Platform.model == 'Simulator') || (Ti.Platform.model.indexOf('sdk') != -1));
var isAndroid = (Ti.Platform.osname == 'android');
var isIPhone = (Ti.Platform.osname == 'iphone');
var isIPad = (Ti.Platform.osname == 'ipad');
var isIOS = (isIPhone == true) || (isIPad == true);

//---------------------------------------------//

var screenWidth = Ti.Platform.displayCaps.platformWidth;
var screenHeight = Ti.Platform.displayCaps.platformHeight;
var screenResolution = screenWidth * screenHeight;
var screenDPI = Ti.Platform.displayCaps.dpi;
var screenMode = SCREEN_MODE_UNKNOWN;

//---------------------------------------------//

if(isAndroid == true)
{
	if(Ti.Platform.displayCaps.dpi <= 120)
	{
		screenMode = SCREEN_MODE_SMALL;
	}
	else if(Ti.Platform.displayCaps.dpi <= 160)
	{
		screenMode = SCREEN_MODE_NORMAL;
	}
	else if(Ti.Platform.displayCaps.dpi <= 240)
	{
		screenMode = SCREEN_MODE_LARGE;
	}
	else if(Ti.Platform.displayCaps.dpi <= 320)
	{
		screenMode = SCREEN_MODE_EXTRA_LARGE;
	}
}
else if(isIPhone == true)
{
	if((screenWidth == 320) && (screenHeight == 480))
	{
		screenMode = SCREEN_MODE_SMALL;
	}
	else if((screenWidth == 640) && (screenHeight == 960))
	{
		screenMode = SCREEN_MODE_NORMAL;
	}
	else if((screenWidth == 640) && (screenHeight == 1036))
	{
		screenMode = SCREEN_MODE_LARGE;
	}
}
else if(isIPad == true)
{
	if((screenWidth == 1024) && (screenHeight == 768))
	{
		screenMode = SCREEN_MODE_SMALL;
	}
	else if((screenWidth == 2048) && (screenHeight == 1536))
	{
		screenMode = SCREEN_MODE_NORMAL;
	}
}

//---------------------------------------------//

function appropriate(params)
{
	if(isAndroid == true)
	{
		if(params.android != undefined)
		{
			return params.android;
		}
	}
	else if(isIOS == true)
	{
		if(isIPhone == true)
		{
			if(params.iphone != undefined)
			{
				return params.iphone;
			}
		}
		else if(isIPad == true)
		{
			if(params.ipad != undefined)
			{
				return params.ipad;
			}
		}
		if(params.ios != undefined)
		{
			return params.ios;
		}
	}
	if(params.any != undefined)
	{
		return params.any;
	}
	return undefined;
}

//---------------------------------------------//

module.exports = {
	SCREEN_MODE : {
		UNKNOWN : SCREEN_MODE_UNKNOWN,
		SMALL : SCREEN_MODE_SMALL,
		NORMAL : SCREEN_MODE_NORMAL,
		LARGE : SCREEN_MODE_LARGE,
		EXTRA_LARGE : SCREEN_MODE_EXTRA_LARGE
	},
	isSimulator : isSimulator,
	isAndroid : isAndroid,
	isIPhone : isIPhone,
	isIPad : isIPad,
	isIOS : isIOS,
	screenWidth : screenWidth,
	screenHeight : screenHeight,
	screenResolution : screenResolution,
	screenDPI : screenDPI,
	screenMode : screenMode,
	appropriate : appropriate
};
