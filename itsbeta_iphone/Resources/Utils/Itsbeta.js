var TiTools = require("TiTools/TiTools");

var ITSBETA_ACCESS_TOKEN = "8e6b3a7b47c3346cb7e4db42c88519bc";

function query(params, successCallback, failureCallback)
{
	TiTools.HTTP.response(
		{
			reguest: {
				method: 'POST',
				url: 'http://www.itsbeta.com/s/info/' + params.url,
				header: [
					{
						type: 'Content-Type',
						value: 'application/json; charset=utf-8'
					}
				],
				post: params.params
			},
			success: successCallback,
			failure: failureCallback
		}
	);
}

function getAchievementsByUid(uid, successCallback)
{
	query(
		{
			params: {
				access_token: ITSBETA_ACCESS_TOKEN,
				type: "fb_user_id",
				id: uid
			},
			url: "playerid.json"
		}, 
		function(response) // success
		{
			var playerId = JSON.parse(response.responseText).player_id;
		
			query(
				{
					params: {
						player_id : playerId,
						access_token : ITSBETA_ACCESS_TOKEN
					},
					url : "achievements.json"
				}, 
				successCallback,   // success
				function(response) // achievements failure
				{
					Ti.API.info("error");
				}
			);
		},
		function(response) // playerid failure
		{
			Ti.API.info("error");
		}
	);
}
function firstStart(info, successCallback)
{
	query(
		{
			params: {
				access_token: ITSBETA_ACCESS_TOKEN,
				category: "other",
				project: "itsbeta",
				badge_name: "itsbeta",
				user_id: info.fbuid,
				user_token: info.accessToken
			},
			url: "/other/itsbeta/achieves/posttofbonce.json"
		}, 
		function(response) // success
		{
			Ti.API.info(response);
			successCallback;  // success
		},
		function(response) // playerid failure
		{
			Ti.API.info("error");
		}
	);
}
function postActiv(data)//Активация по qr-коду активации
{
	var info = TiTools.Global.get("info");
	
	var tempCode = "";
	var flag = false;
	
	for(var i = 0; i < data.length; i++)
	{
		if(data[i] == "=")
		{
			flag = true;
			continue;
		}
		if(flag == true)
		{
			tempCode += data[i];
		}
	}
	
	var params = {
			activation_code : tempCode,
			user_id : info.fbuid,
			user_token : info.accessToken
		};
	Ti.API.info(params);
	
	TiTools.HTTP.response(
		{
			reguest: {
			method: 'POST',
				url: 'http://www.itsbeta.com/s/activate.json',
				header: [
					{
						type: 'Content-Type',
						value: 'application/json; charset=utf-8'
					}
				],
				post: params
			},
			success: function(success)
					{
						Ti.API.info('ok');
						Ti.API.info(success.responseText);
						
						var temp = JSON.parse(success.responseText);
						
						if(temp.error == undefined)
						{
							Ti.UI.createAlertDialog({
								message: "Выполнено!",
								title: "Информация"
							}).show();
							
							Ti.App.fireEvent("reload");
							
						}else
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
							
							Ti.App.fireEvent("actHide");
						}
						
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
						
						var temp = JSON.parse(success.responseText);
						
						if(temp.error == undefined)
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
						}else
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
						}
						
						Ti.App.fireEvent("actHide");
					}
		}
	);
}
function postActivCode(tempCode)//Активация по коду активации
{
	var info = TiTools.Global.get("info");
	
	
	var params = {
			activation_code : tempCode,
			user_id : info.fbuid,
			user_token : info.accessToken
		};
		
	Ti.API.info(params);
	
	TiTools.HTTP.response(
		{
			reguest: {
			method: 'POST',
				url: 'http://www.itsbeta.com/s/activate.json',
				header: [
					{
						type: 'Content-Type',
						value: 'application/json; charset=utf-8'
					}
				],
				post: params
			},
			success: function(success)
					{
						Ti.API.info('ok');
						Ti.API.info(success.responseText);
						
						var temp = JSON.parse(success.responseText);
						
						if(temp.error == undefined)
						{
							Ti.UI.createAlertDialog({
								message: "Выполнено!",
								title: "Информация"
							}).show();
							
							Ti.App.fireEvent("reload");
							
						}else
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
							
							Ti.App.fireEvent("actHide");
						}
						
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
						
						var temp = JSON.parse(success.responseText);
						
						if(temp.error == undefined)
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
						}else
						{
							Ti.UI.createAlertDialog({
								message: "Ошибка!",
								title: "Информация"
							}).show();
						}
						
						Ti.App.fireEvent("actHide");
					}
		}
	);
}


module.exports = {
	firstStart : firstStart,
	postActivCode : postActivCode,
	postActiv:postActiv,
	query: query,
	getAchievementsByUid: getAchievementsByUid
}
