/**
 * @author Gom_Dzhabbar
*/

var TiTools = undefined;
var info = undefined;
var access_token = "8e6b3a7b47c3346cb7e4db42c88519bc";
var categories = [];
var projects = [];
var indexCategories = 0;
var id_user = undefined;

var achievements = [];
var newCategories = [];
var newProjects = [];

var counter = 0;
//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//
// Обязательные функции
//---------------------------------------------//

// Инициализация контроллера окна
function onInitController(window, params)
{
	TiTools = require("TiTools/TiTools");
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/Login.js", window);
	
	ui.infacebook.addEventListener("click", function(event)
	{
		Titanium.Facebook.appid = "264918200296425";
		Titanium.Facebook.permissions = ['publish_stream', 'read_stream'];
		
		// Ti.Facebook.logout();
		// // clear cookies
		// var client = Ti.Network.createHTTPClient();
		// client.clearCookies('https://login.facebook.com');
		
		Ti.API.info(Titanium.Facebook.loggedIn);
		
		function fQuery() 
		{
			var fbuid = Titanium.Facebook.uid; 
			var myQuery = "SELECT name, birthday_date,current_location FROM user WHERE uid = "+fbuid;
		
		
		Titanium.Facebook.request('fql.query', {query: myQuery},  function(x)
			{
				var results = JSON.parse(x.result);
				info = {
					name: results[0].name,
					birthday: results[0].birthday_date
					//city: results[0].current_location.city,
					//country: results[0].current_location.country
				}
				
				//-------------------
				
				var query = {
					params: JSON.stringify({
						access_token: access_token,
						type: "fb_user_id",
						id: fbuid
					}),
					url: "/playerid.json"
				};
				
				queryItsbeta(query,saveIdUser);
				
			});
		};
		
		if(Titanium.Facebook.loggedIn == 0)
		{
			Ti.Facebook.authorize();
			Ti.Facebook.addEventListener('login',function(event){
				
				actIndicator(true);
				
				Ti.API.info("login");
				fQuery();
			});
		}
		else
		{
			actIndicator(true);
			fQuery();
		}
	});
	
 }
 
//---------------------------------------------//

function queryItsbeta(params,collback)
{
	Ti.API.info('itsbeta');
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
				success: function(success)
						{
							Ti.API.info('ok');
							//Ti.API.info(success.responseText);
							collback(success.responseText);
						},
				failure: function(failure)
						{
							
							Ti.API.info('error');
							//Ti.API.info(failure.responseText);
							collback(failure.responseText);
						}
			}
		);
}
//-----collback-----
function saveIdUser(data)
{
	Ti.App.Properties.setString("id_user",JSON.parse(data).player_id);
	Ti.API.info('saveID');
	id_user = JSON.parse(data).player_id;
	
	Ti.API.info('загрузка категорий');
	var query = {
		params: JSON.stringify({
			player_id : id_user,
			access_token : access_token
		}),
		url : "/achievements.json"
	};
	
	queryItsbeta(query,saveAchivs);
}
function saveAchivs(data)
{
	indexCategories++;
	achievements = JSON.parse(data);
	
	//---- собираем список категорий и список проектов -----//
	
	for(var i = 0; i < achievements.length; i++)
	{
		categories.push(
			{
				api_name: achievements[i].api_name,
				display_name: achievements[i].display_name
			}
		);
		
		for(var j = 0; j < achievements[i].projects.length; j++)
		{
			counter += achievements[i].projects[j].achievements.length;
			
			projects.push(
				{
					api_name: achievements[i].projects[j].api_name,
					display_name: achievements[i].projects[j].display_name,
					total_badge: achievements[i].projects[j].total_badge
				}
			);
		}
	}
	
	//------------------------------------------------------//
	
	var win = TiTools.UI.Controls.createWindow(
		{
			main : "Controllers/GeneralWindow.js",
			navBarHidden : true,
			info: info,
			achievements : achievements,
			categories : categories,
			projects : projects,
			counter : counter
		}
	);
	win.initialize();
	win.open();
}
function actIndicator(param)
{
	if(param == true)
	{
		ui.actView.show();
		ui.act.show();
	}
	else
	{
		ui.actView.hide();
		ui.act.hide();
	}
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	
}

// Обработчик при закрытии окна
function onWindowClose(window, event)
{
	
}
//---------------------------------------------//

module.exports = {
	onInitController : onInitController, // Обязательный параметр
	onWindowOpen : onWindowOpen,
	onWindowClose : onWindowClose
};