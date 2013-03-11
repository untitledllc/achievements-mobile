//---------------------------------------------//

function isInt(str)
{
	var regExp = /^\d+$/;
	return regExp.test(str);
}

//---------------------------------------------//

function isFloat(str)
{
	var regExp = /^\d*\.\d+$|^\d+\.\d*$/;
	return regExp.test(str);
}

//---------------------------------------------//

function needsQuoting(str)
{
	var regExp = /^\s|\s$|,|"|\n/;
	return regExp.test(str);
}

//---------------------------------------------//

function trim(str)
{
	if(typeof(str) == 'string')
	{
		var begin = 0;
		var end = str.length - 1;
		while((begin <= end) && (str.charCodeAt(begin) < 33))
		{
			++begin;
		}
		while((end > begin) && (str.charCodeAt(end) < 33))
		{
			--end;
		}
		return str.substr(begin, end - begin + 1);
	}
	return "";
}

//---------------------------------------------//

function paddingLeft(str, length, chars)
{
	str = String(str);
	while(str.length < length)
	{
		str = chars + str;
	}
	return str;
}

//---------------------------------------------//

function paddingRight(str, length, chars)
{
	str = String(str);
	while(str.length < length)
	{
		str = str + chars;
	}
	return str;
}

//---------------------------------------------//

function chomp(str)
{
	var last = str.length - 1;
	if(str.charAt(last) != '\n')
	{
		return str;
	}
	else
	{
		return str.substring(0, last);
	}
}

//---------------------------------------------//

function isPrefix(str, prefix)
{
	if(str.indexOf(prefix) == 0)
	{
		return true;
	}
	return false;
}

//---------------------------------------------//

function isSuffix(str, suffix)
{
	var matched = String(str.match(suffix + '$'));
	if(matched == suffix)
	{
		return true;
	}
	return false;
}

//---------------------------------------------//

function replaceAll(str, search, replace)
{
	return str.split(search).join(replace);
}

//---------------------------------------------//

module.exports = {
	isInt : isInt,
	isFloat : isFloat,
	needsQuoting : needsQuoting,
	trim : trim,
	paddingLeft : paddingLeft,
	paddingRight : paddingRight,
	chomp : chomp,
	isPrefix : isPrefix,
	isSuffix : isSuffix,
	replaceAll : replaceAll
};
