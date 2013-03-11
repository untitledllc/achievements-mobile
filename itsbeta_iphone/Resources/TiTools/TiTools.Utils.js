var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Object', 'Object');
TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');
TiTools.loadLibrary('TiTools/TiTools.Platform', 'Platform');
TiTools.loadLibrary('TiTools/TiTools.JSON', 'JSON');

//---------------------------------------------//

function sleep(time)
{
	var start = new Date().getTime();
	while(true)
	{
		var delta = new Date().getTime() - start;
		if(delta >= time)
		{
			break;
		}
	}
}

//---------------------------------------------//

function info(data)
{
	if(TiTools.Platform.isAndroid == true)
	{
		if(TiTools.Object.isArray(data) == true)
		{
			data = TiTools.JSON.serialize(data);
		}
		else if(TiTools.Object.isObject(data) == true)
		{
			data = TiTools.JSON.serialize(data);
		}
		Ti.API.info('[TiTools]: ' + data);
	}
	else if(TiTools.Platform.isIOS == true)
	{
		alert(data);
	}
}

//---------------------------------------------//

function callPhone(phone)
{
	var alert = Ti.UI.createAlertDialog(
		{
			message : TiTools.Locate.getString('TITOOLS_ALERT_REQUEST_CALL') + '\n' + phone,
			buttonNames : [
				TiTools.Locate.getString('TITOOLS_ALERT_CALL'),
				TiTools.Locate.getString('TITOOLS_ALERT_NO')
			],
			cancel : 1
		}
	);
	alert.addEventListener('click',
		function(event)
		{
			if(event.index == 0)
			{
				var number = phone.replace(/([^0-9])+/g, '');
				if(number.length > 0)
				{
					Ti.Platform.openURL('tel:' + number);
				}
			}
		}
	);
	alert.show();
}

//---------------------------------------------//

/**
	@brief
		Вспомогательная функция для работы с текущими табами,
		возвращает либо то, что на входе, либо текущее значение переменной
		TiTools.currentTab. В эту переменную при каждом вызове функции TiTools.UI.Controls.createTab()
		записывается таб. 
	@param _tab
		Текущий таб или undefined
	@return
		Либо входной параметр либо значение из переменной TiTools.currentTab
**/

function getCurrentTab(_tab)
{
	if( _tab != undefined )
	{
		return _tab;
	}
	else
	{
		return TiTools.currentTab;
	}
}

//---------------------------------------------//

module.exports = {
	sleep : sleep,
	info : info,
	callPhone : callPhone,
	getCurrentTab : getCurrentTab 
};
