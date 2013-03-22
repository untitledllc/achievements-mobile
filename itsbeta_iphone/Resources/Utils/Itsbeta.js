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

module.exports = {
	query: query,
	getAchievementsByUid: getAchievementsByUid
}
