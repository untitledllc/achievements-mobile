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
function code(achievements)//Получение кода активации
{
	var info = TiTools.Global.get("info");
	
	var params = JSON.stringify({
			access_token : ITSBETA_ACCESS_TOKEN,
			category: achievements[1].api_name,
			project: achievements[1].projects[0].api_name,
			badge_name: achievements[1].projects[0].achievements[0].badge_name
		});
		
		Ti.API.info(params);
		
		Ti.API.info('http://www.itsbeta.com/s/'+ achievements[1].api_name +'/'+ achievements[1].projects[0].api_name +'/achieves/postachieve.json')
	
	TiTools.HTTP.response(
		{
			reguest: {
			method: 'POST',
				url: 'http://www.itsbeta.com/s/'+ achievements[1].api_name +'/'+ achievements[1].projects[0].api_name +'/achieves/postachieve.json',
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
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
					}
		}
	);
}

function codePost()//Активация достижения на Facebook
{
	var params = JSON.stringify({
			activation_code : "JfazJR6dkd",
			access_token : access_token,
			category: achievements[1].api_name,
			project: achievements[1].projects[0].api_name,
			user_id : fbuid,
			user_token : accessToken
		});
		
		Ti.API.info(params);
		
		Ti.API.info('http://www.itsbeta.com/s/'+ achievements[1].api_name +'/'+ achievements[1].projects[0].api_name +'/achieves/posttofb.json')
	
	TiTools.HTTP.response(
		{
			reguest: {
			method: 'POST',
				url: 'http://www.itsbeta.com/s/'+ achievements[1].api_name +'/'+ achievements[1].projects[0].api_name +'/achieves/posttofb.json',
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
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
					}
		}
	);
}
function postActiv(data)//Активация по коду активации
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
	
	alert(tempCode);
	
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
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
					}
		}
	);
}
function postActivCode(tempCode)//Активация по коду активации
{
	var info = TiTools.Global.get("info");
	
	alert(tempCode);
	
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
					},
			failure: function(failure)
					{
						Ti.API.info('error');
						Ti.API.info(failure.responseText);
					}
		}
	);
}


module.exports = {
	postActivCode : postActivCode,
	postActiv:postActiv,
	code: code,
	codePost: codePost,
	query: query,
	getAchievementsByUid: getAchievementsByUid
}
