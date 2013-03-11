var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.String', 'String');
TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');
TiTools.loadLibrary('TiTools/TiTools.Filesystem', 'Filesystem');
TiTools.loadLibrary('TiTools/TiTools.Platform', 'Platform');
TiTools.loadLibrary('TiTools/TiTools.JSON', 'JSON');
TiTools.loadLibrary('TiTools/TiTools.XML', 'XML');
TiTools.loadLibrary('TiTools/TiTools.Utils', 'Utils');

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.UI.Controls', 'UI', 'Controls');
TiTools.loadLibrary('TiTools/TiTools.UI.Prefab', 'UI', 'Prefab');
TiTools.loadLibrary('TiTools/TiTools.UI.Preset', 'UI', 'Preset');

//---------------------------------------------//

var loadersNames  = [];
var loadersCaches = [];

//---------------------------------------------//

function preloadSet(name, cache)
{
	var index = loadersNames.indexOf(name);
	if(index > -1)
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_OVERRIDE_PRESET') + '\n' + name);
	}
	loadersNames.push(name);
	loadersCaches.push(cache);
}

//---------------------------------------------//

function preloadGet(name)
{
	var index = loadersNames.indexOf(name);
	if(index > -1)
	{
		return loadersCaches[index];
	}
	return undefined;
}

//---------------------------------------------//

function preloadRemove(name)
{
	var index = loadersNames.indexOf(name);
	if(index > -1)
	{		
		loadersNames.splice(index, 1);
		loadersCaches.splice(index, 1);
	}
}

//---------------------------------------------//

function preload(params)
{
	if(TiTools.Object.isArray(params) == true)
	{
		for(var i = 0; i < params.length; i++)
		{
			preload(params[i]);
		}
	}
	else if(TiTools.Object.isObject(params) == true)
	{
		var current = TiTools.Platform.appropriate(params);
		if(current == undefined)
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_PLATFORM'));
		}
		preload(current);
	}
	else if(TiTools.Object.isString(params) == true)
	{
		preloadFromFilename(params);
	}
}

//---------------------------------------------//

function preloadFromFilename(filename)
{
	if(TiTools.String.isSuffix(filename, '.js') == true)
	{
		var content = TiTools.Filesystem.loadModule(filename);
		if(TiTools.Object.isArray(content) == true)
		{
			for(var i = 0; i < content.length; i++)
			{
				content[i] = preloadFromJSON(content[i]);
			}
		}
		else if(TiTools.Object.isObject(content) == true)
		{
			content = preloadFromJSON(content);
		}
		preloadSet(filename, content);
		return content;
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
				for(var i = 0; i < content.length; i++)
				{
					content[i] = preloadFromJSON(content[i]);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				content = preloadFromJSON(content);
			}
			preloadSet(filename, content);
			return content;
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
				for(var i = 0; i < content.length; i++)
				{
					content[i] = preloadFromXML(content[i]);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				content = preloadFromXML(content);
			}
			preloadSet(filename, content);
			return content;
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
	return undefined;
}

//---------------------------------------------//

function preloadFromJSON(content)
{
	if(content.prefab != undefined)
	{
		if(TiTools.Object.isString(content.prefab) == true)
		{
			var prefab = TiTools.UI.Prefab.get(content.prefab);
			if(prefab != undefined)
			{
				content = TiTools.Object.combine(content, prefab);
			}
			else
			{
				Ti.API.warn(TiTools.Locate.getString('TITOOLS_WARNING_PREFAB_NOT_FOUND') + ': ' + content.prefab);
			}
			delete content.prefab;
		}
		else
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_PREFAB_FORMAT') + ': ' + content.prefab);
		}
	}
	if(content.style != undefined)
	{
		content.style = TiTools.UI.Preset.merge(content.style);
	}
	if(content.root != undefined)
	{
		content.root = preloadFromJSON(content.root);
	}
	if(content.header != undefined)
	{
		content.header = preloadFromJSON(content.header);
	}
	if(content.footer != undefined)
	{
		content.footer = preloadFromJSON(content.footer);
	}
	if(content.tabs != undefined)
	{
		for(var i = 0; i < content.tabs.length; i++)
		{
			content.tabs[i] = preloadFromJSON(content.tabs[i]);
		}
	}
	if(content.sections != undefined)
	{
		for(var i = 0; i < content.sections.length; i++)
		{
			content.sections[i] = preloadFromJSON(content.sections[i]);
		}
	}
	if(content.columns != undefined)
	{
		for(var i = 0; i < content.columns.length; i++)
		{
			content.columns[i] = preloadFromJSON(content.columns[i]);
		}
	}
	if(content.rows != undefined)
	{
		for(var i = 0; i < content.rows.length; i++)
		{
			content.rows[i] = preloadFromJSON(content.rows[i]);
		}
	}
	if(content.subviews != undefined)
	{
		for(var i = 0; i < content.subviews.length; i++)
		{
			content.subviews[i] = preloadFromJSON(content.subviews[i]);
		}
	}
	return content;
}

//---------------------------------------------//

function preloadFromXML(content)
{
}

//---------------------------------------------//

function load(params, owner)
{
	var controller = {};
	var callback = undefined;
	if(owner != undefined)
	{
		switch(owner.className)
		{
			case 'Ti.UI.TabGroup':
			case 'TiUITabGroupProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.Tab':
						case 'TiUITabProxy':
							child.superview = owner;
							owner.addTab(child);
						break;
						default:
							child.superview = owner;
							owner.add(child);
						break;
					}
				};
			break;
			case 'Ti.UI.Tab':
			case 'TiUITabProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.Window':
						case 'TiUIWindowProxy':
							child.superview = owner;
							child.window = owner;
						break;
						default:
							child.superview = owner;
							owner.add(child);
						break;
					}
				};
			break;
			case 'Ti.UI.Window':
			case 'TiUIWindowProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.TabGroup':
						case 'TiUITabGroupProxy':
						break;
						default:
							child.superview = owner;
							owner.add(child);
						break;
					}
				};
			break;
			case 'Ti.UI.ScrollableView':
			case 'TiUIScrollableViewProxy':
				callback = function(child)
				{
					child.superview = owner;
					owner.addView(child);
				};
			break;
			case 'Ti.UI.TableView':
			case 'TiUITableViewProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.SearchBar':
						case 'TiUISearchBarProxy':
							child.superview = owner;
							owner.search = child;
						break;
						case 'Ti.UI.TableViewSection':
						case 'TiUITableViewSectionProxy':
							child.superview = owner;
							var sections = owner.data;
							sections.push(child);
							owner.data = sections;
						break;
						case 'Ti.UI.TableViewRow':
						case 'TiUITableViewRowProxy':
							child.superview = owner;
							owner.appendRow(child);
						break;
						default:
							throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
					}
				};
			break;
			case 'Ti.UI.TableViewSection':
			case 'TiUITableViewSectionProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.TableViewRow':
						case 'TiUITableViewRowProxy':
							child.superview = owner;
							owner.add(child);
						break;
						default:
							throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
					}
				};
			break;
			case 'Ti.UI.PickerColumn':
			case 'TiUIPickerColumnProxy':
				callback = function(child)
				{
					switch(child.className)
					{
						case 'Ti.UI.PickerRow':
						case 'TiUIPickerRowProxy':
							child.superview = owner;
							owner.addRow(child);
						break;
						default:
							throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
					}
				};
			break;
			default:
				callback = function(child)
				{
					child.superview = owner;
					owner.add(child);
				};
			break;
		}
	}
	loadFromController(params, controller, callback);
	return controller;
}

//---------------------------------------------//

function loadFromController(params, controller, callback)
{
	if(TiTools.Object.isArray(params) == true)
	{
		for(var i = 0; i < params.length; i++)
		{
			loadFromController(params[i], controller, callback);
		}
	}
	else if(TiTools.Object.isObject(params) == true)
	{
		var current = TiTools.Platform.appropriate(params);
		if(current == undefined)
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_PLATFORM'));
		}
		loadFromController(current, controller, callback);
	}
	else if(TiTools.Object.isString(params) == true)
	{
		loadFromFilename(params, controller, callback);
	}
}

function loadFromFilename(filename, controller, callback)
{
	var content = preloadGet(filename);
	if(content == undefined)
	{
		content = preloadFromFilename(filename);
	}
	if(content != undefined)
	{
		if(TiTools.String.isSuffix(filename, '.js') == true)
		{
			if(TiTools.Object.isArray(content) == true)
			{
				for(var i = 0; i < content.length; i++)
				{
					loadFromJSON(content[i], controller, callback);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				loadFromJSON(content, controller, callback);
			}
		}
		else if(TiTools.String.isSuffix(filename, '.json') == true)
		{
			if(TiTools.Object.isArray(content) == true)
			{
				for(var i = 0; i < content.length; i++)
				{
					loadFromJSON(content[i], controller, callback);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				loadFromJSON(content, controller, callback);
			}
		}
		else if(TiTools.String.isSuffix(filename, '.xml') == true)
		{
			if(TiTools.Object.isArray(content) == true)
			{
				for(var i = 0; i < content.length; i++)
				{
					loadFromXML(content[i], controller, callback);
				}
			}
			else if(TiTools.Object.isObject(content) == true)
			{
				loadFromXML(content, controller, callback);
			}
		}
		else
		{
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_EXTENSION') + '\n' + filename);
		}
	}
	return controller;
}

//---------------------------------------------//

function loadFromJSON(content, controller, callback)
{
	var outlet = undefined;
	switch(content.style.className)
	{
		case 'Ti.UI.AlertDialog':
		case 'TiUIAlertDialogProxy':
			outlet = TiTools.UI.Controls.createAlertDialog(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.EmailDialog':
		case 'TiUIEmailDialogProxy':
			outlet = TiTools.UI.Controls.createEmailDialog(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ActivityIndicator':
		case 'TiUIActivityIndicatorProxy':
			outlet = TiTools.UI.Controls.createActivityIndicator(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.TabGroup':
		case 'TiUITabGroupProxy':
			outlet = TiTools.UI.Controls.createTabGroup(content.style);
			if(content.tabs != undefined)
			{
				for(var i = 0; i < content.tabs.length; i++)
				{
					loadFromJSON(
						content.tabs[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.Tab':
								case 'TiUITabProxy':
									child.superview = outlet;
									outlet.addTab(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
			}
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.add(child);
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Tab':
		case 'TiUITabProxy':
			outlet = TiTools.UI.Controls.createTab(content.style);
			if(content.root != undefined)
			{
				loadFromJSON(
					content.root,
					controller,
					function(child)
					{
						switch(child.className)
						{
							case 'Ti.UI.Window':
							case 'TiUIWindowProxy':
								child.superview = outlet;
								outlet.window = child;
							break;
							default:
								throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
						}
					}
				);
			}
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.add(child);
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Window':
		case 'TiUIWindowProxy':
			outlet = TiTools.UI.Controls.createWindow(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.View':
		case 'TiUIViewProxy':
			outlet = TiTools.UI.Controls.createView(content.style);
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.add(child);
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ScrollView':
		case 'TiUIScrollViewProxy':
			outlet = TiTools.UI.Controls.createScrollView(content.style);
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.add(child);
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ScrollableView':
		case 'TiUIScrollableViewProxy':
			outlet = TiTools.UI.Controls.createScrollableView(content.style);
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.addView(child);
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ImageView':
		case 'TiUIImageViewProxy':
			outlet = TiTools.UI.Controls.createImageView(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Button':
		case 'TiUIButtonProxy':
			outlet = TiTools.UI.Controls.createButton(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ButtonBar':
		case 'TiUIButtonBarProxy':
			outlet = TiTools.UI.Controls.createButtonBar(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Label':
		case 'TiUILabelProxy':
			outlet = TiTools.UI.Controls.createLabel(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Switch':
		case 'TiUISwitchProxy':
			outlet = TiTools.UI.Controls.createSwitch(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.Slider':
		case 'TiUISliderProxy':
			outlet = TiTools.UI.Controls.createSlider(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.SearchBar':
		case 'TiUISearchBarProxy':
			outlet = TiTools.UI.Controls.createSearchBar(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.ProgressBar':
		case 'TiUIProgressBarProxy':
			outlet = TiTools.UI.Controls.createProgressBar(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.TextField':
		case 'TiUITextFieldProxy':
			outlet = TiTools.UI.Controls.createTextField(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.TextArea':
		case 'TiUITextAreaProxy':
			outlet = TiTools.UI.Controls.createTextArea(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.TableView':
		case 'TiUITableViewProxy':
			outlet = TiTools.UI.Controls.createTableView(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
			if(content.header != undefined)
			{
				loadFromJSON(
					content.header,
					controller,
					function(child)
					{
						child.superview = outlet;
						outlet.headerView = child;
					}
				);
			}
			if(content.footer != undefined)
			{
				loadFromJSON(
					content.footer,
					controller,
					function(child)
					{
						child.superview = outlet;
						outlet.footerView = child;
					}
				);
			}
			if(content.search != undefined)
			{
				loadFromJSON(
					content.search,
					controller,
					function(child)
					{
						child.superview = outlet;
						outlet.search = child;
					}
				);
			}
			if(content.sections != undefined)
			{
				var sections = outlet.data;
				for(var i = 0; i < content.sections.length; i++)
				{
					sections.push(
						loadFromJSON(
							content.sections[i],
							controller,
							function(child)
							{
								switch(child.className)
								{
									case 'Ti.UI.TableViewSection':
									case 'TiUITableViewSectionProxy':
										child.superview = outlet;
									break;
									default:
										throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
								}
							}
						)
					);
				}
				outlet.data = sections;
			}
			else if(content.rows != undefined)
			{
				var rows = [];
				for(var i = 0; i < content.rows.length; i++)
				{
					loadFromJSON(
						content.rows[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.TableViewRow':
								case 'TiUITableViewRowProxy':
									child.superview = outlet;
									rows.push(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
				outlet.appendRow(rows);
			}
		break;
		case 'Ti.UI.TableViewSection':
		case 'TiUITableViewSectionProxy':
			outlet = TiTools.UI.Controls.createTableViewSection(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
			if(content.header != undefined)
			{
				loadFromJSON(
					content.header,
					controller,
					function(child)
					{
						child.superview = outlet;
						outlet.headerView = child;
					}
				);
			}
			if(content.footer != undefined)
			{
				loadFromJSON(
					content.footer,
					controller,
					function(child)
					{
						child.superview = outlet;
						outlet.footerView = child;
					}
				);
			}
			if(content.rows != undefined)
			{
				for(var i = 0; i < content.rows.length; i++)
				{
					loadFromJSON(
						content.rows[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.TableViewRow':
								case 'TiUITableViewRowProxy':
									child.superview = outlet;
									outlet.add(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
			}
		break;
		case 'Ti.UI.TableViewRow':
		case 'TiUITableViewRowProxy':
			outlet = TiTools.UI.Controls.createTableViewRow(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
			if(content.subviews != undefined)
			{
				for(var i = 0; i < content.subviews.length; i++)
				{
					loadFromJSON(
						content.subviews[i],
						controller,
						function(child)
						{
							child.superview = outlet;
							outlet.add(child);
						}
					);
				}
			}
		break;
		case 'Ti.UI.Picker':
		case 'TiUIPickerProxy':
			var childs = [];
			if(content.columns != undefined)
			{
				for(var i = 0; i < content.columns.length; i++)
				{
					loadFromJSON(
						content.columns[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.PickerColumn':
								case 'TiUIPickerColumnProxy':
									childs.push(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
			}
			else if(content.rows != undefined)
			{
				for(var i = 0; i < content.rows.length; i++)
				{
					loadFromJSON(
						content.rows[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.PickerRow':
								case 'TiUIPickerRowProxy':
									childs.push(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
			}
			outlet = TiTools.UI.Controls.createPicker(content.style);
			for(var i = 0; i < childs.length; i++)
			{
				var child = childs[i];
				child.superview = outlet;
			}
			outlet.add(childs);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.PickerColumn':
		case 'TiUIPickerColumnProxy':
			outlet = TiTools.UI.Controls.createPickerColumn(content.style);
			if(content.rows != undefined)
			{
				for(var i = 0; i < content.rows.length; i++)
				{
					loadFromJSON(
						content.rows[i],
						controller,
						function(child)
						{
							switch(child.className)
							{
								case 'Ti.UI.PickerRow':
								case 'TiUIPickerRowProxy':
									outlet.addRow(child);
								break;
								default:
									throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_CLASS_NAME') + ': ' + child.className);
							}
						}
					);
				}
			}
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.PickerRow':
		case 'TiUIPickerRowProxy':
			outlet = TiTools.UI.Controls.createPickerRow(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.WebView':
		case 'TiUIWebViewProxy':
			outlet = TiTools.UI.Controls.createWebView(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.GoogleMapView':
			outlet = TiTools.UI.Controls.createGoogleMapView(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.UI.FacebookLoginButton':
			outlet = TiTools.UI.Controls.createFacebookLoginButton(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		case 'Ti.PaintView':
			outlet = TiTools.UI.Controls.createPaintView(content.style);
			if(callback != undefined)
			{
				callback(outlet);
			}
		break;
		default:
			throw String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_CLASS_NAME') + '\n' + content.style.className);
	}
	if(content.outlet != undefined)
	{
		controller[content.outlet] = outlet;
	}
	return outlet;
}

//---------------------------------------------//

function loadFromXML(content, controller, callback)
{
}

//---------------------------------------------//

module.exports = {
	preload : preload,
	load : load
};
