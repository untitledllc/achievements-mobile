var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary("TiTools/TiTools.Object", "Object");
TiTools.loadLibrary("TiTools/TiTools.String", "String");

//---------------------------------------------//

/**
	@brief
		Получение текущего времени со сдвигом
	@param offset : {
			year : Number, // Смещение года
			month : Number, // Смещение месяца
			day : Number, // Смещение дня
			hour : Number, // Смещение часа
			minute : Number, // Смещение минут
			second : Number // Смещение секунд
		}
	@return
		Количество прошедших миллисекунд с 1 января 1970
**/
function now(offset)
{
	var date = new Date();
	if(offset != undefined)
	{
		if(offset.year != undefined)
		{
			date.setFullYear(date.getFullYear() + offset.year);
		}
		if(offset.month != undefined)
		{
			date.setMonth(date.getMonth() + offset.month);
		}
		if(offset.day != undefined)
		{
			date.setDate(date.getDate() + offset.day);
		}
		if(offset.hour != undefined)
		{
			date.setHours(date.getHours() + offset.hour);
		}
		if(offset.minute != undefined)
		{
			date.setMinutes(date.getMinutes() + offset.minute);
		}
		if(offset.second != undefined)
		{
			date.setSecond(date.getSecond() + offset.second);
		}
	}
	return date.getTime();
}

//---------------------------------------------//

/**
	@brief
		Получение текущего времени и установка фиксированных значений
	@param params : {
			year : Number, // Установка года
			month : Number, // Установка месяца
			day : Number, // Установка дня
			hour : Number, // Установка часа
			minute : Number, // Установка минут
			second : Number // Установка секунд
		}
	@return
		Количество прошедших миллисекунд с 1 января 1970
**/
function make(params)
{
	var date = new Date();
	if(params != undefined)
	{
		if(params.year != undefined)
		{
			date.setFullYear(params.year);
		}
		if(params.month != undefined)
		{
			date.setMonth(params.month);
		}
		if(params.day != undefined)
		{
			date.setDate(params.day);
		}
		if(params.hour != undefined)
		{
			date.setHours(params.hour);
		}
		if(params.minute != undefined)
		{
			date.setMinutes(params.minute);
		}
		if(params.second != undefined)
		{
			date.setSecond(params.second);
		}
	}
	return date.getTime();
}

//---------------------------------------------//

/**
	@brief
		Подсчет количества прошедших дней между датами
	@param a
		Количество прошедших миллисекунд с 1 января 1970
	@param b
		Количество прошедших миллисекунд с 1 января 1970
	@return
		Количество дней между датами
**/
function betweenOfDays(a, b)
{
	return Math.round((b - a) / (1000 * 60 * 60 * 24));
}

//---------------------------------------------//

/**
	@brief
		Форматирование даты в строку
	@param date
		Количество прошедших миллисекунд с 1 января 1970
	@param mask
		Маска по которой происходит форматирование:
			$yy - Год в формате (00)
			$yyyy - Год в формате (0000)
			$m - Месяц в формате (0)
			$mm - Месяц в формате (00)
			$d - День в формате (0)
			$dd - День в формате (00)
			$h - Часы в формате (0)
			$hh - Часы в формате (00)
			$n - Минуты в формате (0)
			$nn - Минуты в формате (00)
			$s - Секунды в формате (0)
			$ss - Секунды в формате (00)
	@return
		Строка
**/
function format(date, mask)
{
	if(TiTools.Object.isNumber(date) == true)
	{
		date = new Date(date);
	}
	var sy = date.getYear();
	var fy = date.getFullYear();
	var m = date.getMonth() + 1;
	var d = date.getDate();
	var h = date.getHours();
	var n = date.getMinutes();
	var s = date.getSeconds();
	var table = {
		"$yy" : String(sy),
		"$yyyy" : String(fy),
		"$m" : String(m),
		"$mm" : TiTools.String.paddingLeft(m, 2, '0'),
		"$d" : String(d),
		"$dd" : TiTools.String.paddingLeft(d, 2, '0'),
		"$h" : String(h),
		"$hh" : TiTools.String.paddingLeft(h, 2, '0'),
		"$n" : String(n),
		"$nn" : TiTools.String.paddingLeft(n, 2, '0'),
		"$s" : String(s),
		"$ss" : TiTools.String.paddingLeft(s, 2, '0')
	};
	return mask.replace(/\$[A-Za-z]*/g,
		function(str, p1, p2, offset, s)
		{
			if(table[str] != undefined)
			{
				return table[str];
			}
			return str;
		}
	);
}

//---------------------------------------------//

module.exports = {
	now : now,
	make : make,
	betweenOfDays : betweenOfDays,
	format : format
};
