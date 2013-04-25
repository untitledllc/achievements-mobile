var TiTools = require("TiTools/TiTools");

var time = undefined;

var ITSBETA_ACCESS_TOKEN = "8e6b3a7b47c3346cb7e4db42c88519bc";

function query(params, successCallback, failureCallback)
{
	timeOut();
	
	TiTools.HTTP.response(
		{
			reguest: {
				method: 'POST',
				url: params.url,
				header: [
					{
						type: 'Content-Type',
						value: 'application/json; charset=utf-8'
					}
				],
				post: params.params
			},
			success: function(success)
			{ 
				clearTimeout(time);
				Ti.API.info('finish_query')
				successCallback(success);
			},
			failure: function(failure)
			{ 
				Ti.API.info('finish_query_failed');
				
				clearTimeout(time);
				
				Ti.UI.createAlertDialog({
						message: "Ошибка выполнения запроса!",
						title: "Информация"
					}).show();
					
				Ti.App.fireEvent("actHide");
					
				failureCallback(success);
			}
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
			url: "http://www.itsbeta.com/s/info/playerid.json"
		}, 
		function(response) // success
		{
			var playerId = JSON.parse(response.responseText).player_id;
			TiTools.Global.set("playerId", playerId);
			query(
				{
					params: {
						player_id : playerId,
						access_token : ITSBETA_ACCESS_TOKEN
					},
					url : "http://www.itsbeta.com/s/info/achievements.json"
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
function getAchievementsRefresh(uid, successCallback,time)
{
	var params = undefined;
	Ti.API.info('getAchievementsRefresh')
	params = {
		player_id : TiTools.Global.get("playerId"),
		access_token : ITSBETA_ACCESS_TOKEN,
		updated_at : time
	};
		
	query(
		{
			params: params,
			url : "http://www.itsbeta.com/s/info/achievements.json"
		}, 
		successCallback,   // success
		function(response) // achievements failure
		{
			Ti.API.info("error");
		}
	);
}
function firstStart(info)
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
			url: "http://www.itsbeta.com/s/other/itsbeta/achieves/posttofbonce.json"
		}, 
		function(response) // success
		{
			var temp = JSON.parse(response.responseText);
			if(temp.error == undefined)
			{
				if(temp.id != undefined)
				{
					/////----------------
					query(
						{
							params: {
								access_token: ITSBETA_ACCESS_TOKEN,
								id: temp.id
							},
							url: 'http://www.itsbeta.com/s/info/describe.json'
						}, 
						function(response) // success
						{
							TiTools.Global.set("startAchivs", JSON.parse(response.responseText));
							Ti.App.fireEvent("complite");
						},
						function(response) // playerid failure
						{
							Ti.API.info("error");
						}
					);
					/////-----------------
				}
			}
			else
			{
				Ti.App.fireEvent("complite");
			}
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
	
	timeOut();
	
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
				clearTimeout(time);
				
				var temp = JSON.parse(success.responseText);
				
				if(temp.error == undefined)
				{
					Ti.App.fireEvent("CloseAdd");
					
					Ti.App.fireEvent("reload",{data : temp});
					
				}
				else
				{c
					Ti.UI.createAlertDialog({
						message: "Ошибка выполнения запроса!",
						title: "Информация"
					}).show();
					
					Ti.App.fireEvent("actHide");
				}
			},
			failure: function(failure)
			{
				
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
		
	timeOut();
	
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
				clearTimeout(time);
				Ti.API.info('ok');
				//Ti.API.info(success.responseText);
				
				var temp = JSON.parse(success.responseText);
				if(temp.error == undefined)
				{
					Ti.App.fireEvent("CloseAdd");
					Ti.App.fireEvent("reload",{data : temp});
				}
				else
				{
					Ti.UI.createAlertDialog({
						message: "Ошибка выполнения запроса!",
						title: "Информация"
					}).show();
					
					Ti.App.fireEvent("actHide");
				}
			},
			failure: function(failure)
			{
				
			}
		}
	);
}
// --- timeout --- //
function timeOut()
{
	Ti.API.info("start_adbort");
	time = setTimeout(function()
	{
		Ti.API.info(time);
		TiTools.HTTP.abort();
		alert('Ошибка выполнения запроса!');
		Ti.API.info('abort');
		Ti.App.fireEvent("hideActive");
		Ti.App.fireEvent("actHide");
	}, 30000);
}
//-------------------

module.exports = {
	firstStart: firstStart,
	postActivCode: postActivCode,
	postActiv: postActiv,
	query: query,
	getAchievementsByUid: getAchievementsByUid,
	getAchievementsRefresh: getAchievementsRefresh
}
