var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary("TiTools/TiTools.Object", "Object");

//---------------------------------------------//

var globalNames  = [];
var globalValues = [];

//---------------------------------------------//

function set(name, value)
{
	var index = globalNames.indexOf(name);
	if(index > -1)
	{
		if(value != undefined)
		{
			globalNames.push(name);
			globalValues.push(value);
		}
	}
	else
	{
		if(value != undefined)
		{
			globalValues[index] = value;
		}
		else
		{
			globalNames.splice(index, 1);
			globalValues.splice(index, 1);
		}
	}
}

//---------------------------------------------//

function get(name)
{
	var index = globalNames.indexOf(name);
	if(index > -1)
	{ 
		return globalValues[index];
	}
	return undefined;
}

//---------------------------------------------//

module.exports = {
	set : set,
	get : get
};
