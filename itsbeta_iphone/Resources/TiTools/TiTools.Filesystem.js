var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');
TiTools.loadLibrary('TiTools/TiTools.Platform', 'Platform');

//---------------------------------------------//

if(Ti.App.TiToolsFilesystemPath == undefined)
{
	var resourcesPath = TiTools.Platform.appropriate(
		{
			android : 'file:///android_asset/Resources/',
			ios : Ti.Filesystem.resourcesDirectory
		}
	);
	var controllersPath = TiTools.Platform.appropriate(
		{
			android : '',
			ios : Ti.Filesystem.resourcesDirectory
		}
	);
	Ti.App.TiToolsFilesystemPath = {
		resourcesPath : resourcesPath,
		controllersPath : controllersPath
	};
}

//---------------------------------------------//

/**
	@brief
		Замена предопределенных путей на абсолютный путь.
		Если файла не сужествует до сработает исключение
	@param path
		Путь до файла
	@return
		Абсолютный путь до файла
**/
function preprocessPath(path)
{
	return path.replace(/%([A-Za-z_]*)%/g,
		function(str, p1, p2, offset, s)
		{
			switch(p1)
			{
				case 'ResourcesPath': return Ti.App.TiToolsFilesystemPath.resourcesPath;
				case 'ControllersPath': return Ti.App.TiToolsFilesystemPath.controllersPath;
				default: break;
			}
			return p1;
		}
	);
}

//---------------------------------------------//

/**
	@brief
		Загрузка модуля.
		Если файла не сужествует то сработает исключение
	@param path
		Путь до модуля
	@return
		Обьект Ti.Module
**/
function loadModule(path)
{
	return require(path.replace(/\.js$/g, ''));
}

//---------------------------------------------//

/**
	@brief
		Получение указателя на файл.
		Если файла не сужествует то сработает исключение
	@param path
		Путь до файла
	@return
		Обьект Ti.Filesystem.File
**/
function getFile(path)
{
	return Ti.Filesystem.getFile(preprocessPath(path));
}

//---------------------------------------------//

module.exports = {
	preprocessPath : preprocessPath,
	loadModule : loadModule,
	getFile : getFile
};
