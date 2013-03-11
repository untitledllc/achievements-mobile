
//---------------------------------------------//

if(Ti.App.TiToolsHtmlStyleSheet == undefined)
{
	Ti.App.TiToolsHtmlStyleSheet = [];
}

//---------------------------------------------//

function styleSheetAdd(name, style)
{
	var list = Ti.App.TiToolsHtmlStyleSheet;
	list.push(
		{
			name : name,
			style : style
		}
	);
	Ti.App.TiToolsHtmlStyleSheet = list;
}

//---------------------------------------------//

function styleSheetSet(name, style)
{
	var list = Ti.App.TiToolsHtmlStyleSheet;
	for(var i = 0; i < list.length; i++)
	{
		if(list[i].name == name)
		{
			list[i].style = style;
			break;
		}
	}
}

//---------------------------------------------//

function styleSheetGet(name)
{
	var list = Ti.App.TiToolsHtmlStyleSheet;
	for(var i = 0; i < list.length; i++)
	{
		if(list[i].name == name)
		{
			return ' ' + list[i].style;
		}
	}
	return '';
}

//---------------------------------------------//

function styleSheetRemove(name)
{
	var list = Ti.App.TiToolsHtmlStyleSheet;
	for(var i = 0; i < list.length; i++)
	{
		if(list[i].name == name)
		{
			list.splice(i, 1);
			break;
		}
	}
	Ti.App.TiToolsHtmlStyleSheet = list;
}

//---------------------------------------------//

function styleSheetRemoveAll()
{
	Ti.App.TiToolsHtmlStyleSheet = [];
}

//---------------------------------------------//

function createTable(style, content)
{
	var res = '<table' + styleSheetGet(style) + '>';
	for(var i = 0; i < content.length; i++)
	{
		res += createTableRow(content[i].style, content[i].content);
	}
	res += '</table>';
	return res;
}

//---------------------------------------------//

function createTableRow(style, content)
{
	var res = '<tr' + styleSheetGet(style) + '>';
	for(var i = 0; i < content.length; i++)
	{
		res += createTableCell(content[i].style, content[i].content);
	}
	res += '</tr>';
	return res;
}

//---------------------------------------------//

function createTableCell(style, content)
{
	return '<td' + styleSheetGet(style) + '>' + content + '</td>';
}

//---------------------------------------------//

module.exports = {
	styleSheet : {
		add : styleSheetAdd,
		set : styleSheetSet,
		get : styleSheetGet,
		remove : styleSheetRemove,
		removeAll : styleSheetRemoveAll
	},
	createTable : createTable,
	createTableRow : createTableRow,
	createTableCell : createTableCell
};
