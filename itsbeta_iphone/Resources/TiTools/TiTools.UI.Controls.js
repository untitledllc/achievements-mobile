var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.UI.Preset', 'UI', 'Preset');

//---------------------------------------------//

function createAlertDialog(params)
{
	return Ti.UI.createAlertDialog(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.AlertDialog'
			} 
		)
	);
}

function createEmailDialog(params)
{
	return Ti.UI.createEmailDialog(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.EmailDialog'
			} 
		)
	);
}

function createActivityIndicator(params)
{
	return Ti.UI.createActivityIndicator(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ActivityIndicator'
			} 
		)
	);
}

function createTabGroup(params)
{
	var self = Ti.UI.createTabGroup(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TabGroup'
			}
		)
	);
	self.initialize = function(args)
	{
		var tabs = self.tabs;
		if(tabs != undefined)
		{
			for(var i = 0; i < tabs.length; i++)
			{
				var tab = tabs[i];
				if(tab.window != undefined)
				{
					switch(tab.window.className)
					{
						case 'Ti.UI.Window':
							tab.window.initialize(args);
						break;
						default:
							throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + '\n' + tab.window.className);
						break;
					}
				}
			}
		}
	}
	return self;
}

function createTab(params)
{
	return Ti.UI.createTab(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Tab'
			} 
		)
	);
}

function createWindow(params)
{
	var self = Ti.UI.createWindow(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Window'
			}
		)
	);
	self.initialize = function(args)
	{
		if(TiTools.Object.isString(params.main) == true)
		{
			var cnt = TiTools.Filesystem.loadModule(params.main);
			if(TiTools.Object.isFunction(cnt) == true)
			{
				cnt(self, args);
			}
			else if(TiTools.Object.isObject(cnt) == true)
			{
				if(TiTools.Object.isFunction(cnt.onInitController) == true)
				{
					cnt.onInitController(self, args);
				}
				if(TiTools.Object.isFunction(cnt.onWindowOpen) == true)
				{
					self.addEventListener("open",
						function(event)
						{
							cnt.onWindowOpen(self, event);
						}
					);
				}
				if(TiTools.Object.isFunction(cnt.onWindowClose) == true)
				{
					self.addEventListener("close",
						function(event)
						{
							cnt.onWindowClose(self, event);
						}
					);
				}
			}
		}
	};
	return self;
}

function createView(params)
{
	return Ti.UI.createView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.View'
			} 
		)
	);
}

function createScrollView(params)
{
	return Ti.UI.createScrollView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ScrollView'
			} 
		)
	);
}

function createScrollableView(params)
{
	return Ti.UI.createScrollableView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ScrollableView'
			} 
		)
	);
}

function createImageView(params)
{
	return Ti.UI.createImageView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ImageView'
			} 
		)
	);
}

function createButton(params)
{
	return Ti.UI.createButton(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Button'
			} 
		)
	);
}

function createButtonBar(params)
{
	return Ti.UI.createButtonBar(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ButtonBar'
			} 
		)
	);
}

function createLabel(params)
{
	return Ti.UI.createLabel(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Label'
			} 
		)
	);
}

function createSwitch(params)
{
	return Ti.UI.createSwitch(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Switch'
			} 
		)
	);
}

function createSlider(params)
{
	return Ti.UI.createSlider(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Slider'
			} 
		)
	);
}

function createSearchBar(params)
{
	return Ti.UI.createSearchBar(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.SearchBar'
			} 
		)
	);
}

function createProgressBar(params)
{
	return Ti.UI.createProgressBar(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.ProgressBar'
			} 
		)
	);
}

function createTextField(params)
{
	return Ti.UI.createTextField(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TextField'
			} 
		)
	);
}

function createTextArea(params)
{
	return Ti.UI.createTextArea(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TextArea'
			} 
		)
	);
}

function createTableView(params)
{
	return Ti.UI.createTableView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TableView'
			} 
		)
	);
}

function createTableViewSection(params)
{
	return Ti.UI.createTableViewSection(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TableViewSection'
			}
		)
	);
}

function createTableViewRow(params)
{
	return Ti.UI.createTableViewRow(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.TableViewRow'
			}
		)
	);
}

function createPicker(params)
{
	return Ti.UI.createPicker(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.Picker'
			} 
		)
	);
}

function createPickerColumn(params)
{
	return Ti.UI.createPickerColumn(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.PickerColumn'
			}
		)
	);
}

function createPickerRow(params)
{
	return Ti.UI.createPickerRow(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.PickerRow'
			}
		)
	);
}

function createWebView(params)
{
	return Ti.Map.createWebView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.WebView'
			}
		)
	);
}

function createGoogleMapView(params)
{
	return Ti.Map.createView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.GoogleMapView'
			}
		)
	);
}

function createGoogleMapViewAnnotation(params)
{
	return Ti.Map.createAnnotation(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.GoogleMapViewAnnotation'
			}
		)
	);
}

function createFacebookLoginButton(params)
{
	return Ti.Facebook.createLoginButton(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.UI.FacebookLoginButton'
			}
		)
	);
}

function createPaintView(params)
{
	var TiPaint = require('ti.paint');
	return TiPaint.createPaintView(
		TiTools.UI.Preset.merge(
			params,
			{
				uid : TiTools.Object.unigueID(),
				className : 'Ti.PaintView'
			}
		)
	);
}

//---------------------------------------------//
// Extension controls
//---------------------------------------------//
if(TiTools.Platform.isAndroid == true)
{
	Ti.include("TiTools/TiTools.UI.Controls.CreateButton3Ext.js");
	
	Ti.include("TiTools.UI.Controls.CreateButton9Ext.js");
	
	Ti.include("TiTools.UI.Controls.CreateButtonBarExt.js");
	 
	Ti.include("TiTools.UI.Controls.CreateProgressBarExt.js");
	
	Ti.include("TiTools.UI.Controls.CreateTabsExt.js");
	 
	Ti.include("TiTools.UI.Controls.CreateModalWindowList.js");
	
	Ti.include("TiTools.UI.Controls.CreateModalWindowDateWith.js");
	
	Ti.include("TiTools.UI.Controls.CreateModalWindowDateOrTime.js");
}
else
{
	if(TiTools.Platform.isIOS == true)
	{
		Ti.include("TiTools/TiTools.UI.Controls.CreateButton3Ext.js");
	
		Ti.include("TiTools/TiTools.UI.Controls.CreateButton9Ext.js");
		
		Ti.include("TiTools/TiTools.UI.Controls.CreateButtonBarExt.js");
		 
		Ti.include("TiTools/TiTools.UI.Controls.CreateProgressBarExt.js");
		
		Ti.include("TiTools/TiTools.UI.Controls.CreateTabsExt.js");
		 
		Ti.include("TiTools/TiTools.UI.Controls.CreateModalWindowList.js");
		
		Ti.include("TiTools/TiTools.UI.Controls.CreateModalWindowDateWith.js");
		
		Ti.include("TiTools/TiTools.UI.Controls.CreateModalWindowDateOrTime.js");
	}
	
}
//---------------------------------------------//

module.exports = {
	createAlertDialog : createAlertDialog,
	createEmailDialog : createEmailDialog,
	createActivityIndicator : createActivityIndicator,
	createTabGroup : createTabGroup,
	createTab : createTab,
	createWindow : createWindow,
	createView : createView,
	createScrollView : createScrollView,
	createScrollableView : createScrollableView,
	createImageView : createImageView,
	createButton : createButton,
	createButtonBar : createButtonBar,
	createLabel : createLabel,
	createSwitch : createSwitch,
	createSlider : createSlider,
	createSearchBar : createSearchBar,
	createProgressBar : createProgressBar,
	createTextField : createTextField,
	createTextArea : createTextArea,
	createTableView : createTableView,
	createTableViewSection : createTableViewSection,
	createTableViewRow : createTableViewRow,
	createPicker : createPicker,
	createPickerColumn : createPicker,
	createPickerRow : createPicker,
	createWebView : createWebView,
	createGoogleMapView : createGoogleMapView,
	createGoogleMapViewAnnotation : createGoogleMapViewAnnotation,
	createFacebookLoginButton : createFacebookLoginButton,
	createPaintView : createPaintView,
	Ext : {
		createButton3 : createButton3Ext,
		createButton9 : createButton9Ext,
		createButtonBar : createButtonBarExt,
		createProgressBar : createProgressBarExt,
		createTabs : createTabsExt,
		createModalWindowList : createModalWindowList,
		createModalWindowDateOrTime : createModalWindowDateOrTime,
		createModalWindowDateWith : createModalWindowDateWith
	}
};
