/* Описание модуля
 * Это основной модуль TiTools, отвечает за инициализацию и загрузку библиотек
 */

//---------------------------------------------//

/**
	@brief
		Загрузка какой-нибудь библиотеки
	@param 
		Переменное число аргументов, первый - путь к модулю, 
		остальные - пространство в TiTools, к которому добавляются модули. 
		Например loadLibrary("TiTools/TiTools.UI.Preset", "UI", "Preset");
	@return
		false - не удалось загрузить библиотеку, true - удалось
**/
function loadLibrary()
{
	if(arguments.length < 2)
	{
		return false;
	}
	
	var isLoaded = true;
	var temp = module.exports;
	
	for(var i = 1; i < arguments.length; ++i)
	{
		if( temp[arguments[i]] == undefined )
		{
			isLoaded = false;
			break;
		}
		temp = temp[arguments[i]];
	}
	
	if(isLoaded == false)
	{
		temp = module.exports;
		for(var i = 1; i < arguments.length-1; ++i)
		{
			if(temp[arguments[i]] == undefined)
			{
				temp[arguments[i]] = {};
			}
			temp = temp[arguments[i]];
		}
		temp[arguments[arguments.length-1]] = require(arguments[0]);
	}
	return true;
}

/**
	@brief
		Загрузка всех библиотек TiTools
	@return
		ничего
**/
function initLibraries()
{
	loadLibrary("TiTools/TiTools.Object", "Object");
	loadLibrary("TiTools/TiTools.String", "String");
	loadLibrary("TiTools/TiTools.DateTime", "DateTime");
	loadLibrary("TiTools/TiTools.Global", "Global");
	loadLibrary("TiTools/TiTools.Storage", "Storage");
	loadLibrary("TiTools/TiTools.Locate", "Locate");
	loadLibrary("TiTools/TiTools.Filesystem", "Filesystem");
	loadLibrary("TiTools/TiTools.Platform", "Platform");
	loadLibrary("TiTools/TiTools.HTML", "HTML");
	loadLibrary("TiTools/TiTools.HTTP", "HTTP");
	loadLibrary("TiTools/TiTools.JSON", "JSON");
	loadLibrary("TiTools/TiTools.XML", "XML");
	loadLibrary("TiTools/TiTools.CSV", "CSV");
	loadLibrary("TiTools/TiTools.Utils", "Utils");
	loadLibrary("TiTools/TiTools.GeoLocation", "GeoLocation");
	
	loadLibrary("TiTools/TiTools.UI.Controls", "UI", "Controls");
	loadLibrary("TiTools/TiTools.UI.Loader", "UI", "Loader");
	loadLibrary("TiTools/TiTools.UI.Prefab", "UI", "Prefab");
	loadLibrary("TiTools/TiTools.UI.Preset", "UI", "Preset");
}

//---------------------------------------------//

module.exports = {
	loadLibrary : loadLibrary,
	initLibraries : initLibraries
}
