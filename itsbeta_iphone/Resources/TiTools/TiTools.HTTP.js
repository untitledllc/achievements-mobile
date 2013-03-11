var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.Locate', 'Locate');

//---------------------------------------------//

var httpHandle = undefined;

//---------------------------------------------//

function isOnline()
{
	return Ti.Network.online;
}

function hasRequest()
{
	if(httpHandle == undefined)
	{
		return true;
	}
	return false;
}

function response(params)
{
	if(params == undefined)
	{
		throw new String(TiTools.Locate.getString('TITOOLS_THROW_BAD_PARAMS'));
	}
	if(httpHandle != undefined)
	{
		throw new String(TiTools.Locate.getString('TITOOLS_THROW_HHTP_SINGLETON'));
	}
	switch(params.reguest.method)
	{
		case 'GET':
		case 'POST':
		break;
		default:
			throw new String(TiTools.Locate.getString('TITOOLS_THROW_UNKNOWN_METHOD'));
	}
	var handle = Ti.Network.createHTTPClient(
		{
			timeout : 30000,
			enableKeepAlive : false,
			onload : function(event)
			{
				var handle = httpHandle;
				httpHandle = undefined;
				if(params.success != undefined)
				{
					params.success(handle);
				}
			},
			onsendstream : function(event)
			{
				if(params.sendProgress != undefined)
				{
					params.sendProgress(event.progress);
				}
			},
			ondatastream : function(event)
			{
				if(params.readProgress != undefined)
				{
					params.readProgress(event.progress);
				}
			},
			onerror : function(event)
			{
				var handle = httpHandle;
				httpHandle = undefined;
				if(params.failure != undefined)
				{
					params.failure(handle);
				}
			}
		}
	);
	var url = params.reguest.url;
	if(params.reguest.args != undefined)
	{
		var count = 0;
		for(var i in params.reguest.args)
		{
			var item = params.reguest.args[i];
			if(item != undefined)
			{
				url += (((count == 0) ? '?' : '&') + i + '=' + item);
				count++;
			}
		}
	}
	switch(params.reguest.method)
	{
		case 'GET': handle.open('GET', url); break;
		case 'POST': handle.open('POST', url); break;
		default: break;
	}
	if(params.reguest.header != undefined)
	{
		for(var j = 0; j < params.reguest.header.length; j++)
		{
			var header = params.reguest.header[j];
			handle.setRequestHeader(header.type, header.value);
		}
	}
	switch(params.reguest.method)
	{
		case 'GET': handle.send(); break;
		case 'POST': handle.send(params.reguest.post); break;
		default: break;
	}
	httpHandle = handle;
	return httpHandle;
}

function abort()
{
	if(httpHandle != undefined)
	{
		httpHandle.abort();
		httpHandle = undefined;
	}
}

//---------------------------------------------//

module.exports = {
	isOnline : isOnline,
	hasRequest : hasRequest,
	response : response,
	abort : abort
};