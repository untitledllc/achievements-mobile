var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.String', 'String');

//---------------------------------------------//

/**
	@brief
		Серилизация обьекта в строку
	@param csv
		Двумерный массив обьектов
	@return
		Форматированную строку в формате CSV
**/
function serialize(csv)
{
	var out = "";
	for(var i = 0; i < csv.length; ++i)
	{
		var row = csv[i];
		for(var j = 0; j < row.length; ++j)
		{
			var cur = row[j];
			if(TiTools.Object.isString(cur) == true)
			{
				cur = cur.replace(/"/g, '""');
				if((TiTools.String.needsQuoting(cur) == true) || (TiTools.String.isInt(cur) == true) || (TiTools.String.isFloat(cur) == true))
				{
					cur = '"' + cur + '"';
				}
				else if(cur == '')
				{
					cur = '""';
				}
			}
			else if(TiTools.Object.isNumber(cur) == true)
			{
				cur = cur.toString(10);
			}
			else if(cur == null)
			{
				cur = '';
			}
			else
			{
				cur = cur.toString();
			}
			out += (j < row.length - 1) ? cur + ',' : cur;
		}
		out += '\n';
	}
	return out;
}

//---------------------------------------------//

/**
	@brief
		Десерилизация строки в обьект
	@param str
		Форматированная строка в формате CSV
	@param trim
		Требуется ли нормализовывать значения в ячейках
	@return
		Двумерный массив обьектов
**/
function deserialize(str, trim)
{
	var inQuote = false;
	var fieldQuoted = false;
	var field = '';
	var row = [];
	var out = [];
	
	function deserializeField(field)
	{
		if(fieldQuoted != true)
		{
			if(field == '')
			{
				field = null;
			}
			else if(trim == true)
			{
				field = TiTools.String.trim(field);
			}
			if(TiTools.String.isInt(field) == true)
			{
				field = parseInt(field, 10);
			}
			else if(TiTools.String.isFloat(field) == true)
			{
				field = parseFloat(field, 10);
			}
		}
		return field;
	}
	
	str = TiTools.String.chomp(str);
	for(var i = 0; i < str.length; ++i)
	{
		var cur = str.charAt(i);
		if((inQuote == false) && ((cur == ',') || (cur == '\n')))
		{
			field = deserializeField(field);
			row.push(field);
			if(cur == '\n')
			{
				out.push(row);
				row = [];
			}
			field = '';
			fieldQuoted = false;
		}
		else
		{
			if(cur != '"')
			{
				field += cur;
			}
			else
			{
				if(inQuote == false)
				{
					inQuote = true;
					fieldQuoted = true;
				}
				else
				{
					if(str.charAt(i + 1) == '"')
					{
						field += '"';
						i += 1;
					}
					else
					{
						inQuote = false;
					}
				}
			}
		}
	}
	field = deserializeField(field);
	row.push(field);
	out.push(row);
	return out;
}

//---------------------------------------------//

module.exports = {
	serialize : serialize,
	deserialize : deserialize
};
