var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');

//---------------------------------------------//

var getStringProc = undefined;
if(Ti.Locate != undefined)
{
	getStringProc = Ti.Locate.getString;
}
if(TiTools.Object.isFunction(getStringProc) == false)
{
	getStringProc = L;
}

//---------------------------------------------//

function getString(key, defaults)
{
	if(defaults == undefined)
	{
		defaults = key;
	}
	if(TiTools.Object.isFunction(getStringProc) == true)
	{
		return getStringProc(key, defaults);
	}
	return defaults;
}

//---------------------------------------------//

module.exports = {
	getString : getString
};
