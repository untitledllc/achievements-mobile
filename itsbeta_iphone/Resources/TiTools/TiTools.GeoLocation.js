var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.HTTP', 'HTTP');
TiTools.loadLibrary('TiTools/TiTools.JSON', 'JSON');

//---------------------------------------------//

/**
	@brief
		Конфигурирование сервиса геолокации
	@param params : {
			message : String, // Текст сообщения при запросе у пользователя разрешиния
			provider : Number, // Метод
			accuracy : Number, // Точность отределения
			distanceFilter : Number, // Минимальная чувствительность срабатыния
		}
	@return
		Обьект Ti.Filesystem.File
**/
function configure(params)
{
	if(params.message != undefined)
	{
		Ti.Geolocation.purpose = params.message;
	}
	if(params.provider != undefined)
	{
		Ti.Geolocation.preferredProvider = params.provider;
	}
	if(params.accuracy != undefined)
	{
		Ti.Geolocation.accuracy = params.accuracy;
	}
	if(params.distanceFilter != undefined)
	{
		Ti.Geolocation.distanceFilter = params.distanceFilter;
	}
}

//---------------------------------------------//

function currentPosition(params)
{
	function currentPositionCallback(event)
	{
		try
		{
			if(event.success == true)
			{
				if(params.success != undefined)
				{
					params.success(
						{
							longitude : event.coords.longitude,
							latitude : event.coords.latitude
						}
					);
				}
			}
			else if(event.error != undefined)
			{
				if(params.failure != undefined)
				{
					params.failure(
						{
							code : event.code,
							message : event.error
						}
					);
				}
			}
		}
		catch(error)
		{
			if(params.except != undefined)
			{
				params.except(error);
			}
		}
		Ti.Geolocation.removeEventListener('location', currentPositionCallback);
	}
	
	if(Ti.Geolocation.locationServicesEnabled == true)
	{
		Ti.Geolocation.getCurrentPosition(currentPositionCallback);
	}
	else
	{
		currentPositionCallback();
	}
}

//---------------------------------------------//

function currentLocation(params)
{
	try
	{
		if(TiTools.HTTP.isOnline() == false)
		{
			if(params.failure != undefined)
			{
				params.failure(status);
			}
			return;
		}
		TiTools.HTTP.response(
			{
				reguest : {
					method : 'GET',
					url : 'http://maps.googleapis.com/maps/api/geocode/json',
					header : [
						{
							type : 'Content-Type',
							value : 'application/json; charset=utf-8'
						},
						{
							type : 'Cache-Control',
							value : 'no-cache, must-revalidate'
						}
					],
					args : {
						latlng : params.position.latitude + ',' + params.position.longitude,
						sensor : 'false',
						language : 'ru'
					}
				},
				success : function(response)
				{
					try
					{
						json = TiTools.JSON.deserialize(response.responseText);
						switch(json.status)
						{
							case 'OK':
								var location = {};
								if(json.results != undefined)
								{
									if(json.results.length > 0)
									{
										location = {
											address : json.results[0].formatted_address,
											componet : {}
										};
										for(var i = 0; i < json.results[0].address_components.length; i++)
										{
											if(json.results[0].address_components[i].types.length > 0)
											{
												location.componet[json.results[0].address_components[i].types[0]] = json.results[0].address_components[i].long_name;
											}
										}
									}
								}
								if(params.success != undefined)
								{
									params.success(location);
								}
							break;
							default:
							break;
						}
					}
					catch(error)
					{
						if(params.except != undefined)
						{
							params.except(error);
						}
					}
				},
				failure : function(status)
				{
					if(params.failure != undefined)
					{
						params.failure(status);
					}
				}
			}
		);
	}
	catch(error)
	{
		if(params.except != undefined)
		{
			params.except();
		}
	}
}

//---------------------------------------------//

function paveRoute(params)
{
	try
	{
		if(TiTools.HTTP.isOnline() == false)
		{
			if(params.failure != undefined)
			{
				params.failure(status);
			}
			return;
		}
		TiTools.HTTP.response(
			{
				reguest : {
					method : 'GET',
					url : 'http://maps.google.com/',
					args : {
						saddr : params.a.latitude + ',' + params.a.longitude,
						daddr : params.b.latitude + ',' + params.b.longitude,
						output : 'kml',
						doflg : 'ptk',
						dirflg : 'w',
						hl : 'en'
					}
				},
				success : function(response)
				{
					try
					{
						var route = {
							name : params.name,
							points : []
						};
						var xml = response.responseXML;
						if(xml != undefined)
						{
							var coords = xml.documentElement.getElementsByTagName('LineString');
							for(var i = 0; i < coords.length; i++)
							{
								var lines = coords.item(i).firstChild.text.split(' ');
								for(var j = 0; j < lines.length; j++)
								{
									var points = lines[j].split(',');
									if((points[0] != undefined) && (points[1] != undefined))
									{
										route.points.push(
											{
												longitude : points[0],
												latitude : points[1]
											}
										);
									}
								}
							}
						}
						if(params.success != undefined)
						{
							params.success(route);
						}
					}
					catch(error)
					{
						if(params.except != undefined)
						{
							params.except(error);
						}
					}
				},
				failure : function(status)
				{
					if(params.failure != undefined)
					{
						params.failure(status);
					}
				}
			}
		);
	}
	catch(error)
	{
		if(params.except != undefined)
		{
			params.except();
		}
	}
}

//---------------------------------------------//

function distance(a, b)
{
	var radius = 6372795.0;
	var cl1 = Math.cos(a.latitude * Math.PI / 180.0);
	var sl1 = Math.sin(a.latitude * Math.PI / 180.0);
	var cl2 = Math.cos(b.latitude * Math.PI / 180.0);
	var sl2 = Math.sin(b.latitude * Math.PI / 180.0);
	var dc = Math.cos((b.longitude - a.longitude) * Math.PI / 180.0);
	var ds = Math.sin((b.longitude - a.longitude) * Math.PI / 180.0);
	var yy = Math.sqrt(Math.pow(cl2 * ds, 2.0) + Math.pow(cl1 * sl2 - sl1 * cl2 * dc, 2.0));
	var xx = sl1 * sl2 + cl1 * cl2 * dc;
	return Math.atan2(yy, xx) * radius;
}

//---------------------------------------------//

module.exports = {
	configure : configure,
	currentPosition : currentPosition,
	currentLocation : currentLocation,
	paveRoute : paveRoute,
	distance : distance
};
