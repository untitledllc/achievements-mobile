//---------------------------------------------//

var lastUniqueId = 0;

//---------------------------------------------//

function unigueID()
{
	lastUniqueId++;
	return lastUniqueId;
}

//---------------------------------------------//

function combine(objectA, objectB)
{
	var result = clone(objectA);
	if(objectB != undefined)
	{
		for(var i in objectB)
		{
			if(isArray(objectB[i]) == true)
			{
				if(result[i] == undefined)
				{
					result[i] = [];
				}
				result[i] = combine(result[i], objectB[i]);
			}
			else if(isObject(objectB[i]) == true)
			{
				if(result[i] == undefined)
				{
					result[i] = {};
				}
				result[i] = combine(result[i], objectB[i]);
			}
			else
			{
				result[i] = objectB[i];
			}
		}
	}
	return result;
}

//---------------------------------------------//

function clone(object)
{
	if(object != undefined)
	{
		var result = undefined;
		if(isArray(object) == true)
		{
			result = [];
		}
		else if(isObject(object) == true)
		{
			result = {};
		}
		if(result != undefined)
		{
			for(var prop in object)
			{
				if(object[prop] != undefined)
				{
					var field = object[prop];
					if(isObject(field) == true)
					{
						result[prop] = clone(field);
					}
					else
					{
						result[prop] = field;
					}
				}
			}
			return result;
		}
	}
	return object;
}

//---------------------------------------------//

function swap(object, paramA, paramB)
{
	var temp = object[paramA];
	object[paramA] = object[paramB];
	object[paramB] = temp;
}

//---------------------------------------------//

function isFunction(object)
{
	return (typeof(object) == 'function');
}

//---------------------------------------------//

function isObject(object)
{
	if(object == undefined)
	{
		return false;
	}
	return (object.toString() == '[object Object]');
}

//---------------------------------------------//

function isArray(object)
{
	if(object == undefined)
	{
		return false;
	}
	return (Object.prototype.toString.call(object) == '[object Array]');
}

//---------------------------------------------//

function isNumber(object)
{
	return (typeof(object) == 'number');
}

//---------------------------------------------//

function isString(object)
{
	return (typeof(object) == 'string');
}

//---------------------------------------------//

module.exports = {
	unigueID : unigueID,
	combine : combine,
	clone : clone,
	swap : swap,
	isFunction : isFunction,
	isObject : isObject,
	isArray : isArray,
	isNumber : isNumber,
	isString : isString
};
