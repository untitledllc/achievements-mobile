var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.String', 'String');
TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');
TiTools.loadLibrary('TiTools/TiTools.Filesystem', 'Filesystem');
TiTools.loadLibrary('TiTools/TiTools.Platform', 'Platform');
TiTools.loadLibrary('TiTools/TiTools.JSON', 'JSON');
TiTools.loadLibrary('TiTools/TiTools.XML', 'XML');

//---------------------------------------------//

var prefabsNames  = [];
var prefabsCaches = [];

//---------------------------------------------//

function set(name, prefab)
{
	var index = prefabsNames.indexOf(name);
	if(index > -1)
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_OVERRIDE_PREFABS') + '\n' + name);
	}
	prefabsNames.push(name);
	prefabsCaches.push(prefab); 
}

//---------------------------------------------//

function get(name)
{
	var index = prefabsNames.indexOf(name);
	if( index > -1 )
	{
		return prefabsCaches[index]; 
	}
	return undefined;
}

//---------------------------------------------//

function remove(name)
{
	var index = prefabsNames.indexOf(name);
	if(index > -1)
	{
		prefabsNames.splice(index, 1);
		prefabsCaches.splice(index, 1); 
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
	if((TiTools.Object.isString(content.name) == false) || (TiTools.Object.isObject(content.prefab) == false))
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_PREFAB_FORMAT'));
	}
	set(content.name, content.prefab);
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
	load : load
};
