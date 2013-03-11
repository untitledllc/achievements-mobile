var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.String', 'String');
TiTools.loadLibrary('TiTools/TiTools.DateTime', 'DateTime');
TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');
TiTools.loadLibrary('TiTools/TiTools.Filesystem', 'Filesystem');
TiTools.loadLibrary('TiTools/TiTools.Platform', 'Platform');
TiTools.loadLibrary('TiTools/TiTools.JSON', 'JSON');
TiTools.loadLibrary('TiTools/TiTools.XML', 'XML');

//---------------------------------------------//

var presetsNames  = [];
var presetsStyles = [];

//---------------------------------------------//

function set(name, style)
{
	var index = presetsNames.indexOf(name);
	if(index > -1)
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_OVERRIDE_PRESET') + '\n' + name);
	}
	presetsNames.push(name);
	presetsStyles.push(style);
}

//---------------------------------------------//

function get(name)
{
	var index = presetsNames.indexOf(name);
	if(index > -1)
	{ 
		return presetsStyles[index]; 
	}
	return undefined;
}

//---------------------------------------------//

function remove(name)
{
	var index = presetsNames.indexOf(name);
	if(index > -1)
	{
		presetsNames.splice(index, 1);
		presetsStyles.splice(index, 1);
	}
}
//---------------------------------------------//

function merge(params, defaults)
{
	var result = undefined;
	if(params != undefined)
	{
		result = TiTools.Object.clone(params);
		if(TiTools.Object.isArray(result.preset) == true)
		{
			for(var i = 0; i < result.preset.length; i++)
			{
				if(TiTools.Object.isString(result.preset[i]) == true)
				{
					var preset = get(result.preset[i]);
					if(preset != undefined)
					{
						result = TiTools.Object.combine(TiTools.Object.clone(preset), result);
					}
					else
					{
						Ti.API.warn(TiTools.Locate.getString('TITOOLS_WARNING_PRESET_NOT_FOUND') + ': ' + result.preset[i]);
					}
				}
			}
			delete result.preset;
		}
		else if(TiTools.Object.isString(result.preset) == true)
		{
			var preset = get(result.preset);
			if(preset != undefined)
			{
				result = TiTools.Object.combine(preset, result);
			}
			else
			{
				Ti.API.warn(TiTools.Locate.getString('TITOOLS_WARNING_PRESET_NOT_FOUND') + ': ' + result.preset);
			}
			delete result.preset;
		}
	}
	if(defaults != undefined)
	{
		result = TiTools.Object.combine(defaults, result);
	}
	return preprocess(result);
}

//---------------------------------------------//

function preprocess(params)
{
	for(var i in params)
	{
		if(TiTools.Object.isArray(params[i]) == true)
		{
			params[i] = preprocess(params[i]);
		}
		else if(TiTools.Object.isObject(params[i]) == true)
		{
			params[i] = preprocess(params[i]);
		}
		else if(TiTools.Object.isString(params[i]) == true)
		{
			params[i] = preprocessArgument(params[i]);
		}
	}
	if(params.margin != undefined)
	{
		if(TiTools.Object.isString(params.margin) == true)
		{
			var margins = params.margin.split(' ');
			if(margins.length > 0)
			{
				switch(margins.length)
				{
					case 2:
						params.top = (params.top != undefined) ? params.top : Number(margins[0]);
						params.right = (params.right != undefined) ? params.right : Number(margins[1]);
						params.bottom = (params.bottom != undefined) ? params.bottom : Number(margins[0]);
						params.left = (params.left != undefined) ? params.left : Number(margins[1]);
					break;
					case 3:
						params.top = (params.top != undefined) ? params.top : Number(margins[0]);
						params.right = (params.right != undefined) ? params.right : Number(margins[1]);
						params.bottom = (params.bottom != undefined) ? params.bottom : Number(margins[2]);
						params.left = (params.left != undefined) ? params.left : Number(margins[1]);
					break;
					case 4:
						params.top = (params.top != undefined) ? params.top : Number(margins[0]);
						params.right = (params.right != undefined) ? params.right : Number(margins[1]);
						params.bottom = (params.bottom != undefined) ? params.bottom : Number(margins[2]);
						params.left = (params.left != undefined) ? params.left : Number(margins[3]);
					break;
					default:
						params.top = (params.top != undefined) ? params.top : Number(margins[0]);
						params.right = (params.right != undefined) ? params.right : Number(margins[0]);
						params.bottom = (params.bottom != undefined) ? params.bottom : Number(margins[0]);
						params.left = (params.left != undefined) ? params.left : Number(margins[0]);
					break;
				}
			}
		}
		else if(TiTools.Object.isNumber(params.margin) == true)
		{
			params.top = (params.top != undefined) ? params.top : params.margin;
			params.right = (params.right != undefined) ? params.right : params.margin;
			params.bottom = (params.bottom != undefined) ? params.bottom : params.margin;
			params.left = (params.left != undefined) ? params.left : params.margin;
		}
		delete params.margin;
	}
	return params;
}

//---------------------------------------------//

function preprocessArgument(arg)
{
	if(TiTools.Object.isString(arg) == true)
	{
		arg = arg.replace(/LANG\(([A-Za-z0-9_\.]*)\)/g,
			function(str, p1, p2, offset, s)
			{
				return TiTools.Locate.getString(p1, p1);
			}
		);
		switch(arg)
		{
			case 'Ti.UI.FILL': return Ti.UI.FILL;
			case 'Ti.UI.SIZE': return Ti.UI.SIZE;
			case 'Ti.UI.PORTRAIT': return Ti.UI.PORTRAIT;
			case 'Ti.UI.UPSIDE_PORTRAIT': return Ti.UI.UPSIDE_PORTRAIT;
			case 'Ti.UI.LANDSCAPE_LEFT': return Ti.UI.LANDSCAPE_LEFT;
			case 'Ti.UI.LANDSCAPE_RIGHT': return Ti.UI.LANDSCAPE_RIGHT;
			case 'Ti.UI.INPUT_BORDERSTYLE_NONE': return Ti.UI.INPUT_BORDERSTYLE_NONE;
			case 'Ti.UI.INPUT_BORDERSTYLE_BEZEL': return Ti.UI.INPUT_BORDERSTYLE_BEZEL;
			case 'Ti.UI.INPUT_BORDERSTYLE_LINE': return Ti.UI.INPUT_BORDERSTYLE_LINE;
			case 'Ti.UI.INPUT_BORDERSTYLE_ROUNDED': return Ti.UI.INPUT_BORDERSTYLE_ROUNDED;
			case 'Ti.UI.INPUT_BUTTONMODE_ALWAYS': return Ti.UI.INPUT_BUTTONMODE_ALWAYS;
			case 'Ti.UI.INPUT_BUTTONMODE_NEVER': return Ti.UI.INPUT_BUTTONMODE_NEVER;
			case 'Ti.UI.INPUT_BUTTONMODE_ONBLUR': return Ti.UI.INPUT_BUTTONMODE_ONBLUR;
			case 'Ti.UI.INPUT_BUTTONMODE_ONFOCUS': return Ti.UI.INPUT_BUTTONMODE_ONFOCUS;
			case 'Ti.UI.PICKER_TYPE_PLAIN': return Ti.UI.PICKER_TYPE_PLAIN;
			case 'Ti.UI.PICKER_TYPE_DATE': return Ti.UI.PICKER_TYPE_DATE;
			case 'Ti.UI.PICKER_TYPE_TIME': return Ti.UI.PICKER_TYPE_TIME;
			case 'Ti.UI.PICKER_TYPE_DATE_AND_TIME': return Ti.UI.PICKER_TYPE_DATE_AND_TIME;
			case 'Ti.UI.PICKER_TYPE_COUNT_DOWN_TIMER': return Ti.UI.PICKER_TYPE_COUNT_DOWN_TIMER;
			case 'Ti.UI.TEXT_ALIGNMENT_LEFT': return Ti.UI.TEXT_ALIGNMENT_LEFT;
			case 'Ti.UI.TEXT_ALIGNMENT_CENTER': return Ti.UI.TEXT_ALIGNMENT_CENTER;
			case 'Ti.UI.TEXT_ALIGNMENT_RIGHT': return Ti.UI.TEXT_ALIGNMENT_RIGHT;
			case 'Ti.UI.TEXT_AUTOCAPITALIZATION_NONE': return Ti.UI.TEXT_AUTOCAPITALIZATION_NONE;
			case 'Ti.UI.TEXT_AUTOCAPITALIZATION_SENTENCES': return Ti.UI.TEXT_AUTOCAPITALIZATION_SENTENCES;
			case 'Ti.UI.TEXT_AUTOCAPITALIZATION_WORDS': return Ti.UI.TEXT_AUTOCAPITALIZATION_WORDS;
			case 'Ti.UI.TEXT_AUTOCAPITALIZATION_ALL': return Ti.UI.TEXT_AUTOCAPITALIZATION_ALL;
			case 'Ti.UI.TEXT_VERTICAL_ALIGNMENT_TOP': return Ti.UI.TEXT_VERTICAL_ALIGNMENT_TOP;
			case 'Ti.UI.TEXT_VERTICAL_ALIGNMENT_CENTER': return Ti.UI.TEXT_VERTICAL_ALIGNMENT_CENTER;
			case 'Ti.UI.TEXT_VERTICAL_ALIGNMENT_BOTTOM': return Ti.UI.TEXT_VERTICAL_ALIGNMENT_BOTTOM;
			case 'Ti.UI.KEYBOARD_DEFAULT': return Ti.UI.KEYBOARD_DEFAULT;
			case 'Ti.UI.KEYBOARD_ASCII': return Ti.UI.KEYBOARD_ASCII;
			case 'Ti.UI.KEYBOARD_EMAIL': return Ti.UI.KEYBOARD_EMAIL;
			case 'Ti.UI.KEYBOARD_URL': return Ti.UI.KEYBOARD_URL;
			case 'Ti.UI.KEYBOARD_APPEARANCE_ALERT': return Ti.UI.KEYBOARD_APPEARANCE_ALERT;
			case 'Ti.UI.KEYBOARD_APPEARANCE_DEFAULT': return Ti.UI.KEYBOARD_APPEARANCE_DEFAULT;
			case 'Ti.UI.KEYBOARD_NAMEPHONE_PAD': return Ti.UI.KEYBOARD_NAMEPHONE_PAD;
			case 'Ti.UI.KEYBOARD_NUMBER_PAD': return Ti.UI.KEYBOARD_NUMBER_PAD;
			case 'Ti.UI.KEYBOARD_NUMBERS_PUNCTUATION': return Ti.UI.KEYBOARD_NUMBERS_PUNCTUATION;
			case 'Ti.UI.KEYBOARD_DECIMAL_PAD': return Ti.UI.KEYBOARD_DECIMAL_PAD;
			case 'Ti.UI.KEYBOARD_PHONE_PAD': return Ti.UI.KEYBOARD_PHONE_PAD;
			case 'Ti.UI.ActivityIndicatorStyle.PLAIN': return Ti.UI.ActivityIndicatorStyle.PLAIN;
			case 'Ti.UI.ActivityIndicatorStyle.DARK': return Ti.UI.ActivityIndicatorStyle.DARK;
			case 'Ti.UI.ActivityIndicatorStyle.BIG': return Ti.UI.ActivityIndicatorStyle.BIG;
			case 'Ti.UI.ActivityIndicatorStyle.BIG_DARK': return Ti.UI.ActivityIndicatorStyle.BIG_DARK;
			case 'Ti.UI.iOS.AD_SIZE_LANDSCAPE': return Ti.UI.iOS.AD_SIZE_LANDSCAPE;
			case 'Ti.UI.iOS.AD_SIZE_PORTRAIT': return Ti.UI.iOS.AD_SIZE_PORTRAIT;
			case 'Ti.UI.iOS.AUTODETECT_ADDRESS': return Ti.UI.iOS.AUTODETECT_ADDRESS;
			case 'Ti.UI.iOS.AUTODETECT_ALL': return Ti.UI.iOS.AUTODETECT_ALL;
			case 'Ti.UI.iOS.AUTODETECT_CALENDAR': return Ti.UI.iOS.AUTODETECT_CALENDAR;
			case 'Ti.UI.iOS.AUTODETECT_LINK': return Ti.UI.iOS.AUTODETECT_LINK;
			case 'Ti.UI.iOS.AUTODETECT_NONE': return Ti.UI.iOS.AUTODETECT_NONE;
			case 'Ti.UI.iOS.AUTODETECT_PHONE': return Ti.UI.iOS.AUTODETECT_PHONE;
			case 'Ti.UI.iOS.BLEND_MODE_CLEAR': return Ti.UI.iOS.BLEND_MODE_CLEAR;
			case 'Ti.UI.iOS.BLEND_MODE_COLOR': return Ti.UI.iOS.BLEND_MODE_COLOR;
			case 'Ti.UI.iOS.BLEND_MODE_COLOR_BURN': return Ti.UI.iOS.BLEND_MODE_COLOR_BURN;
			case 'Ti.UI.iOS.BLEND_MODE_COLOR_DODGE': return Ti.UI.iOS.BLEND_MODE_COLOR_DODGE;
			case 'Ti.UI.iOS.BLEND_MODE_COPY': return Ti.UI.iOS.BLEND_MODE_COPY;
			case 'Ti.UI.iOS.BLEND_MODE_DARKEN': return Ti.UI.iOS.BLEND_MODE_DARKEN;
			case 'Ti.UI.iOS.BLEND_MODE_DESTINATION_ATOP': return Ti.UI.iOS.BLEND_MODE_DESTINATION_ATOP;
			case 'Ti.UI.iOS.BLEND_MODE_DESTINATION_IN': return Ti.UI.iOS.BLEND_MODE_DESTINATION_IN;
			case 'Ti.UI.iOS.BLEND_MODE_DESTINATION_OUT': return Ti.UI.iOS.BLEND_MODE_DESTINATION_OUT;
			case 'Ti.UI.iOS.BLEND_MODE_DESTINATION_OVER': return Ti.UI.iOS.BLEND_MODE_DESTINATION_OVER;
			case 'Ti.UI.iOS.BLEND_MODE_DIFFERENCE': return Ti.UI.iOS.BLEND_MODE_DIFFERENCE;
			case 'Ti.UI.iOS.BLEND_MODE_EXCLUSION': return Ti.UI.iOS.BLEND_MODE_EXCLUSION;
			case 'Ti.UI.iOS.BLEND_MODE_HARD_LIGHT': return Ti.UI.iOS.BLEND_MODE_HARD_LIGHT;
			case 'Ti.UI.iOS.BLEND_MODE_HUE': return Ti.UI.iOS.BLEND_MODE_HUE;
			case 'Ti.UI.iOS.BLEND_MODE_LIGHTEN': return Ti.UI.iOS.BLEND_MODE_LIGHTEN;
			case 'Ti.UI.iOS.BLEND_MODE_LUMINOSITY': return Ti.UI.iOS.BLEND_MODE_LUMINOSITY;
			case 'Ti.UI.iOS.BLEND_MODE_MULTIPLY': return Ti.UI.iOS.BLEND_MODE_MULTIPLY;
			case 'Ti.UI.iOS.BLEND_MODE_NORMAL': return Ti.UI.iOS.BLEND_MODE_NORMAL;
			case 'Ti.UI.iOS.BLEND_MODE_OVERLAY': return Ti.UI.iOS.BLEND_MODE_OVERLAY;
			case 'Ti.UI.iOS.BLEND_MODE_PLUS_DARKER': return Ti.UI.iOS.BLEND_MODE_PLUS_DARKER;
			case 'Ti.UI.iOS.BLEND_MODE_PLUS_LIGHTER': return Ti.UI.iOS.BLEND_MODE_PLUS_LIGHTER;
			case 'Ti.UI.iOS.BLEND_MODE_SATURATION': return Ti.UI.iOS.BLEND_MODE_SATURATION;
			case 'Ti.UI.iOS.BLEND_MODE_SCREEN': return Ti.UI.iOS.BLEND_MODE_SCREEN;
			case 'Ti.UI.iOS.BLEND_MODE_SOFT_LIGHT': return Ti.UI.iOS.BLEND_MODE_SOFT_LIGHT;
			case 'Ti.UI.iOS.BLEND_MODE_SOURCE_ATOP': return Ti.UI.iOS.BLEND_MODE_SOURCE_ATOP;
			case 'Ti.UI.iOS.BLEND_MODE_SOURCE_IN': return Ti.UI.iOS.BLEND_MODE_SOURCE_IN;
			case 'Ti.UI.iOS.BLEND_MODE_SOURCE_OUT': return Ti.UI.iOS.BLEND_MODE_SOURCE_OUT;
			case 'Ti.UI.iOS.BLEND_MODE_XOR': return Ti.UI.iOS.BLEND_MODE_XOR;
			case 'Ti.UI.iOS.COLOR_GROUP_TABLEVIEW_BACKGROUND': return Ti.UI.iOS.COLOR_GROUP_TABLEVIEW_BACKGROUND;
			case 'Ti.UI.iOS.COLOR_SCROLLVIEW_BACKGROUND': return Ti.UI.iOS.COLOR_SCROLLVIEW_BACKGROUND;
			case 'Ti.UI.iOS.COLOR_UNDER_PAGE_BACKGROUND': return Ti.UI.iOS.COLOR_UNDER_PAGE_BACKGROUND;
			case 'Ti.UI.iOS.COLOR_VIEW_FLIPSIDE_BACKGROUND': return Ti.UI.iOS.COLOR_VIEW_FLIPSIDE_BACKGROUND;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_UNKNOWN': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_UNKNOWN;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_UP': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_UP;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_RIGHT': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_RIGHT;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_DOWN': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_DOWN;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_LEFT': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_LEFT;
			case 'Ti.UI.iPad.POPOVER_ARROW_DIRECTION_ANY': return Ti.UI.iPad.POPOVER_ARROW_DIRECTION_ANY;
			case 'Ti.UI.iPhone.MODAL_PRESENTATION_CURRENT_CONTEXT': return Ti.UI.iPhone.MODAL_PRESENTATION_CURRENT_CONTEXT;
			case 'Ti.UI.iPhone.MODAL_PRESENTATION_FORMSHEET': return Ti.UI.iPhone.MODAL_PRESENTATION_FORMSHEET;
			case 'Ti.UI.iPhone.MODAL_PRESENTATION_FULLSCREEN': return Ti.UI.iPhone.MODAL_PRESENTATION_FULLSCREEN;
			case 'Ti.UI.iPhone.MODAL_PRESENTATION_PAGESHEET': return Ti.UI.iPhone.MODAL_PRESENTATION_PAGESHEET;
			case 'Ti.UI.iPhone.MODAL_TRANSITION_STYLE_COVER_VERTICAL': return Ti.UI.iPhone.MODAL_TRANSITION_STYLE_COVER_VERTICAL;
			case 'Ti.UI.iPhone.MODAL_TRANSITION_STYLE_CROSS_DISSOLVE': return Ti.UI.iPhone.MODAL_TRANSITION_STYLE_CROSS_DISSOLVE;
			case 'Ti.UI.iPhone.MODAL_TRANSITION_STYLE_FLIP_HORIZONTAL': return Ti.UI.iPhone.MODAL_TRANSITION_STYLE_FLIP_HORIZONTAL;
			case 'Ti.UI.iPhone.MODAL_TRANSITION_STYLE_PARTIAL_CURL': return Ti.UI.iPhone.MODAL_TRANSITION_STYLE_PARTIAL_CURL;
			case 'Ti.UI.iPhone.ActivityIndicatorStyle.PLAIN': return Ti.UI.iPhone.ActivityIndicatorStyle.PLAIN;
			case 'Ti.UI.iPhone.ActivityIndicatorStyle.DARK': return Ti.UI.iPhone.ActivityIndicatorStyle.DARK;
			case 'Ti.UI.iPhone.ActivityIndicatorStyle.BIG': return Ti.UI.iPhone.ActivityIndicatorStyle.BIG;
			case 'Ti.UI.iPhone.AnimationStyle.NONE': return Ti.UI.iPhone.AnimationStyle.NONE;
			case 'Ti.UI.iPhone.AnimationStyle.CURL_UP': return Ti.UI.iPhone.AnimationStyle.CURL_UP;
			case 'Ti.UI.iPhone.AnimationStyle.CURL_DOWN': return Ti.UI.iPhone.AnimationStyle.CURL_DOWN;
			case 'Ti.UI.iPhone.AnimationStyle.FLIP_FROM_LEFT': return Ti.UI.iPhone.AnimationStyle.FLIP_FROM_LEFT;
			case 'Ti.UI.iPhone.AnimationStyle.FLIP_FROM_RIGHT': return Ti.UI.iPhone.AnimationStyle.FLIP_FROM_RIGHT;
			case 'Ti.UI.iPhone.ProgressBarStyle.DEFAULT': return Ti.UI.iPhone.TableViewSeparatorStyle.DEFAULT;
			case 'Ti.UI.iPhone.ProgressBarStyle.PLAIN': return Ti.UI.iPhone.TableViewSeparatorStyle.PLAIN;
			case 'Ti.UI.iPhone.ProgressBarStyle.BAR': return Ti.UI.iPhone.TableViewSeparatorStyle.BAR;
			case 'Ti.UI.iPhone.RowAnimationStyle.NONE': return Ti.UI.iPhone.RowAnimationStyle.NONE;
			case 'Ti.UI.iPhone.RowAnimationStyle.FADE': return Ti.UI.iPhone.RowAnimationStyle.FADE;
			case 'Ti.UI.iPhone.RowAnimationStyle.TOP': return Ti.UI.iPhone.RowAnimationStyle.TOP;
			case 'Ti.UI.iPhone.RowAnimationStyle.RIGHT': return Ti.UI.iPhone.RowAnimationStyle.RIGHT;
			case 'Ti.UI.iPhone.RowAnimationStyle.BOTTOM': return Ti.UI.iPhone.RowAnimationStyle.BOTTOM;
			case 'Ti.UI.iPhone.RowAnimationStyle.LEFT': return Ti.UI.iPhone.RowAnimationStyle.LEFT;
			case 'Ti.UI.iPhone.ScrollIndicatorStyle.DEFAULT': return Ti.UI.iPhone.ScrollIndicatorStyle.DEFAULT;
			case 'Ti.UI.iPhone.ScrollIndicatorStyle.BLACK': return Ti.UI.iPhone.ScrollIndicatorStyle.BLACK;
			case 'Ti.UI.iPhone.ScrollIndicatorStyle.WHITE': return Ti.UI.iPhone.ScrollIndicatorStyle.WHITE;
			case 'Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_NONE': return Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_NONE;
			case 'Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_FADE': return Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_FADE;
			case 'Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_SLIDE': return Ti.UI.iPhone.StatusBar.ANIMATION_STYLE_SLIDE;
			case 'Ti.UI.iPhone.StatusBar.DEFAULT': return Ti.UI.iPhone.StatusBar.DEFAULT;
			case 'Ti.UI.iPhone.StatusBar.GRAY': return Ti.UI.iPhone.StatusBar.GRAY;
			case 'Ti.UI.iPhone.StatusBar.GREY': return Ti.UI.iPhone.StatusBar.GREY;
			case 'Ti.UI.iPhone.StatusBar.OPAQUE_BLACK': return Ti.UI.iPhone.StatusBar.OPAQUE_BLACK;
			case 'Ti.UI.iPhone.StatusBar.TRANSLUCENT_BLACK': return Ti.UI.iPhone.StatusBar.TRANSLUCENT_BLACK;
			case 'Ti.UI.iPhone.SystemButton.ACTION': return Ti.UI.iPhone.SystemButton.ACTION;
			case 'Ti.UI.iPhone.SystemButton.ACTIVITY': return Ti.UI.iPhone.SystemButton.ACTIVITY;
			case 'Ti.UI.iPhone.SystemButton.ADD': return Ti.UI.iPhone.SystemButton.ADD;
			case 'Ti.UI.iPhone.SystemButton.BOOKMARKS': return Ti.UI.iPhone.SystemButton.BOOKMARKS;
			case 'Ti.UI.iPhone.SystemButton.CAMERA': return Ti.UI.iPhone.SystemButton.CAMERA;
			case 'Ti.UI.iPhone.SystemButton.CANCEL': return Ti.UI.iPhone.SystemButton.CANCEL;
			case 'Ti.UI.iPhone.SystemButton.COMPOSE': return Ti.UI.iPhone.SystemButton.COMPOSE;
			case 'Ti.UI.iPhone.SystemButton.CONTACT_ADD': return Ti.UI.iPhone.SystemButton.CONTACT_ADD;
			case 'Ti.UI.iPhone.SystemButton.DISCLOSURE': return Ti.UI.iPhone.SystemButton.DISCLOSURE;
			case 'Ti.UI.iPhone.SystemButton.DONE': return Ti.UI.iPhone.SystemButton.DONE;
			case 'Ti.UI.iPhone.SystemButton.EDIT': return Ti.UI.iPhone.SystemButton.EDIT;
			case 'Ti.UI.iPhone.SystemButton.FAST_FORWARD': return Ti.UI.iPhone.SystemButton.FAST_FORWARD;
			case 'Ti.UI.iPhone.SystemButton.FIXED_SPACE': return Ti.UI.iPhone.SystemButton.FIXED_SPACE;
			case 'Ti.UI.iPhone.SystemButton.FLEXIBLE_SPACE': return Ti.UI.iPhone.SystemButton.FLEXIBLE_SPACE;
			case 'Ti.UI.iPhone.SystemButton.INFO_DARK': return Ti.UI.iPhone.SystemButton.INFO_DARK;
			case 'Ti.UI.iPhone.SystemButton.INFO_LIGHT': return Ti.UI.iPhone.SystemButton.INFO_LIGHT;
			case 'Ti.UI.iPhone.SystemButton.ORGANIZE': return Ti.UI.iPhone.SystemButton.ORGANIZE;
			case 'Ti.UI.iPhone.SystemButton.PAUSE': return Ti.UI.iPhone.SystemButton.PAUSE;
			case 'Ti.UI.iPhone.SystemButton.PLAY': return Ti.UI.iPhone.SystemButton.PLAY;
			case 'Ti.UI.iPhone.SystemButton.REFRESH': return Ti.UI.iPhone.SystemButton.REFRESH;
			case 'Ti.UI.iPhone.SystemButton.REPLY': return Ti.UI.iPhone.SystemButton.REPLY;
			case 'Ti.UI.iPhone.SystemButton.REWIND': return Ti.UI.iPhone.SystemButton.REWIND;
			case 'Ti.UI.iPhone.SystemButton.SAVE': return Ti.UI.iPhone.SystemButton.SAVE;
			case 'Ti.UI.iPhone.SystemButton.SPINNER': return Ti.UI.iPhone.SystemButton.SPINNER;
			case 'Ti.UI.iPhone.SystemButton.STOP': return Ti.UI.iPhone.SystemButton.STOP;
			case 'Ti.UI.iPhone.SystemButton.TRASH': return Ti.UI.iPhone.SystemButton.TRASH;
			case 'Ti.UI.iPhone.SystemButtonStyle.PLAIN': return Ti.UI.iPhone.SystemButtonStyle.PLAIN;
			case 'Ti.UI.iPhone.SystemButtonStyle.DONE': return Ti.UI.iPhone.SystemButtonStyle.DONE;
			case 'Ti.UI.iPhone.SystemButtonStyle.BAR': return Ti.UI.iPhone.SystemButtonStyle.BAR;
			case 'Ti.UI.iPhone.SystemButtonStyle.BORDERED': return Ti.UI.iPhone.SystemButtonStyle.BORDERED;
			case 'Ti.UI.iPhone.TableViewScrollPosition.NONE': return Ti.UI.iPhone.TableViewScrollPosition.NONE;
			case 'Ti.UI.iPhone.TableViewScrollPosition.TOP': return Ti.UI.iPhone.TableViewScrollPosition.TOP;
			case 'Ti.UI.iPhone.TableViewScrollPosition.MIDDLE': return Ti.UI.iPhone.TableViewScrollPosition.MIDDLE;
			case 'Ti.UI.iPhone.TableViewScrollPosition.BOTTOM': return Ti.UI.iPhone.TableViewScrollPosition.BOTTOM;
			case 'Ti.UI.iPhone.TableViewStyle.GROUPED': return Ti.UI.iPhone.TableViewStyle.GROUPED;
			case 'Ti.UI.iPhone.TableViewStyle.PLAIN': return Ti.UI.iPhone.TableViewStyle.PLAIN;
			case 'Ti.UI.iPhone.TableViewSeparatorStyle.NONE': return Ti.UI.iPhone.TableViewSeparatorStyle.NONE;
			case 'Ti.UI.iPhone.TableViewSeparatorStyle.SINGLE_LINE': return Ti.UI.iPhone.TableViewSeparatorStyle.SINGLE_LINE;
			case 'Ti.UI.Android.LINKIFY_ALL': return Ti.UI.Android.LINKIFY_ALL;
			case 'Ti.UI.Android.LINKIFY_EMAIL_ADDRESSES': return Ti.UI.Android.LINKIFY_EMAIL_ADDRESSES;
			case 'Ti.UI.Android.LINKIFY_MAP_ADDRESSES': return Ti.UI.Android.LINKIFY_MAP_ADDRESSES;
			case 'Ti.UI.Android.LINKIFY_PHONE_NUMBERS': return Ti.UI.Android.LINKIFY_PHONE_NUMBERS;
			case 'Ti.UI.Android.LINKIFY_WEB_URLS': return Ti.UI.Android.LINKIFY_WEB_URLS;
			case 'Ti.UI.Android.PIXEL_FORMAT_A_8': return Ti.UI.Android.PIXEL_FORMAT_A_8;
			case 'Ti.UI.Android.PIXEL_FORMAT_LA_88': return Ti.UI.Android.PIXEL_FORMAT_LA_88;
			case 'Ti.UI.Android.PIXEL_FORMAT_L_8': return Ti.UI.Android.PIXEL_FORMAT_L_8;
			case 'Ti.UI.Android.PIXEL_FORMAT_OPAQUE': return Ti.UI.Android.PIXEL_FORMAT_OPAQUE;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGBA_4444': return Ti.UI.Android.PIXEL_FORMAT_RGBA_4444;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGBA_5551': return Ti.UI.Android.PIXEL_FORMAT_RGBA_5551;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGBA_8888': return Ti.UI.Android.PIXEL_FORMAT_RGBA_8888;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGBX_8888': return Ti.UI.Android.PIXEL_FORMAT_RGBX_8888;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGB_332': return Ti.UI.Android.PIXEL_FORMAT_RGB_332;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGB_565': return Ti.UI.Android.PIXEL_FORMAT_RGB_565;
			case 'Ti.UI.Android.PIXEL_FORMAT_RGB_888': return Ti.UI.Android.PIXEL_FORMAT_RGB_888;
			case 'Ti.UI.Android.PIXEL_FORMAT_TRANSLUCENT': return Ti.UI.Android.PIXEL_FORMAT_TRANSLUCENT;
			case 'Ti.UI.Android.PIXEL_FORMAT_TRANSPARENT': return Ti.UI.Android.PIXEL_FORMAT_TRANSPARENT;
			case 'Ti.UI.Android.PIXEL_FORMAT_UNKNOWN': return Ti.UI.Android.PIXEL_FORMAT_UNKNOWN;
			case 'Ti.UI.Android.SOFT_INPUT_ADJUST_PAN': return Ti.UI.Android.SOFT_INPUT_ADJUST_PAN;
			case 'Ti.UI.Android.SOFT_INPUT_ADJUST_RESIZE': return Ti.UI.Android.SOFT_INPUT_ADJUST_RESIZE;
			case 'Ti.UI.Android.SOFT_INPUT_ADJUST_UNSPECIFIED': return Ti.UI.Android.SOFT_INPUT_ADJUST_UNSPECIFIED;
			case 'Ti.UI.Android.SOFT_INPUT_STATE_ALWAYS_HIDDEN': return Ti.UI.Android.SOFT_INPUT_STATE_ALWAYS_HIDDEN;
			case 'Ti.UI.Android.SOFT_INPUT_STATE_ALWAYS_VISIBLE': return Ti.UI.Android.SOFT_INPUT_STATE_ALWAYS_VISIBLE;
			case 'Ti.UI.Android.SOFT_INPUT_STATE_HIDDEN': return Ti.UI.Android.SOFT_INPUT_STATE_HIDDEN;
			case 'Ti.UI.Android.SOFT_INPUT_STATE_UNSPECIFIED': return Ti.UI.Android.SOFT_INPUT_STATE_UNSPECIFIED;
			case 'Ti.UI.Android.SOFT_INPUT_STATE_VISIBLE': return Ti.UI.Android.SOFT_INPUT_STATE_VISIBLE;
			case 'Ti.UI.Android.SOFT_KEYBOARD_DEFAULT_ON_FOCUS': return Ti.UI.Android.SOFT_KEYBOARD_DEFAULT_ON_FOCUS;
			case 'Ti.UI.Android.SOFT_KEYBOARD_HIDE_ON_FOCUS': return Ti.UI.Android.SOFT_KEYBOARD_HIDE_ON_FOCUS;
			case 'Ti.UI.Android.SOFT_KEYBOARD_SHOW_ON_FOCUS': return Ti.UI.Android.SOFT_KEYBOARD_SHOW_ON_FOCUS;
			case 'Ti.UI.Android.SWITCH_STYLE_CHECKBOX': return Ti.UI.Android.SWITCH_STYLE_CHECKBOX;
			case 'Ti.UI.Android.SWITCH_STYLE_TOGGLEBUTTON': return Ti.UI.Android.SWITCH_STYLE_TOGGLEBUTTON;
			case 'Ti.UI.Android.WEBVIEW_PLUGINS_OFF': return Ti.UI.Android.WEBVIEW_PLUGINS_OFF;
			case 'Ti.UI.Android.WEBVIEW_PLUGINS_ON': return Ti.UI.Android.WEBVIEW_PLUGINS_ON;
			case 'Ti.UI.Android.WEBVIEW_PLUGINS_ON_DEMAND': return Ti.UI.Android.WEBVIEW_PLUGINS_ON_DEMAND;
			default: return TiTools.Filesystem.preprocessPath(arg);
		}
	}
	return arg;
}

//---------------------------------------------//

function applyByName(object, name)
{
	var params = {};
	if(TiTools.Object.isArray(name) == true)
	{
		for(var i = 0; i < name.length; i++)
		{
			if(TiTools.Object.isString(name[i]) == true)
			{
				var preset = get(name[i]);
				if(preset != undefined)
				{
					params = TiTools.Object.combine(preset, params);
				}
			}
		}
	}
	else if(TiTools.Object.isString(name) == true)
	{
		var preset = get(name);
		if(preset != undefined)
		{
			params = TiTools.Object.combine(preset, params);
		}
	}
	apply(object, preprocess(params));
}

//---------------------------------------------//

function apply(object, params)
{
	for(var i in params)
	{
		if(TiTools.Object.isArray(params[i]) == true)
		{
			apply(object[i], params[i]);
		}
		else if(TiTools.Object.isObject(params[i]) == true)
		{
			apply(object[i], params[i]);
		}
		else
		{
			object[i] = params[i];
		}
	}
}

//---------------------------------------------//

function load(params)
{
	if(TiTools.Object.isArray(params) == true)
	{
		for(var i = 0; i < params.length; i++)
		{
			load(params[i]);
		}
	}
	else if(TiTools.Object.isObject(params) == true)
	{
		var current = TiTools.Platform.appropriate(params);
		if(current == undefined)
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_PLATFORM'));
		}
		load(current);
	}
	else if(TiTools.Object.isString(params) == true)
	{
		loadFromFilename(params);
	}
}

//---------------------------------------------//

function loadFromFilename(filename)
{
	if(TiTools.String.isSuffix(filename, '.js') == true)
	{
		var module = TiTools.Filesystem.loadModule(filename);
		if(TiTools.Object.isArray(module) == true)
		{
			for(var j = 0; j < module.length; j++)
			{
				loadFromJSON(module[j]);
			}
		}
		else if(TiTools.Object.isObject(module) == true)
		{
			loadFromJSON(module);
		}
	}
	else if(TiTools.String.isSuffix(filename, '.json') == true)
	{
		var file = TiTools.Filesystem.getFile(filename);
		if(file.exists() == true)
		{
			var blob = file.read();
			var content = TiTools.JSON.deserialize(blob.text);
			if(TiTools.Object.isArray(content) == true)
			{
				for(var j = 0; j < content.length; j++)
				{
					loadFromJSON(content[j]);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				loadFromJSON(content);
			}
		}
		else
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_NOT_FOUND') + '\n' + filename);
		}
	}
	else if(TiTools.String.isSuffix(filename, '.xml') == true)
	{
		var file = TiTools.Filesystem.getFile(filename);
		if(file.exists() == true)
		{
			var blob = file.read();
			var content = TiTools.XML.deserialize(blob.text);
			if(TiTools.Object.isArray(content) == true)
			{
				for(var j = 0; j < content.length; j++)
				{
					loadFromXML(content[j]);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				loadFromXML(content);
			}
		}
		else
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_NOT_FOUND') + '\n' + filename);
		}
	}
	else
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_EXTENSION') + '\n' + filename);
	}
}

//---------------------------------------------//

function loadFromJSON(content)
{
	if((TiTools.Object.isString(content.name) == false) || (TiTools.Object.isObject(content.style) == false))
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_PRESET_FORMAT'));
	}
	set(content.name, content.style);
}

//---------------------------------------------//

function loadFromXML(content)
{
}

//---------------------------------------------//

module.exports = {
	set : set,
	get : get,
	remove : remove,
	merge : merge,
	applyByName : applyByName,
	apply : apply,
	load : load
};
